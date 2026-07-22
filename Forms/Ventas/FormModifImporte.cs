using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Models;
using CinetCore.Utils;
using CinetCore.Services.Importes;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace CinetCore
{
    public partial class FormModifImporte : Form
    {
        private readonly DataAccess dataAccess;
        private string selectedDbKey;

        public FormModifImporte()
        {
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
            dataAccess = CinetCore.Infrastructure.AppDI.GetService<CinetCore.Data.DataAccess>();
            InicializarBases();
        }

        private void InicializarBases()
        {
            cbBaseDatos.Items.AddRange(dataAccess.GetKeys());
            cbBaseDatos.SelectedIndex = 0;
            selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            cbBaseDatos.SelectedIndexChanged += (_, __) =>
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
        }

        private void FormModifImporte_Load(object sender, EventArgs e)
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Visible = false;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!ValidarEntrada())
                return;

            if (!SeguridadHelper.ValidarClave())
            {
                Logger.LogInfo("Clave incorrecta al intentar modificar importe");

                CinetCore.Utils.Alert.Show(
                    "La contraseña ingresada es incorrecta.",
                    "Acceso denegado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }


            try
            {
                btnModificar.Enabled = false;

                var request = ConstruirRequest();
                var service = new ModificacionImporteService(dataAccess);

                service.ModificarImporte(selectedDbKey, request);

                CinetCore.Utils.Alert.Show("Modificación realizada correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                CinetCore.Utils.Alert.Show(
                    UserMessageHelper.GetFriendlyMessage("al modificar el importe del comprobante", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnModificar.Enabled = true;
            }
        }

        private bool ValidarEntrada()
        {
            if (string.IsNullOrWhiteSpace(txtSucursal.Text) ||
                string.IsNullOrWhiteSpace(txtComprobante.Text) ||
                string.IsNullOrWhiteSpace(txtImporte.Text) ||
                cbTipo.SelectedIndex == -1)
            {
                CinetCore.Utils.Alert.Show("Debe completar todos los datos.");
                return false;
            }

            if (!int.TryParse(txtSucursal.Text, out _) ||
                !int.TryParse(txtComprobante.Text, out _))
            {
                CinetCore.Utils.Alert.Show("Sucursal y Comprobante deben ser numéricos.");
                return false;
            }

            if (!decimal.TryParse(
                txtImporte.Text.Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _))
            {
                CinetCore.Utils.Alert.Show("Importe inválido.");
                return false;
            }

            return true;
        }

        private ModificacionImporteRequest ConstruirRequest()
        {
            string suc = txtSucursal.Text.PadLeft(4, '0');
            string comp = txtComprobante.Text.PadLeft(8, '0');

            decimal total = decimal.Parse(
                txtImporte.Text.Replace(",", "."),
                CultureInfo.InvariantCulture
            );

            decimal iva = Math.Round(total * 0.21m, 2);
            decimal neto = Math.Round(total - iva, 2);

            return new ModificacionImporteRequest
            {
                Sucursal = suc,
                Comprobante = comp,
                TipoComprobante = cbTipo.SelectedItem.ToString(),
                Total = total,
                Iva = iva,
                Neto = neto
            };
        }

        private void btnVolver_Click(object sender, EventArgs e) => Close();
    }
}
