using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;
using CinetCore.Utils;

namespace CinetCore
{
    public partial class FormResultadosEquipo : Form
    {

        private string _connectionString;
        private string _query;

        public FormResultadosEquipo(DataTable datos)
        {
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
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
                CinetCore.Utils.Alert.Show(
                    UserMessageHelper.GetFriendlyMessage("al consultar las ventas con CAEA del equipo seleccionado", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormResultadosEquipo_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;


            lblVersion.Visible = false;
        }

        private void btoActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }
    }
}
