using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormVerSucursales : Form
    {
        private DataAccess dataAccess;
        private DataTable dtSucursales;
        private string selectedDbKey;

        public FormVerSucursales()
        {
            InitializeComponent();
            dataAccess = new DataAccess();

            // Cargar bases de datos en el ComboBox
            var keys = dataAccess.GetKeys();
            cbBaseDatos.Items.AddRange(keys);

            if (cbBaseDatos.Items.Count > 0)
            {
                cbBaseDatos.SelectedIndex = 0; // Predeterminado: MOSTAZA_ERP
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            }

            // Cambiar base → recargar sucursales
            cbBaseDatos.SelectedIndexChanged += (s, e) =>
            {
                if (cbBaseDatos.SelectedItem != null)
                {
                    selectedDbKey = cbBaseDatos.SelectedItem.ToString();
                    CargarSucursales();   // Recargar desde la base seleccionada
                    AplicarFiltro();      // Aplicar filtro si ya hay texto en txtBuscar
                }
            };

            // Buscar en tiempo real
            txtBuscar.TextChanged += TxtBuscar_TextChanged;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormVerSucursales_Load(object sender, EventArgs e)
        {
            // Cargar datos al iniciar
            CargarSucursales();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        // Con este metodo selecciono que query hacer dependiendo la base
        private string GetQueryByDatabase(string dbKey)
        {
            if (dbKey.Equals("MOSTAZA_ERP", StringComparison.OrdinalIgnoreCase))
            {
                return @"
            SELECT *
FROM (
    SELECT DISTINCT 
        ve.vene_caja AS Caja,
        su.SUC_CODIGO AS Sucursal, 
        ve.PERI_CODIGO AS Local 
    FROM SUCURSALES su
    INNER JOIN VENTAS_E ve ON ve.SUC_CODIGO = su.SUC_CODIGO
    WHERE su.SUC_CODIGO IN (
        SELECT SUC_CODIGO 
        FROM SUCURSALES 
        WHERE SUC_CODIGO >= '1500' 
          AND SUC_LOCAL = 'FE'
    )
    AND ve.VENE_FECHA > GETDATE() - 30
) AS X
ORDER BY X.Local, TRY_CAST(X.Caja AS INT);";
            }
            else if (dbKey.Equals("GMG_ERP", StringComparison.OrdinalIgnoreCase))
            {
                return @"
            SELECT *
FROM (
    SELECT DISTINCT 
        ve.vene_caja AS Caja,
        su.SUC_CODIGO AS Sucursal, 
        ve.PERI_CODIGO AS Local 
    FROM SUCURSALES su
    INNER JOIN VENTAS_E ve ON ve.SUC_CODIGO = su.SUC_CODIGO
    WHERE su.SUC_CODIGO IN (
        SELECT SUC_CODIGO 
        FROM SUCURSALES 
        WHERE SUC_CODIGO >= '0300' 
          AND SUC_LOCAL = 'FE'
    )
    AND ve.VENE_FECHA > GETDATE() - 30
) AS X
ORDER BY X.Local, TRY_CAST(X.Caja AS INT);";
            }

            return ""; // fallback
        }

        private void CargarSucursales()
        {
            try
            {
                dtSucursales = new DataTable();

                string query = GetQueryByDatabase(selectedDbKey);

                using (SqlConnection conexion = dataAccess.GetConnection(selectedDbKey))
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    Logger.LogQuery(cmd.CommandText);
                    conexion.Open();
                    adapter.Fill(dtSucursales);
                }

                dgvSucursales.DataSource = dtSucursales;

                dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSucursales.AllowUserToAddRows = false;
                dgvSucursales.ReadOnly = true;
                dgvSucursales.RowHeadersVisible = false;

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("Error al cargar las sucursales: " + ex.Message);
            }
        }


        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            if (dtSucursales == null) return;

            string filtro = txtBuscar.Text.Trim().Replace("'", "''");

            DataView vista = dtSucursales.DefaultView;

            if (string.IsNullOrEmpty(filtro))
            {
                vista.RowFilter = "";
            }
            else
            {
                vista.RowFilter =
                    $"Convert(Sucursal, 'System.String') LIKE '%{filtro}%' " +
                    $"OR Convert(Local, 'System.String') LIKE '%{filtro}%'";
            }
        }

    }
}
