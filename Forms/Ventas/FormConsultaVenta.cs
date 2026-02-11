using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinetCore
{
    public partial class FormConsultaVenta : Form
    {
        private DataAccess dataAccess;
        private string selectedDbKey;
        public FormConsultaVenta()
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

        private void FormConsultaVenta_Load(object sender, EventArgs e)
        {
            
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }



        private void btnConsultar_Click(object sender, EventArgs e)
        {

            // en estos if valido que los campos no esten vacios y solo se puedan ingresar numeros
            if (string.IsNullOrWhiteSpace(txtComprobante.Text) ||
                string.IsNullOrWhiteSpace(txtSucursal.Text) ||
                cbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe completar los datos");
                return;
            }
            if (!int.TryParse(txtSucursal.Text, out int sucCodigo) ||
                !int.TryParse(txtComprobante.Text, out int veneNumero))
            {
                MessageBox.Show("Sucursal y Comprobante deben ser numéricos.");
                return;
            }

            // Normalizar valores a la longitud correcta
            txtSucursal.Text = txtSucursal.Text.PadLeft(4, '0');
            txtComprobante.Text = txtComprobante.Text.PadLeft(8, '0');

            string sucursal = txtSucursal.Text;
            string numero = txtComprobante.Text;

            string tipoComprobante = cbTipo.SelectedItem.ToString().Trim();
            if (tipoComprobante.Length > 10 || tipoComprobante.Any(c => !char.IsLetterOrDigit(c)))
            {
                MessageBox.Show("Tipo de comprobante inválido.");
                return;
            }

            try
            {
                btnConsultar.Enabled = false;
                dgvVenta.DataSource = null;

                
                DataTable dtVenta = new DataTable();

                using (var conexion = dataAccess.GetConnection(selectedDbKey))
                {
                    conexion.Open();

                    int timeout = 120; 

                    using (var cmd = new SqlCommand(@"
                SELECT PERI_CODIGO As Local, VENE_FECHA As Fecha, VENE_HORA As Hora, E.SUC_CODIGO As Sucursal, E.VENE_NUMERO As NumComprobante, E.CBTEE_CODIGO As TipoComprobante, E.VENE_CAE As CAE, E.VENE_CAJA As Caja, T.VENT_IMPORTE As ImporteTotal FROM VENTAS_E E INNER JOIN VENTAS_T T ON T.SUC_CODIGO = E.SUC_CODIGO AND T.CBTEE_CODIGO = E.CBTEE_CODIGO AND T.VENE_NUMERO = E.VENE_NUMERO AND T.VENT_CONCEPTO = 'TOTAL' 
                WHERE E.VENE_NUMERO = @vene_numero AND E.SUC_CODIGO = @suc_codigo AND E.CBTEE_CODIGO = @cbtee_codigo", conexion))
                    {
                        Logger.LogQuery(cmd.CommandText);
                        cmd.CommandTimeout = timeout;
                        cmd.Parameters.AddWithValue("@suc_codigo", txtSucursal.Text.Trim());
                        cmd.Parameters.AddWithValue("@vene_numero", txtComprobante.Text.Trim());
                        cmd.Parameters.AddWithValue("@cbtee_codigo", cbTipo.SelectedItem.ToString().Trim());

                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtVenta);
                        }
                    }
                }

                
                dgvVenta.DataSource = dtVenta;

                if (dtVenta.Rows.Count > 0)
                    MessageBox.Show("Venta encontrada en la base de datos.");
                else
                    MessageBox.Show("No existe la venta en la base de datos.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show(
                    UserMessageHelper.GetFriendlyMessage("al consultar la venta en la base de datos", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnConsultar.Enabled = true;
            }
        }

    }
}
