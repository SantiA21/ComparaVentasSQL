using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormVerSucursales : Form
    {
        private DataAccess dataAccess; // Clase para manejar la conexión
        private DataTable dtSucursales;
        public FormVerSucursales()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormInicio mainForm = new FormInicio();
            mainForm.Show();
        }

        private void FormVerSucursales_Load(object sender, EventArgs e)
        {
            CargarSucursales();
            txtBuscar.TextChanged += TxtBuscar_TextChanged;

            // Obtiene la versión del ensamblado actual
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Opcional: convertirlo a texto amigable
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void CargarSucursales()
        {
            try
            {
                dtSucursales = new DataTable();

                using (SqlConnection conexion = dataAccess.GetConnection())
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT DISTINCT 
                        ve.vene_caja As Caja,
                        su.SUC_CODIGO As Sucursal, 
                        ve.PERI_CODIGO As Local 
                    FROM SUCURSALES su
                    INNER JOIN VENTAS_E ve ON ve.SUC_CODIGO = su.SUC_CODIGO
                    WHERE su.SUC_CODIGO IN (
                        SELECT SUC_CODIGO 
                        FROM SUCURSALES 
                        WHERE SUC_CODIGO >= '1500' 
                          AND SUC_LOCAL = 'FE'
                    )
                    AND ve.VENE_FECHA > GETDATE() - 30
                    ORDER BY 1", conexion))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    conexion.Open();
                    adapter.Fill(dtSucursales);
                }

                dgvSucursales.DataSource = dtSucursales;

                // Ajustes visuales opcionales
                dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSucursales.AllowUserToAddRows = false;
                dgvSucursales.ReadOnly = true;
                dgvSucursales.RowHeadersVisible = false;

                // Mensaje opcional
                if (dtSucursales.Rows.Count > 0)
                    lblEstado.Text = $"✅ {dtSucursales.Rows.Count} sucursales encontradas.";
                else
                    lblEstado.Text = "❌ No se encontraron sucursales con ventas en los últimos 30 días.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las sucursales: " + ex.Message);
            }
        }
        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtSucursales == null || dtSucursales.Rows.Count == 0)
                    return;

                string filtro = txtBuscar.Text.Trim().Replace("'", "''"); // evita errores o inyecciones
                DataView vista = dtSucursales.DefaultView;

                if (string.IsNullOrEmpty(filtro))
                {
                    vista.RowFilter = ""; // mostrar todo si el filtro está vacío
                }
                else
                {
                    vista.RowFilter = $"Convert(Sucursal, 'System.String') LIKE '%{filtro}%' OR Convert(Local, 'System.String') LIKE '%{filtro}%'";
                }

                lblEstado.Text = $"🔍 Mostrando {vista.Count} resultados.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar los datos: " + ex.Message);
            }
        }
    }
}
