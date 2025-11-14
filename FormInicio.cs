using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

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
            Form1 mainForm = new Form1();
            mainForm.Show();
        }

        private void consultarVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormConsultaVenta mainForm = new FormConsultaVenta();
            mainForm.Show();
        }

        private void verSucursalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormVerSucursales mainForm = new FormVerSucursales();
            mainForm.Show();
        }

        private void FormInicio_Load(object sender, EventArgs e)
        {
            // Obtiene la versión del ensamblado actual
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Opcional: convertirlo a texto amigable
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }


    }
}
