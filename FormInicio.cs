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

        string GetLocalVersion()
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "version.txt"
            );

            return File.ReadAllText(path).Trim();
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

        private async void FormInicio_Load(object sender, EventArgs e)
        {
            // Mostrar versión (opcional)
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {v}";

            // Chequear actualización
            if (UpdateChecker.HayActualizacion(out Version versionNueva))
            {
                var r = MessageBox.Show(
                    $"Hay una nueva versión ({versionNueva}).\n¿Desea actualizar ahora?",
                    "Actualización disponible",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );

                if (r == DialogResult.Yes)
                {
                    LanzarUpdater();
                }
            }
        }

        private void LanzarUpdater()
        {
            string updaterPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ComparadorVentas.Updater.exe"
            );

            Process.Start(updaterPath, Process.GetCurrentProcess().Id.ToString());
            Application.Exit();
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
