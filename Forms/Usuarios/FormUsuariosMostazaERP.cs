using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinetCore.Services.Usuarios;
using CinetCore.Infrastructure;
using CinetCore.Data;
using CinetCore.Utils;
using System.Net.Http;
using System.Text.Json;

namespace CinetCore
{
    public partial class FormUsuariosMostazaERP : Form
    {
        private static readonly HttpClient client = new HttpClient();

        private int currentPage = 1;
        private int pageSize = 50;
        private int totalRecords = 0;
        private bool primeraCarga = true;

        private System.Windows.Forms.Timer searchTimer;

        private DataAccess dataAccess;
        private DataTable dtUsuarios;
        public FormUsuariosMostazaERP()
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

        private async void FormUsuariosMostazaERP_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";

            lblLoading.Left = (pnlLoading.Width - lblLoading.Width) / 2;
            lblLoading.Top = (pnlLoading.Height / 2) - 30;

            progressBarLoading.Left = (pnlLoading.Width - progressBarLoading.Width) / 2;
            progressBarLoading.Top = lblLoading.Bottom + 10;

            await CargarUsuarios();
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

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private async Task<PagedResponse<Usuario>> ObtenerUsuariosAsync(string buscar = null)
        {
            string url = $"http://localhost:5000/api/usuarios/mostaza?page={currentPage}&pageSize={pageSize}";

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
                if (primeraCarga)
                    MostrarLoading();

                var resultado = await ObtenerUsuariosAsync(txtBuscar.Text);

                if (resultado == null) return;

                totalRecords = resultado.Total;

                dgvUsuarios.DataSource = resultado.Data;

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
                if (primeraCarga == false)
                    OcultarLoading();
            }
        }
        private async void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void nudPagina_ValueChanged(object sender, EventArgs e)
        {
            if (currentPage == (int)nudPagina.Value)
                return;

            currentPage = (int)nudPagina.Value;
            await CargarUsuarios();
        }
    }
}
