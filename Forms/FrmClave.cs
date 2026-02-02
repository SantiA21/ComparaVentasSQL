using System;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FrmClave : Form
    {
        public string ClaveIngresada { get; private set; }

        public FrmClave()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ClaveIngresada = txtClave.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}