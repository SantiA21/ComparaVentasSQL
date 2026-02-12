using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Services.Usuarios;
using CinetCore.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinetCore
{
    public partial class FormUsuariosGmgERP : Form
    {
        private static readonly HttpClient client = new HttpClient();

        private int currentPage = 1;
        private int pageSize = 50;
        private int totalRecords = 0;

        private System.Windows.Forms.Timer searchTimer;

        private DataAccess dataAccess;
        private DataTable dtUsuarios;
        public FormUsuariosGmgERP()
        {
            InitializeComponent();

            searchTimer = new System.Windows.Forms.Timer();
            searchTimer.Interval = 500; // medio segundo
            searchTimer.Tick += async (s, e) =>
            {
                searchTimer.Stop();
                currentPage = 1;
                await CargarUsuarios();
            };

            nudPagina.Minimum = 1;
            nudPagina.Value = 1;
            nudPagina.Maximum = 1; // después lo actualizamos dinámicamente
        }

        private async void FormUsuariosGmgERP_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";

            await CargarUsuarios();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private async Task<PagedResponse<Usuario>> ObtenerUsuariosAsync(string buscar = null)
        {
            string url = $"http://localhost:5000/api/usuarios/gmg?page={currentPage}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(buscar))
                url += $"&buscar={buscar}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PagedResponse<Usuario>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        private async Task CargarUsuarios()
        {
            try
            {
                var resultado = await ObtenerUsuariosAsync(txtBuscar.Text);

                if (resultado == null) return;

                totalRecords = resultado.Total;

                int totalPaginas = (int)Math.Ceiling((double)totalRecords / pageSize);

                nudPagina.Maximum = totalPaginas == 0 ? 1 : totalPaginas;
                nudPagina.Value = currentPage;

                dgvUsuarios.DataSource = resultado.Data;

                lblPagina.Text = $"Página {currentPage}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private async void nudPagina_ValueChanged(object sender, EventArgs e)
        {
            if (currentPage == (int)nudPagina.Value)
                return;

            currentPage = (int)nudPagina.Value;
            await CargarUsuarios();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
