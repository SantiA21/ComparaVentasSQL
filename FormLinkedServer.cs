using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;

namespace ComparaVentasExcel
{
    public partial class FormLinkedServer : Form
    {
        private DataAccess dataAccess;
        private string selectedDbKey;
        public FormLinkedServer()
        {
            InitializeComponent();
        }

        private string connectionToMotherServer = "";

        private void btnCargarEquipos_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServidorMadre.Text))
            {
                MessageBox.Show("Debe ingresar la IP / Hostname del servidor.");
                return;
            }


            connectionToMotherServer = $"Server={txtServidorMadre.Text};Database=BACKOFFICE;User Id=sa;Password=cinettorcel;";

            try
            {
                cbEquipos.Items.Clear();

                using (var conn = new SqlConnection(connectionToMotherServer))
                {
                    conn.Open();

                    string query = @"
DECLARE @infoCaja table([caja] varchar(20), [equipo] varchar(259), [version] varchar(200));

INSERT INTO @infoCaja
SELECT DISTINCT caja, EQUIPO, valor 
FROM (
        SELECT 
            RANK() OVER (PARTITION BY caja, parametro ORDER BY fechatrans DESC) AS rango,
            *
        FROM hparamloc
        WHERE parametro = 'VERSION'
) sub
WHERE rango = 1;

SELECT vene_caja, suc_codigo, equipo
FROM (
    SELECT 
        ROW_NUMBER() OVER (PARTITION BY v.vene_caja ORDER BY v.vene_fecha DESC) AS rn,
        v.vene_caja,
        v.suc_codigo,
        i.equipo
    FROM VENTAS_E v
    INNER JOIN @infoCaja i 
        ON v.vene_caja = i.caja COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE v.vene_caja <> ''
) subq
WHERE rn = 1
ORDER BY equipo;
";

                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string equipo = reader["equipo"].ToString();
                            if (!string.IsNullOrWhiteSpace(equipo))
                                cbEquipos.Items.Add(equipo);
                        }
                    }
                }

                if (cbEquipos.Items.Count > 0)
                    cbEquipos.SelectedIndex = 0;

                MessageBox.Show("Equipos cargados correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando equipos: " + ex.Message);
            }
        }

        private string ResolverHost(string host)
        {
            try
            {
                var entry = System.Net.Dns.GetHostEntry(host);
                var ip = entry.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return ip?.ToString() ?? host;
            }
            catch
            {
                return host; // fallback
            }
        }

        private string LimpiarHostname(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return null;

            // Quitar espacios y caracteres raros
            host = host.Trim();

            // Caso: "POS2 # POS2"
            if (host.Contains("#"))
                host = host.Split('#')[0].Trim();

            // Quitar posibles puertos erróneos
            if (host.Contains(","))
                host = host.Split(',')[0].Trim();

            // Quitar posibles dominios no deseados
            if (host.Contains(" "))
                host = host.Split(' ')[0].Trim();

            return host;
        }



        private void btnConsultarEquipo_Click(object sender, EventArgs e)
        {
            if (cbEquipos.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un equipo.");
                return;
            }

            string equipo = cbEquipos.SelectedItem?.ToString();

            string hostLimpio = LimpiarHostname(equipo);

            // Mostrar para pruebas
            MessageBox.Show("Consultando ventas en el: " + hostLimpio);


            string linkedServerName = $"[{hostLimpio},1433]";



            try
            {
                using (var conn = new SqlConnection(connectionToMotherServer))
                {
                    conn.Open();

                    string checkLS = @"SELECT COUNT(*) FROM sys.servers WHERE name = @ls";
                    using (var cmdCheck = new SqlCommand(checkLS, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@ls", $"{hostLimpio},1433");
                        int exists = (int)cmdCheck.ExecuteScalar();

                        if (exists == 0)
                        {
                            string nombreLinkedServer = "LS_" + hostLimpio;
                            string addLS = $@"
EXEC master.dbo.sp_addlinkedserver 
    @server = '{nombreLinkedServer},1433',
    @srvproduct = '',
    @provider = 'SQLNCLI',
    @datasrc = '{hostLimpio},1433';

EXEC master.dbo.sp_addlinkedsrvlogin 
    @rmtsrvname = '{nombreLinkedServer},1433',
    @useself = 'FALSE',
    @locallogin = NULL,
    @rmtuser = 'sa',
    @rmtpassword = 'cinettorcel';
";

                            using (var cmdAdd = new SqlCommand(addLS, conn))
                                cmdAdd.ExecuteNonQuery();
                        }
                    }

                    string query = $@"
SELECT 
    VENE_FECHA AS Fecha,
    VENE_HORA AS Hora,
    SUC_CODIGO AS Sucursal,
    VENE_NUMERO AS NumComprobante,
    CBTEE_CODIGO AS TipoComprobante,
    VENE_CAE AS CAE,
    CAEA_INFORMADO
FROM {linkedServerName}.cinet_pdv.dbo.VENTAS_E
WHERE USA_CAEA = 'S'
ORDER BY VENE_FECHA DESC, VENE_HORA DESC;
";

                    DataTable dt = new DataTable();
                    using (var cmd = new SqlCommand(query, conn))
                    using (var da = new SqlDataAdapter(cmd))
                        da.Fill(dt);

                    FormResultadosEquipo fr = new FormResultadosEquipo(dt);
                    fr.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error consultando equipo: " + ex.Message);
            }

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLinkedServer_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;


            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }
    }
}
