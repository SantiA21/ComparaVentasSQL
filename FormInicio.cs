using ComparaVentasExcel.Update;
using ComparaVentasExcel.Utils;
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
            try
            {
                if (UpdateChecker.HayActualizacion(out string versionNueva))
                {
                    var r = MessageBox.Show(
                        $"Hay una nueva versión ({versionNueva}). ¿Actualizar ahora?",
                        "Actualización disponible",
                        MessageBoxButtons.YesNo);

                    if (r == DialogResult.Yes)
                    {
                        IniciarUpdater();
                    }
                }
            }
            catch
            {
                // Si no hay internet o GitHub no responde, no pasa nada
            }
        }

        private void IniciarUpdater()
        {
            string updaterPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Updater.exe"
            );

            Process.Start(updaterPath);
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
