using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Utils;
using ClosedXML.Excel;
using CinetCore.Services.ComparacionExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace CinetCore;

public partial class FormComparaExcel : Form
{
    private DataAccess dataAccess;
    private DataTable dtOriginal;
    private string selectedDbKey;

    public FormComparaExcel()
    {
        InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);

        dataAccess = CinetCore.Infrastructure.AppDI.GetService<CinetCore.Data.DataAccess>();
        dgvResultados.RowHeadersVisible = false;

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

    private DataTable CrearTablaResultados()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("ID Unico");
        dt.Columns.Add("Local");
        dt.Columns.Add("Sucursal");
        dt.Columns.Add("Comprobante");
        dt.Columns.Add("Tipo");
        dt.Columns.Add("Importe");
        dt.Columns.Add("Fecha");
        dt.Columns.Add("CAE");
        dt.Columns.Add("Resultado");

        return dt;
    }

    private void btnProcesar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtArchivo.Text))
        {
            CinetCore.Utils.Alert.Show("Seleccione un archivo Excel primero.");
            return;
        }

        try
        {
            var excelService = new ExcelComparacionService();
            var ventasService = new VentasERPService(dataAccess);

            DataTable dt = CrearTablaResultados();

            foreach (var fila in excelService.LeerExcel(txtArchivo.Text))
            {
                if (!IdUnicoParser.TryParse(
                    fila.IdUnico,
                    out string sucursal,
                    out string comprobante,
                    out string tipo))
                {
                    continue;
                }

                var resultado = ventasService.ObtenerLocalYExistencia(
                    selectedDbKey,
                    sucursal,
                    comprobante,
                    tipo
                );

                dt.Rows.Add(
                    fila.IdUnico,
                    resultado.Local, 
                    sucursal,
                    comprobante,
                    tipo,
                    fila.Importe,
                    fila.Fecha,
                    fila.CAE,
                    resultado.Existe
                );
            }


            dtOriginal = dt;
            dgvResultados.DataSource = dt;

            AgregarColumnaCheck();
            CargarComboDesdeTabla(dtOriginal, cbSucursal, "Sucursal");
            CargarComboDesdeTabla(dtOriginal, cbLocal, "Local");

            lblEstado.Text = "✅ Proceso finalizado";
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            CinetCore.Utils.Alert.Show(
                UserMessageHelper.GetFriendlyMessage("al comparar las ventas con la base de datos", ex),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }



    private void AgregarColumnaCheck()
    {

        if (dgvResultados.Columns.Contains("Seleccionado"))
            return;

        DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
        chk.Name = "Seleccionado";
        chk.HeaderText = "";
        chk.Width = 40;
        chk.ReadOnly = false; 
        chk.FalseValue = false;
        chk.TrueValue = true;



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


        if (cbSucursal.SelectedItem != null && cbSucursal.Text != "Todos")
            filtro += $"Sucursal = '{cbSucursal.Text.Replace("'", "''")}'";


        if (cbLocal.SelectedItem != null && cbLocal.Text != "Todos")
        {
            if (filtro != "") filtro += " AND ";
            filtro += $"Local = '{cbLocal.Text.Replace("'", "''")}'";
        }


        if (chkSoloExistentes.Checked)
        {
            if (filtro != "") filtro += " AND ";
            filtro += "[Resultado] = '✅ Existe'";
        }


        if (chkSoloNoExistentes.Checked)
        {
            if (filtro != "") filtro += " AND ";
            filtro += "[Resultado] = '❌ No existe'";
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
            CinetCore.Utils.Alert.Show("No hay datos para exportar.");
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

                    CinetCore.Utils.Alert.Show("Exportado correctamente.");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            CinetCore.Utils.Alert.Show("Error al exportar: " + ex.Message);
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
        lblVersion.Visible = false;
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

    private void lblVersion_Click(object sender, EventArgs e)
    {

    }
}
