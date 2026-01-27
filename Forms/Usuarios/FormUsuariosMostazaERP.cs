using ComparaVentasExcel.Data;
using ComparaVentasExcel.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormUsuariosMostazaERP : Form
    {
        private DataAccess dataAccess;
        public FormUsuariosMostazaERP()
        {
            InitializeComponent();
        }

        private void FormUsuariosMostazaERP_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";

            CargarUsuarios();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                dgvUsuarios.DataSource =
                    UsuarioMostazaERPService.ObtenerUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar usuarios de Mostaza ERP:\n" + ex.Message,
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


    }
}
