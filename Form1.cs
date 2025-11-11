using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace ComparadorExcelSQL
{
    public partial class Form1 : Form
    {
        private DataAccess dataAccess; // Clase para manejar la conexión

        public Form1()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        // Botón para examinar archivo
        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtArchivo.Text = ofd.FileName;
                }
            }
        }

        // Botón para procesar archivo
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArchivo.Text))
            {
                MessageBox.Show("Seleccione un archivo Excel primero.");
                return;
            }

            try
            {
                lblEstado.Text = "Procesando...";
                btnProcesar.Enabled = false;
                dgvResultados.DataSource = null;

                DataTable dtResultados = new DataTable();
                dtResultados.Columns.Add("ID Unico");
                dtResultados.Columns.Add("Local");
                dtResultados.Columns.Add("Sucursal");
                dtResultados.Columns.Add("Número");
                dtResultados.Columns.Add("Comprobante");
                dtResultados.Columns.Add("Existe");

                using (var conexion = dataAccess.GetConnection())
                {
                    conexion.Open();

                    using (var workbook = new XLWorkbook(txtArchivo.Text))
                    {
                        var hoja = workbook.Worksheet(1);

                        // Buscar la columna "ID Unico"
                        var encabezados = hoja.Row(1).Cells().Select(c => c.GetString()).ToList();
                        int colIndex = encabezados.FindIndex(c => c.Trim().Equals("ID Unico", StringComparison.OrdinalIgnoreCase)) + 1;

                        if (colIndex == 0)
                        {
                            MessageBox.Show("No se encontró la columna 'ID Unico' en el archivo.");
                            return;
                        }

                        int fila = 2;
                        int procesadas = 0;

                        while (!hoja.Cell(fila, colIndex).IsEmpty())
                        {
                            string dato = hoja.Cell(fila, colIndex).GetString()?.Trim();

                            if (string.IsNullOrWhiteSpace(dato))
                            {
                                fila++;
                                continue;
                            }

                            string[] partes = dato.Split('-');
                            if (partes.Length != 3)
                            {
                                dtResultados.Rows.Add(dato, "-", "-", "-", "⚠️ Formato inválido");
                                fila++;
                                continue;
                            }

                            string suc_codigo = partes[0];
                            string vene_numero = partes[1].PadLeft(8, '0');
                            string cbtee_codigo = (partes[2] == "1" || partes[2] == "6") ? "FAB" : "DESCONOCIDO";

                            string query = @"
                                SELECT TOP 1 PERI_CODIGO 
                                FROM ventas_e 
                                WHERE suc_codigo = @suc_codigo 
                                  AND vene_numero = @vene_numero 
                                  AND cbtee_codigo = @cbtee_codigo;

                                SELECT TOP 1 PERI_CODIGO 
                                FROM ventas_e 
                                WHERE suc_codigo = @suc_codigo;";

                            using (var cmd = new SqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@suc_codigo", suc_codigo);
                                cmd.Parameters.AddWithValue("@vene_numero", vene_numero);
                                cmd.Parameters.AddWithValue("@cbtee_codigo", cbtee_codigo);

                                string peri_codigo = "-";
                                string resultado = "❌ No existe";

                                using (var reader = cmd.ExecuteReader())
                                {
                                    // Primer resultado: búsqueda exacta
                                    if (reader.Read() && reader[0] != DBNull.Value)
                                    {
                                        peri_codigo = reader[0].ToString();
                                        resultado = "✅ Existe";
                                    }
                                    else
                                    {
                                        // Si no hay coincidencia exacta, pasamos al segundo query
                                        if (reader.NextResult() && reader.Read() && reader[0] != DBNull.Value)
                                        {
                                            peri_codigo = reader[0].ToString();
                                            resultado = "❌ No existe";
                                        }
                                    }
                                }

                                // “Local” = peri_codigo
                                dtResultados.Rows.Add(dato, peri_codigo, suc_codigo, vene_numero, cbtee_codigo, resultado);
                            }

                            procesadas++;
                            fila++;
                        }

                        dgvResultados.DataSource = dtResultados;
                        lblEstado.Text = $"✅ Proceso finalizado. {procesadas} filas procesadas.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante el proceso: " + ex.Message);
            }
            finally
            {
                btnProcesar.Enabled = true;
            }
        }

        // Botón para exportar a Excel
        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.DataSource == null)
            {
                MessageBox.Show("No hay datos para exportar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
                    sfd.FileName = "Resultado_Comparacion.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var wb = new XLWorkbook())
                        {
                            DataTable dt = ((DataTable)dgvResultados.DataSource).Copy();
                            wb.Worksheets.Add(dt, "Resultados");
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("✅ Archivo exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message);
            }
        }
    }
}
