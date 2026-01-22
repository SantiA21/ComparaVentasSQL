using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Reflection;
using System.Collections.Generic;


namespace ComparaVentasExcel
{
    public partial class Form1 : Form
    {
        private DataAccess dataAccess;
        private DataTable dtOriginal;
        private string selectedDbKey;

        public Form1()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
            dgvResultados.RowHeadersVisible = false;

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
                dtResultados.Columns.Add("ImporteARCA");
                dtResultados.Columns.Add("Fecha");
                dtResultados.Columns.Add("CAE");
                dtResultados.Columns.Add("Existe");

                using (var conexion = dataAccess.GetConnection(selectedDbKey))
                {
                    conexion.Open();

                    using (var workbook = new XLWorkbook(txtArchivo.Text))
                    {
                        var hoja = workbook.Worksheet(1);

                        // ============================
                        // MAPEO DE COLUMNAS POR NOMBRE
                        // ============================
                        var colIndexMap = hoja.Row(1)
                            .CellsUsed()
                            .ToDictionary(
                                c => c.GetString().Trim(),
                                c => c.Address.ColumnNumber
                            );

                        string[] columnasNecesarias =
                        {
                            "ID Unico",
                            "ARCA",
                            "Primera fecha: Fecha de EmisiÃ³n",
                            "Suma de CÃ³d. AutorizaciÃ³n"
                        };

                        foreach (var col in columnasNecesarias)
                        {
                            if (!colIndexMap.ContainsKey(col))
                            {
                                MessageBox.Show($"No se encontró la columna '{col}' en el Excel.");
                                return;
                            }
                        }

                        int fila = 2;
                        int procesadas = 0;

                        while (!hoja.Cell(fila, colIndexMap["ID Unico"]).IsEmpty())
                        {
                            string dato = hoja.Cell(fila, colIndexMap["ID Unico"]).GetString().Trim();
                            string importe = hoja.Cell(fila, colIndexMap["ARCA"]).GetString().Trim();
                            string fecha = hoja.Cell(fila, colIndexMap["Primera fecha: Fecha de EmisiÃ³n"]).GetString().Trim();
                            string cae = hoja.Cell(fila, colIndexMap["Suma de CÃ³d. AutorizaciÃ³n"]).GetString().Trim();

                            if (string.IsNullOrWhiteSpace(dato))
                            {
                                fila++;
                                continue;
                            }

                            string[] partes = dato.Split('-');
                            if (partes.Length != 3)
                            {
                                dtResultados.Rows.Add(dato, "-", "-", "-", "-", importe, fecha, cae, "⚠️ Formato inválido");
                                fila++;
                                continue;
                            }

                            string suc_codigo = partes[0].PadLeft(4, '0');
                            string vene_numero = partes[1].PadLeft(8, '0');
                            string cbtee_codigo =
                                partes[2] == "1" ? "FAA" :
                                partes[2] == "6" ? "FAB" : "DESCONOCIDO";

                            string query = @"
                                SELECT TOP 1 PERI_CODIGO 
                                FROM ventas_e 
                                WHERE suc_codigo = @suc 
                                  AND vene_numero = @num 
                                  AND cbtee_codigo = @tipo;

                                SELECT TOP 1 PERI_CODIGO 
                                FROM ventas_e 
                                WHERE suc_codigo = @suc;
                            ";

                            string peri = "-";
                            string resultado = "❌ No existe";

                            using (var cmd = new SqlCommand(query, conexion))
                            {
                                cmd.Parameters.AddWithValue("@suc", suc_codigo);
                                cmd.Parameters.AddWithValue("@num", vene_numero);
                                cmd.Parameters.AddWithValue("@tipo", cbtee_codigo);

                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read() && reader[0] != DBNull.Value)
                                    {
                                        peri = reader[0].ToString();
                                        resultado = "✅ Existe";
                                    }
                                    else if (reader.NextResult() && reader.Read() && reader[0] != DBNull.Value)
                                    {
                                        peri = reader[0].ToString();
                                    }
                                }
                            }

                            dtResultados.Rows.Add(
                                dato,
                                peri,
                                suc_codigo,
                                vene_numero,
                                cbtee_codigo,
                                importe,
                                fecha,
                                cae,
                                resultado
                            );

                            procesadas++;
                            fila++;
                        }

                        dtOriginal = dtResultados;
                        dgvResultados.DataSource = dtOriginal;

                        AgregarColumnaCheck();
                        foreach (DataGridViewColumn col in dgvResultados.Columns)
                            col.ReadOnly = true;

                        dgvResultados.Columns["Seleccionado"].ReadOnly = false;

                        CargarComboDesdeTabla(dtOriginal, cbSucursal, "Sucursal");
                        CargarComboDesdeTabla(dtOriginal, cbLocal, "Local");

                        lblEstado.Text = $"✅ Proceso finalizado. {procesadas} filas procesadas.";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("Error durante el proceso: " + ex.Message);
            }
            finally
            {
                btnProcesar.Enabled = true;
            }
        }

        private void AgregarColumnaCheck()
        {
            // Evitar duplicarla si ya existe
            if (dgvResultados.Columns.Contains("Seleccionado"))
                return;

            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionado";
            chk.HeaderText = "";
            chk.Width = 40;
            chk.ReadOnly = false; // IMPORTANTE
            chk.FalseValue = false;
            chk.TrueValue = true;


            // La insertamos como primera columna
            dgvResultados.Columns.Insert(0, chk);

        }

        private void dgvResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvResultados.Columns[e.ColumnIndex].Name == "Seleccionado")
            {
                dgvResultados.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }



        private void ComboFiltro_Changed(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (dtOriginal == null) return;

            DataView dv = dtOriginal.DefaultView;
            string filtro = "";

            // Filtro por sucursal
            if (cbSucursal.SelectedItem != null && cbSucursal.Text != "Todos")
                filtro += $"Sucursal = '{cbSucursal.Text.Replace("'", "''")}'";

            // Filtro por local
            if (cbLocal.SelectedItem != null && cbLocal.Text != "Todos")
            {
                if (filtro != "") filtro += " AND ";
                filtro += $"Local = '{cbLocal.Text.Replace("'", "''")}'";
            }

            // Filtro "Existe"
            if (chkSoloExistentes.Checked)
            {
                if (filtro != "") filtro += " AND ";
                filtro += "[Existe] = '✅ Existe'";
            }

            // Filtro "No existe"
            if (chkSoloNoExistentes.Checked)
            {
                if (filtro != "") filtro += " AND ";
                filtro += "[Existe] = '❌ No existe'";
            }

            dv.RowFilter = filtro;
            dgvResultados.DataSource = dv;

            CargarComboDesdeVista(dv, cbSucursal, "Sucursal");
            CargarComboDesdeVista(dv, cbLocal, "Local");
        }

        private void CargarComboDesdeTabla(DataTable tabla, ComboBox combo, string columna)
        {
            if (tabla == null || !tabla.Columns.Contains(columna))
                return;

            var valores = tabla.AsEnumerable()
                .Select(r => r[columna]?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            combo.SelectedIndexChanged -= ComboFiltro_Changed;

            combo.Items.Clear();
            combo.Items.Add("Todos");
            combo.Items.AddRange(valores);
            combo.SelectedIndex = 0;

            combo.SelectedIndexChanged += ComboFiltro_Changed;
        }

        private void CargarComboDesdeVista(DataView vista, ComboBox combo, string columna)
        {
            if (vista == null || !vista.Table.Columns.Contains(columna))
                return;

            // Guardamos la selección actual del usuario
            string valorSeleccionado = combo.SelectedItem?.ToString();

            var valores = vista.ToTable()
                .AsEnumerable()
                .Select(r => r[columna]?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            combo.SelectedIndexChanged -= ComboFiltro_Changed;

            combo.Items.Clear();
            combo.Items.Add("Todos");
            combo.Items.AddRange(valores.ToArray());

            if (!string.IsNullOrWhiteSpace(valorSeleccionado) &&
                combo.Items.Contains(valorSeleccionado))
            {
                combo.SelectedItem = valorSeleccionado;
            }
            else
            {
                combo.SelectedIndex = 0;
            }

            combo.SelectedIndexChanged += ComboFiltro_Changed;
        }



        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvResultados.DataSource == null)
            {
                MessageBox.Show("No hay datos para exportar.");
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
                            DataTable dt = ((DataView)dgvResultados.DataSource).ToTable();
                            wb.Worksheets.Add(dt, "Resultados");
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Exportado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show("Error al exportar: " + ex.Message);
            }
        }

        private void chkMostrarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrarTodos.Checked)
            {
                chkSoloExistentes.Checked = false;
                chkSoloNoExistentes.Checked = false;
                AplicarFiltros();
            }
        }

        private void chkSoloExistentes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoloExistentes.Checked)
            {
                chkMostrarTodos.Checked = false;
                chkSoloNoExistentes.Checked = false;
                AplicarFiltros();
            }
        }

        private void chkSoloNoExistentes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoloNoExistentes.Checked)
            {
                chkMostrarTodos.Checked = false;
                chkSoloExistentes.Checked = false;
                AplicarFiltros();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiarFiltros()
        {
            
            if (cbSucursal.Items.Count > 0) cbSucursal.SelectedIndex = 0;
            if (cbLocal.Items.Count > 0) cbLocal.SelectedIndex = 0;

            
            chkMostrarTodos.Checked = true;
            chkSoloExistentes.Checked = false;
            chkSoloNoExistentes.Checked = false;

            
            if (dtOriginal != null)
            {
                dgvResultados.DataSource = dtOriginal;

                
                CargarComboDesdeTabla(dtOriginal, cbSucursal, "Sucursal");
                CargarComboDesdeTabla(dtOriginal, cbLocal, "Local");
            }

            lblEstado.Text = "Filtros limpiados.";
        }


        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }
    }
}
