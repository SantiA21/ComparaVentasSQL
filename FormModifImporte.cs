using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace ComparaVentasExcel
{
    public partial class FormModifImporte : Form
    {
        private DataAccess dataAccess;
        private string selectedDbKey;
        public FormModifImporte()
        {
            InitializeComponent();
            dataAccess = new DataAccess();

            var keys = dataAccess.GetKeys();
            cbBaseDatos.Items.AddRange(keys);

            if (cbBaseDatos.Items.Count > 0)
            {
                cbBaseDatos.SelectedIndex = 0;
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            }

            cbBaseDatos.SelectedIndexChanged += (s, e) =>
            {
                if (cbBaseDatos.SelectedItem != null)
                    selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            };
        }

        private void CbBaseDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBaseDatos.SelectedItem != null)
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormModifImporte_Load(object sender, EventArgs e)
        {

            Version version = Assembly.GetExecutingAssembly().GetName().Version;


            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            var confirmar = MessageBox.Show(
                "¿Está seguro que desea modificar los importes del comprobante?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmar != DialogResult.Yes)
                return;

            using (var frmClave = new FrmClave())
            {
                if (frmClave.ShowDialog() != DialogResult.OK)
                    return;

                if (frmClave.ClaveIngresada != "Cinet2026@")
                {
                    MessageBox.Show(
                        "❌ Clave incorrecta. Operación cancelada.",
                        "Acceso denegado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtComprobante.Text) ||
            string.IsNullOrWhiteSpace(txtSucursal.Text) ||
            string.IsNullOrWhiteSpace(txtImporte.Text) ||
            cbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe completar todos los datos.");
                return;
            }

            if (!int.TryParse(txtSucursal.Text, out _) ||
                !int.TryParse(txtComprobante.Text, out _))
            {
                MessageBox.Show("Sucursal y Comprobante deben ser numéricos.");
                return;
            }

            txtSucursal.Text = txtSucursal.Text.PadLeft(4, '0');
            txtComprobante.Text = txtComprobante.Text.PadLeft(8, '0');

            string sucursal = txtSucursal.Text.Trim();
            string comprobante = txtComprobante.Text.Trim();
            string tipoComprobante = cbTipo.SelectedItem.ToString().Trim();

            if (!decimal.TryParse(
                txtImporte.Text.Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out decimal total))
            {
                MessageBox.Show("Importe inválido.");
                return;
            }

            decimal iva1 = Math.Round(total * 0.21m, 2);
            decimal neto1 = Math.Round(total - iva1, 2);

            string sql = @"
UPDATE VENTAS_T
SET vent_importe = @TOTAL
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO IN ('SUBTOTAL','TOTAL')
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VENTAS_T
SET vent_importe = @IVA1
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO = 'IVA1'
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VENTAS_T
SET vent_importe = @NETO1
WHERE SUC_CODIGO = @SUCURSAL
  AND VENE_NUMERO = @COMPROBANTE
  AND VENT_CONCEPTO = 'NETO1'
  AND CBTEE_CODIGO = @TIPOCBTE;

UPDATE VAL_MOVIMIENTOS
SET VALM_IMPORTE = @TOTAL
WHERE CBTEINSUC_CODIGO = @SUCURSAL
  AND INGE_NUMERO = @COMPROBANTE;
";

            try
            {
                btnModificar.Enabled = false;

                using (var conexion = dataAccess.GetConnection(selectedDbKey))
                {
                    conexion.Open();

                    using (var tran = conexion.BeginTransaction())
                    using (var cmd = new SqlCommand(sql, conexion, tran))

                    {
                        Logger.LogQuery(cmd.CommandText);
                        cmd.CommandTimeout = 120;

                        cmd.Parameters.Add("@SUCURSAL", SqlDbType.VarChar, 10).Value = sucursal;
                        cmd.Parameters.Add("@COMPROBANTE", SqlDbType.VarChar, 10).Value = comprobante;
                        cmd.Parameters.Add("@TIPOCBTE", SqlDbType.VarChar, 3).Value = tipoComprobante;
                        cmd.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = total;
                        cmd.Parameters.Add("@IVA1", SqlDbType.Decimal).Value = iva1;
                        cmd.Parameters.Add("@NETO1", SqlDbType.Decimal).Value = neto1;

                        cmd.ExecuteNonQuery();
                        tran.Commit();

                        MessageBox.Show(
                            "✅ Modificación realizada correctamente.",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("❌ Error al modificar: " + ex.Message);
            }
            finally
            {
                btnModificar.Enabled = true;
            }
        }
    }
}