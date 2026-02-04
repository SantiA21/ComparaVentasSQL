using ComparaVentasExcel.Forms.Sucursales;
using ComparaVentasExcel.Forms.Usuarios;
using ComparaVentasExcel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormInicio : Form
    {
        public FormInicio()
        {
            InitializeComponent();
        }


        private void LanzarUpdater()
        {
            string updaterPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Updater.exe"
            );

            Process.Start(updaterPath, Process.GetCurrentProcess().Id.ToString());
            Application.Exit();
        }

        private void MostrarVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {v.Major}.{v.Minor}.{v.Build}";
        }

        private bool VengoDeActualizar()
        {
            string flagPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "updated.flag"
            );

            if (File.Exists(flagPath))
            {
                File.Delete(flagPath);
                return true;
            }

            return false;
        }

        private void MostrarChangelog()
        {
            using (var frm = new FormChangelog())
            {
                frm.ShowDialog();
            }
        }


        private async void FormInicio_Load(object sender, EventArgs e)
        {
            MostrarVersion();

            // 1️⃣ Si vengo de una actualización → mostrar changelog
            if (VengoDeActualizar())
            {
                MostrarChangelog();
                return;
            }

            // 2️⃣ Si NO vengo del updater → chequear si hay update
            if (!Program.ArrancoDesdeUpdater)
            {
                if (UpdateChecker.HayActualizacion(out Version versionNueva))
                {

                    MessageBox.Show(
                "Hay una nueva versión disponible.\nLa aplicación se actualizará automáticamente.",
                "Actualización",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
                    LanzarUpdater();
                    Application.Exit();
                }
            }
        }

        private void importarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormComparaExcel mainForm = new FormComparaExcel())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void consultarVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormConsultaVenta mainForm = new FormConsultaVenta())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void ventasConCAEAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormLinkedServer mainForm = new FormLinkedServer())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void modificarImporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormModifImporte mainForm = new FormModifImporte())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void verSucursalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormVerSucursales mainForm = new FormVerSucursales())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void novToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FormChangelog())
            {
                frm.ShowDialog();
            }
        }

        private void usuariosMOSTAZAERPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormUsuariosMostazaERP mainForm = new FormUsuariosMostazaERP())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void usuariosGMGERPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormUsuariosGmgERP mainForm = new FormUsuariosGmgERP())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void usuariosBackofficeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormConexionRemota mainForm = new FormConexionRemota())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }

        private void insertarSucursalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormInsertarSucursalFE mainForm = new FormInsertarSucursalFE())
            {
                mainForm.ShowDialog();
            }
            this.Show();
        }
    }
}
