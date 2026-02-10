using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel.Forms.Precios
{
    public partial class FormPrecios : Form
    {
        public FormPrecios()
        {
            InitializeComponent();
        }

        private async Task<List<PrecioDto>> ObtenerPreciosAsync(
    string lista,
    string articulo)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7071/");

            var query = "api/precios?";

            if (!string.IsNullOrWhiteSpace(lista))
                query += $"lista={lista}&";

            if (!string.IsNullOrWhiteSpace(articulo))
                query += $"articulo={articulo}&";

            var response = await client.GetAsync(query);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<PrecioDto>>(
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

        private void FormPrecios_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
