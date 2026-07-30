using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using CinetCore.Data;
using CinetCore.Infrastructure;

namespace CinetCore.Forms.Comunes
{
    public class FormHealthCheck : Form
    {
        private DataGridView dgvEstado;
        private Button btnProbar;
        private Button btnCerrar;
        private Label lblEstadoGeneral;

        public FormHealthCheck()
        {
            InitializeComponent();
            this.Load += (s, e) => EjecutarPruebasAsync();
        }

        private void InitializeComponent()
        {
            this.Text = "Diagnóstico de Conectividad y Estado de Bases de Datos (Health Check)";
            this.Size = new Size(850, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 350);

            var panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                Padding = new Padding(12)
            };

            lblEstadoGeneral = new Label
            {
                Text = "Verificando conexiones configuradas en dbconfig.ini...",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(12, 18)
            };

            btnProbar = new Button
            {
                Text = "⚡ Volver a Probar",
                Location = new Point(530, 13),
                Width = 140,
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnProbar.Click += async (s, e) => await EjecutarPruebasAsync();

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Location = new Point(680, 13),
                Width = 100,
                Height = 30,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCerrar.Click += (s, e) => this.Close();

            panelSuperior.Controls.Add(lblEstadoGeneral);
            panelSuperior.Controls.Add(btnProbar);
            panelSuperior.Controls.Add(btnCerrar);

            dgvEstado = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            dgvEstado.Columns.Add("BaseDatos", "Base de Datos");
            dgvEstado.Columns.Add("Estado", "Estado");
            dgvEstado.Columns.Add("Latencia", "Latencia");
            dgvEstado.Columns.Add("Detalle", "Versión SQL / Detalle de Error");

            dgvEstado.Columns["BaseDatos"].Width = 160;
            dgvEstado.Columns["Estado"].Width = 130;
            dgvEstado.Columns["Latencia"].Width = 100;
            dgvEstado.Columns["Detalle"].Width = 400;

            this.Controls.Add(dgvEstado);
            this.Controls.Add(panelSuperior);
        }

        private async Task EjecutarPruebasAsync()
        {
            btnProbar.Enabled = false;
            dgvEstado.Rows.Clear();
            lblEstadoGeneral.Text = "Verificando conexiones en tiempo real...";
            lblEstadoGeneral.ForeColor = Color.DarkBlue;

            var dataAccess = new DataAccess();
            var configs = dataAccess.GetAllConnectionStrings();

            if (configs.Count == 0)
            {
                lblEstadoGeneral.Text = "No se encontraron conexiones configuradas en dbconfig.ini.";
                btnProbar.Enabled = true;
                return;
            }

            int exitosas = 0;

            foreach (var kvp in configs)
            {
                int rowIndex = dgvEstado.Rows.Add(kvp.Key, "🟡 Probando...", "...", "");
                var row = dgvEstado.Rows[rowIndex];

                var resultado = await ProbarConexionIndividualAsync(kvp.Value);
                if (resultado.Exito)
                {
                    exitosas++;
                    row.Cells["Estado"].Value = "🟢 Conectado";
                    row.Cells["Estado"].Style.ForeColor = Color.DarkGreen;
                    row.Cells["Latencia"].Value = $"{resultado.LatenciaMs} ms";
                    row.Cells["Detalle"].Value = resultado.VersionSql;
                }
                else
                {
                    row.Cells["Estado"].Value = "🔴 Error";
                    row.Cells["Estado"].Style.ForeColor = Color.DarkRed;
                    row.Cells["Latencia"].Value = "N/A";
                    row.Cells["Detalle"].Value = resultado.ErrorMsg;
                }
            }

            if (exitosas == configs.Count)
            {
                lblEstadoGeneral.Text = $"✅ Todas las conexiones ({exitosas}/{configs.Count}) están operativas y accesibles.";
                lblEstadoGeneral.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblEstadoGeneral.Text = $"⚠️ {exitosas} de {configs.Count} bases de datos operativas. Revisa los errores.";
                lblEstadoGeneral.ForeColor = Color.DarkOrange;
            }

            btnProbar.Enabled = true;
        }

        private async Task<(bool Exito, long LatenciaMs, string VersionSql, string ErrorMsg)> ProbarConexionIndividualAsync(string connectionString)
        {
            var sw = Stopwatch.StartNew();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                // Ajustar Connect Timeout a 4s si no está configurado explícitamente corto
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (builder.ConnectTimeout > 5)
                {
                    builder.ConnectTimeout = 5;
                }
                builder.TrustServerCertificate = true;

                using var conn = new SqlConnection(builder.ConnectionString);
                await conn.OpenAsync(cts.Token);
                
                using var cmd = new SqlCommand("SELECT @@VERSION", conn);
                var obj = await cmd.ExecuteScalarAsync(cts.Token);
                sw.Stop();

                string verStr = obj?.ToString() ?? "SQL Server (Conectado)";
                string lineaResumida = verStr.Split('\n')[0].Trim();

                return (true, sw.ElapsedMilliseconds, lineaResumida, "");
            }
            catch (Exception ex)
            {
                sw.Stop();
                Logger.LogError(ex);
                return (false, 0, "", ex.Message);
            }
        }
    }
}
