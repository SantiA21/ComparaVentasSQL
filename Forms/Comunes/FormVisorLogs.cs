using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CinetCore.Infrastructure;

namespace CinetCore.Forms.Comunes
{
    public class FormVisorLogs : Form
    {
        private ComboBox cbArchivos;
        private TextBox txtContenido;
        private Button btnRecargar;
        private Button btnCopiar;
        private Button btnCerrar;
        private Label lblTotalLineas;

        public FormVisorLogs()
        {
            InitializeComponent();
            CargarListaArchivos();
        }

        private void InitializeComponent()
        {
            this.Text = "Visor de Logs y Errores del Sistema";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 400);

            var panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10)
            };

            var lblArchivo = new Label
            {
                Text = "Archivo de Log:",
                AutoSize = true,
                Location = new Point(12, 16)
            };

            cbArchivos = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(110, 13),
                Width = 280
            };
            cbArchivos.SelectedIndexChanged += CbArchivos_SelectedIndexChanged;

            btnRecargar = new Button
            {
                Text = "🔄 Recargar",
                Location = new Point(400, 11),
                Width = 95,
                Height = 27
            };
            btnRecargar.Click += (s, e) => CargarListaArchivos();

            btnCopiar = new Button
            {
                Text = "📋 Copiar Contenido",
                Location = new Point(505, 11),
                Width = 135,
                Height = 27
            };
            btnCopiar.Click += BtnCopiar_Click;

            panelSuperior.Controls.Add(lblArchivo);
            panelSuperior.Controls.Add(cbArchivos);
            panelSuperior.Controls.Add(btnRecargar);
            panelSuperior.Controls.Add(btnCopiar);

            var panelInferior = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 45,
                Padding = new Padding(10)
            };

            lblTotalLineas = new Label
            {
                Text = "Líneas: 0 | Tamaño: 0 KB",
                AutoSize = true,
                Location = new Point(12, 14)
            };

            btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(780, 9),
                Width = 90,
                Height = 27
            };
            btnCerrar.Click += (s, e) => this.Close();

            panelInferior.Controls.Add(lblTotalLineas);
            panelInferior.Controls.Add(btnCerrar);

            txtContenido = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
                WordWrap = false
            };

            this.Controls.Add(txtContenido);
            this.Controls.Add(panelSuperior);
            this.Controls.Add(panelInferior);
        }

        private void CargarListaArchivos()
        {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            var archivos = Directory.GetFiles(logDir, "*.txt")
                .OrderByDescending(f => File.GetLastWriteTime(f))
                .ToList();

            cbArchivos.Items.Clear();
            foreach (var archivo in archivos)
            {
                cbArchivos.Items.Add(Path.GetFileName(archivo));
            }

            if (cbArchivos.Items.Count > 0)
            {
                cbArchivos.SelectedIndex = 0;
            }
            else
            {
                txtContenido.Text = "No se encontraron archivos de log en el directorio.";
                lblTotalLineas.Text = "Líneas: 0 | Tamaño: 0 KB";
            }
        }

        private void CbArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbArchivos.SelectedItem == null) return;

            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string path = Path.Combine(logDir, cbArchivos.SelectedItem.ToString());

            try
            {
                if (File.Exists(path))
                {
                    using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = new StreamReader(stream);
                    string contenido = reader.ReadToEnd();
                    txtContenido.Text = contenido;
                    txtContenido.SelectionStart = 0;
                    txtContenido.SelectionLength = 0;

                    int lineas = contenido.Split('\n').Length;
                    long bytes = new FileInfo(path).Length;
                    lblTotalLineas.Text = $"Líneas: {lineas} | Tamaño: {(bytes / 1024.0):F2} KB";
                }
            }
            catch (Exception ex)
            {
                txtContenido.Text = $"Error al leer el archivo de log:\r\n{ex.Message}";
            }
        }

        private void BtnCopiar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtContenido.Text))
            {
                Clipboard.SetText(txtContenido.Text);
                MessageBox.Show("Contenido del log copiado al portapapeles.", "Visor de Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
