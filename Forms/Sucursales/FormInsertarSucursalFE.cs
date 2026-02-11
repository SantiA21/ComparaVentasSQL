using CinetCore.Infrastructure;
using CinetCore.Services.Sucursales;
using CinetCore.Utils;
using CinetCore.Data;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace CinetCore.Forms.Sucursales
{
    public partial class FormInsertarSucursalFE : Form
    {
        private readonly SucursalService _sucursalService;
        private string selectedDbKey;

        public FormInsertarSucursalFE()
        {
            InitializeComponent();

            var dataAccess = new DataAccess();
            _sucursalService = new SucursalService(dataAccess);

            cbBaseDatos.SelectedIndexChanged += CbBaseDatos_SelectedIndexChanged;

            var keys = dataAccess.GetKeys();
            cbBaseDatos.Items.AddRange(keys);

            if (cbBaseDatos.Items.Count > 0)
            {
                cbBaseDatos.SelectedIndex = 0;
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            }

        }

        private void CbBaseDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBaseDatos.SelectedItem != null)
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
        }

        private void FormInsertarSucursalFE_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void txtSucursal_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
                return;
            }

            // Limitar a 4 caracteres
            TextBox txt = sender as TextBox;
            if (char.IsDigit(e.KeyChar) && txt.Text.Length >= 4)
            {
                e.Handled = true;
            }
        }

        private void txtSucursal_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSucursal.Text))
                return;

            // Seguridad extra
            if (!int.TryParse(txtSucursal.Text, out int valor) || valor < 0)
            {
                MessageBox.Show("Sucursal inválida.");
                txtSucursal.Focus();
                return;
            }

            txtSucursal.Text = valor.ToString("D4"); // completa con ceros
        }

        private bool ValidarSucursal()
        {
            if (!int.TryParse(txtSucursal.Text, out int suc))
            {
                MessageBox.Show("La sucursal debe ser numérica.");
                return false;
            }

            if (suc < 0)
            {
                MessageBox.Show("La sucursal no puede ser negativa.");
                return false;
            }

            if (txtSucursal.Text.Length > 4)
            {
                MessageBox.Show("La sucursal no puede tener más de 4 dígitos.");
                return false;
            }

            txtSucursal.Text = suc.ToString("D4");
            return true;
        }


        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (cbBaseDatos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una base de datos.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSucursal.Text))
            {
                MessageBox.Show("Ingrese el PDV.");
                return;
            }

            if (!ValidarSucursal())
                return;

            if (!SeguridadHelper.ValidarClave())
            {
                Logger.LogInfo("Clave incorrecta al intentar insertar la sucursal");

                MessageBox.Show(
                    "La contraseña ingresada es incorrecta.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            try
            {
                string dbKey = cbBaseDatos.SelectedItem.ToString();
                string pdv = txtSucursal.Text.Trim();

                _sucursalService.InsertarSucursal(dbKey, pdv);

                MessageBox.Show(
                    "Sucursal insertada correctamente.",
                    "OK",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtSucursal.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al insertar la sucursal:\n" + ex.Message,
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
