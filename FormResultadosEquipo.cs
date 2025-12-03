using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormResultadosEquipo : Form
    {
        public FormResultadosEquipo(DataTable datos)
        {
            InitializeComponent();
            dgvDatos.DataSource = datos;
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
    }
}
