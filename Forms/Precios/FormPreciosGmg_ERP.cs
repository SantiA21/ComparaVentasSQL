using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinetCore.Forms.Precios
{
    public partial class FormPreciosGmg_ERP : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private int currentPage = 1;
        private int pageSize = 100;
        private int totalRecords = 0;
        private bool primeraCarga = true;
        private System.Windows.Forms.Timer searchTimer;
        public FormPreciosGmg_ERP()
        {
            InitializeComponent();
            searchTimer = new System.Windows.Forms.Timer();
            searchTimer.Interval = 500;
            searchTimer.Tick += async (s, e) =>
            {
                searchTimer.Stop();
                currentPage = 1;
                await CargarPrecios();
            };

            nudPagina.Minimum = 1;
            nudPagina.Value = 1;
            nudPagina.Maximum = 1;
        }

        private void MostrarLoading()
        {
            pnlLoading.Visible = true;
            pnlLoading.BringToFront();
            this.UseWaitCursor = true;
        }

        private void OcultarLoading()
        {
            pnlLoading.Visible = false;
            this.UseWaitCursor = false;
        }

        private async void FormPreciosGmg_ERP_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";

            lblLoading.Left = (pnlLoading.Width - lblLoading.Width) / 2;
            lblLoading.Top = (pnlLoading.Height / 2) - 30;

            progressBarLoading.Left = (pnlLoading.Width - progressBarLoading.Width) / 2;
            progressBarLoading.Top = lblLoading.Bottom + 10;

            await CargarPrecios();
        }

        private async Task CargarPrecios()
        {
            try
            {
                if (primeraCarga)
                    MostrarLoading();

                var resultado = await ObtenerPreciosAsync(
                    txtLista.Text.Trim(),
                    txtArticulo.Text.Trim());

                if (resultado == null) return;

                totalRecords = resultado.Total;

                dgvPrecios.DataSource = resultado.Data;

                int totalPaginas = (int)Math.Ceiling((double)totalRecords / pageSize);
                nudPagina.Maximum = totalPaginas == 0 ? 1 : totalPaginas;
                nudPagina.Value = currentPage;

                primeraCarga = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (!primeraCarga)
                    OcultarLoading();
            }
        }

        private async Task<PagedResponse<PrecioDto>> ObtenerPreciosAsync(
    string lista,
    string articulo)
        {
            var baseUrl = "http://localhost:5000/api/precios/mostaza";

            var parametros = new List<string>
    {
        $"page={currentPage}",
        $"pageSize={pageSize}"
    };

            if (!string.IsNullOrWhiteSpace(lista))
                parametros.Add($"lista={Uri.EscapeDataString(lista)}");

            if (!string.IsNullOrWhiteSpace(articulo))
                parametros.Add($"articulo={Uri.EscapeDataString(articulo)}");

            var url = baseUrl + "?" + string.Join("&", parametros);

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PagedResponse<PrecioDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            btnBuscar.Enabled = false;

            try
            {
                var precios = await ObtenerPreciosAsync(
                    txtLista.Text.Trim(),
                    txtArticulo.Text.Trim());

                dgvPrecios.DataSource = precios;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = true;
            }
        }

        private async void nudPagina_ValueChanged(object sender, EventArgs e)
        {
            currentPage = (int)nudPagina.Value;
            await CargarPrecios();
        }

        private async void txtFiltros_TextChanged(object sender, EventArgs e)
        {
            currentPage = (int)nudPagina.Value;
            await CargarPrecios();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
