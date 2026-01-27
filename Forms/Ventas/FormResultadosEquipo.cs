using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ComparaVentasExcel
{
    public partial class FormResultadosEquipo : Form
    {

        private string _connectionString;
        private string _query;

        public FormResultadosEquipo(DataTable datos)
        {
            InitializeComponent();
            dgvDatos.DataSource = datos;
        }

        public FormResultadosEquipo(string connectionString, string query)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _query = query;

            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var da = new SqlDataAdapter(_query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDatos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar datos: " + ex.Message);
            }
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormResultadosEquipo_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;


            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btoActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
