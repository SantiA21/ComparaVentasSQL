using CinetCore.Infrastructure;
using CinetCore.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CinetCore
{
    public partial class FormLinkedServer : Form
    {
        private string connectionToMotherServer = "";

        public FormLinkedServer()
        {
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
        }

        private void btnCargarEquipos_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServidorMadre.Text))
            {
                CinetCore.Utils.Alert.Show("Debe ingresar la IP / Hostname del servidor.");
                return;
            }

            // NOTA DE SEGURIDAD: Las credenciales están hardcodeadas por simplicidad.
            // En producción, deberían estar en un archivo de configuración encriptado.
            // El servidor madre debe ser validado antes de usar.
            string servidor = txtServidorMadre.Text.Trim();
            
            // Validación básica del servidor
            if (servidor.Contains(";") || servidor.Contains("'") || servidor.Contains("\""))
            {
                CinetCore.Utils.Alert.Show("El nombre del servidor contiene caracteres no permitidos.");
                return;
            }

            connectionToMotherServer =
                $"Server={servidor};Database=BACKOFFICE;User Id=sa;Password=cinettorcel;";

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
                        Logger.LogQuery(cmd.CommandText);
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

                CinetCore.Utils.Alert.Show("Equipos cargados correctamente.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                CinetCore.Utils.Alert.Show(
                    UserMessageHelper.GetFriendlyMessage("al cargar los equipos desde el servidor BACKOFFICE", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string LimpiarHostname(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return null;

            host = host.Trim();

            // Remover caracteres peligrosos para prevenir SQL injection
            if (host.Contains("#"))
                host = host.Split('#')[0].Trim();

            if (host.Contains(","))
                host = host.Split(',')[0].Trim();

            if (host.Contains(" "))
                host = host.Split(' ')[0].Trim();

            // Validar que solo contenga caracteres alfanuméricos, puntos, guiones y guiones bajos
            // Esto previene inyección SQL adicional
            foreach (char c in host)
            {
                if (!char.IsLetterOrDigit(c) && c != '.' && c != '-' && c != '_')
                {
                    throw new ArgumentException("El hostname contiene caracteres no permitidos.");
                }
            }

            return host;
        }

        private string ResolverBasePDV(SqlConnection conn, string linkedServerName)
        {
            string query = $@"
SELECT TOP 1 name
FROM {linkedServerName}.master.sys.databases
WHERE name IN ('cinet_pdv', 'cinet_pdv_totem', 'cinet_pdv_auto')
ORDER BY 
    CASE name
        WHEN 'cinet_pdv' THEN 1
        WHEN 'cinet_pdv_totem' THEN 2
        WHEN 'cinet_pdv_auto' THEN 3
    END
";

            using (var cmd = new SqlCommand(query, conn))
            {
                Logger.LogQuery(cmd.CommandText);
                object result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }

        private void btnConsultarEquipo_Click(object sender, EventArgs e)
        {
            string equipoIngresado = cbEquipos.Text;

            if (string.IsNullOrWhiteSpace(equipoIngresado))
            {
                CinetCore.Utils.Alert.Show("Debe ingresar o seleccionar un equipo.");
                return;
            }

            string hostLimpio;
            try
            {
                hostLimpio = LimpiarHostname(equipoIngresado);
                if (string.IsNullOrWhiteSpace(hostLimpio))
                {
                    CinetCore.Utils.Alert.Show("No se pudo procesar el nombre del equipo.");
                    return;
                }
            }
            catch (ArgumentException ex)
            {
                CinetCore.Utils.Alert.Show($"Error en el nombre del equipo: {ex.Message}");
                return;
            }

            string linkedServerName = $"[{hostLimpio},1433]";
            string linkedServerRawName = $"{hostLimpio},1433";

            try
            {
                using (var conn = new SqlConnection(connectionToMotherServer))
                {
                    conn.Open();

                    // Verificar si existe el linked server
                    string checkLS = @"SELECT COUNT(*) FROM sys.servers WHERE name = @ls";
                    using (var cmdCheck = new SqlCommand(checkLS, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@ls", linkedServerRawName);
                        int exists = (int)cmdCheck.ExecuteScalar();

                        if (exists == 0)
                        {
                            string addLS = $@"
EXEC master.dbo.sp_addlinkedserver 
    @server = '{linkedServerRawName}',
    @srvproduct = '',
    @provider = 'SQLNCLI',
    @datasrc = '{linkedServerRawName}';

EXEC master.dbo.sp_addlinkedsrvlogin 
    @rmtsrvname = '{linkedServerRawName}',
    @useself = 'FALSE',
    @locallogin = NULL,
    @rmtuser = 'sa',
    @rmtpassword = 'cinettorcel';
";
                            using (var cmdAdd = new SqlCommand(addLS, conn))
                                cmdAdd.ExecuteNonQuery();
                        }
                    }

                    // 🔥 Resolver base PDV dinámica
                    string basePDV = ResolverBasePDV(conn, linkedServerName);

                    if (string.IsNullOrEmpty(basePDV))
                    {
                        CinetCore.Utils.Alert.Show("No se encontró una base PDV válida en el equipo.");
                        return;
                    }

                    string query = $@"
SELECT 
    CAEA_INFORMADO,
    VENE_CAE AS CAE,
    VENE_LEYENDACAE AS LeyendaCAE,
    VENE_FECHA AS Fecha,
    VENE_HORA AS Hora,
    SUC_CODIGO AS Sucursal,
    VENE_NUMERO AS NumComprobante,
    CBTEE_CODIGO AS TipoComprobante
FROM {linkedServerName}.{basePDV}.dbo.VENTAS_E
WHERE USA_CAEA = 'S'
ORDER BY VENE_FECHA DESC, VENE_HORA DESC;
";

                    FormResultadosEquipo fr =
                        new FormResultadosEquipo(connectionToMotherServer, query);

                    fr.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                CinetCore.Utils.Alert.Show(
                    UserMessageHelper.GetFriendlyMessage("al consultar el equipo seleccionado", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormLinkedServer_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Visible = false;
        }

        private FormVerSucursalesV2 _formSucursales;

        private void btnSucursales_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(connectionToMotherServer))
            {
                CinetCore.Utils.Alert.Show("Debe cargar primero el servidor madre.");
                return;
            }

            if (_formSucursales == null || _formSucursales.IsDisposed)
            {
                txtServidorMadre.Enabled = false;

                _formSucursales = new FormVerSucursalesV2(connectionToMotherServer);

                _formSucursales.FormClosed += (s, args) =>
                {
                    txtServidorMadre.Enabled = true;
                };

                _formSucursales.Show();
            }
            else
            {
                _formSucursales.BringToFront();
            }
        }
    }
}
