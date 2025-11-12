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

namespace ComparaVentasExcel
{
    public partial class FormConsultaVenta : Form
    {
        private DataAccess dataAccess; // Clase para manejar la conexión
        public FormConsultaVenta()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormInicio mainForm = new FormInicio();
            mainForm.Show();
        }

        private void FormConsultaVenta_Load(object sender, EventArgs e)
        {
            // Obtiene la versión del ensamblado actual
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Opcional: convertirlo a texto amigable
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }



        private void btnConsultar_Click(object sender, EventArgs e)
        {
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

                // Crear DataTables
                DataTable dtVenta = new DataTable();

                using (var conexion = dataAccess.GetConnection())
                {
                    conexion.Open();

                    int timeout = 120; 

                    using (var cmd = new SqlCommand(@"
                SELECT PERI_CODIGO As Local, VENE_FECHA As Fecha, VENE_HORA As Hora, E.SUC_CODIGO As Sucursal, E.VENE_NUMERO As NumComprobante, E.CBTEE_CODIGO As TipoComprobante, E.VENE_CAE As CAE, E.VENE_CAJA As Caja, T.VENT_IMPORTE As ImporteTotal FROM VENTAS_E E INNER JOIN VENTAS_T T ON T.SUC_CODIGO = E.SUC_CODIGO AND T.CBTEE_CODIGO = E.CBTEE_CODIGO AND T.VENE_NUMERO = E.VENE_NUMERO AND T.VENT_CONCEPTO = 'TOTAL' 
                WHERE E.VENE_NUMERO = @vene_numero AND E.SUC_CODIGO = @suc_codigo AND E.CBTEE_CODIGO = @cbtee_codigo", conexion))
                    {
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

                // Mostrar los resultados
                dgvVenta.DataSource = dtVenta;

                if (dtVenta.Rows.Count > 0)
                    MessageBox.Show("✅ Venta encontrada en la base de datos.");
                else
                    MessageBox.Show("❌ No existe la venta en la base de datos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante la consulta: " + ex.Message);
            }
            finally
            {
                btnConsultar.Enabled = true;
            }
        }

    }
}
