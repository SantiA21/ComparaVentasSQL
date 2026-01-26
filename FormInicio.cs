using ComparaVentasExcel.Update;
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
        private void importarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (Form1 mainForm = new Form1())
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

        private void verSucursalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (FormVerSucursales mainForm = new FormVerSucursales())
            {
                mainForm.ShowDialog();
            }
            this.Show();

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

        private async void FormInicio_Load(object sender, EventArgs e)
        {
            // Mostrar versión (opcional)
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {v.Major}.{v.Minor}.{v.Build}";

            // Chequear actualización
            if (!Program.ArrancoDesdeUpdater)
            {
                if (UpdateChecker.HayActualizacion(out Version versionNueva))
                {
                    MessageBox.Show(
                        $"Se encontró una nueva versión ({versionNueva}).\nLa aplicación se actualizará automáticamente.",
                        "Actualización",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LanzarUpdater();
                    Application.Exit();
                }
            }
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

    }
}
