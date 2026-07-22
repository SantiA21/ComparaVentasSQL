using CinetCore.Utils;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinetCore.Infrastructure;

namespace CinetCore.Forms.Salvaventas
{
    public partial class FormConexionSalvaventas : Form
    {
        private static string _savedPassword = "";
        private static bool _remember = false;

        private TextBox txtIp;
        private TextBox txtPassword;
        private CheckBox chkRecordar;
        private Button btnConectar;
        private Label lblStatus;
        private Label lblIp;
        private Label lblPassword;
        private Label lblVersion;

        public FormConexionSalvaventas()
        {
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
            lblVersion.Visible = false;
            
            if (_remember)
            {
                txtPassword.Text = _savedPassword;
                chkRecordar.Checked = true;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Conexión a Base de Datos - Salvaventas";
            this.Size = new Size(420, 360);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            var lblTitle = new Label() { Text = "Iniciar Sesión", Location = new Point(50, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64) };

            lblIp = new Label() { Text = "Dirección IP:", Location = new Point(50, 70), AutoSize = true, ForeColor = Color.Gray };
            txtIp = new TextBox() { Location = new Point(50, 95), Width = 300, Font = new Font("Segoe UI", 10F) };
            
            lblPassword = new Label() { Text = "Contraseña:", Location = new Point(50, 135), AutoSize = true, ForeColor = Color.Gray };
            txtPassword = new TextBox() { Location = new Point(50, 160), Width = 300, PasswordChar = '•', Font = new Font("Segoe UI", 10F) };
            
            chkRecordar = new CheckBox() { Text = "Recordar Contraseña", Location = new Point(50, 195), AutoSize = true, ForeColor = Color.FromArgb(64, 64, 64) };
            
            btnConectar = new Button() { Text = "CONECTAR", Location = new Point(50, 235), Width = 300, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnConectar.FlatAppearance.BorderSize = 0;
            btnConectar.Click += async (s, e) => await BtnConectar_Click(s, e);

            lblStatus = new Label() { Text = "", Location = new Point(50, 285), Width = 300, TextAlign = ContentAlignment.MiddleCenter };
            lblVersion = new Label() { Text = "", Location = new Point(360, 295), AutoSize = true, ForeColor = Color.LightGray };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblIp);
            this.Controls.Add(txtIp);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(chkRecordar);
            this.Controls.Add(btnConectar);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblVersion);
        }

        private async Task BtnConectar_Click(object sender, EventArgs e)
        {
            string ip = txtIp.Text.Trim();
            string password = txtPassword.Text;

            if (chkRecordar.Checked)
            {
                _savedPassword = password;
                _remember = true;
            }
            else
            {
                _savedPassword = "";
                _remember = false;
            }

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(password))
            {
                CinetCore.Utils.Alert.Show("Por favor, ingrese la IP y la contraseña.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetLoading(true, "Conectando al servidor central...");

            try
            {
                string connectionString = $"Data Source={ip};Initial Catalog=backoffice;User ID=sa;Password={password};TrustServerCertificate=True;Connection Timeout=15;";
                
                // Probar conexión
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                Logger.Info($"Conexión exitosa a IP: {ip}");

                string codLocal = "Desconocido";
                try
                {
                    using var command = new SqlCommand("SELECT para_valor FROM parametros WHERE para_codigo = 'NOMLOCAL'", connection);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        codLocal = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error al obtener NOMLOCAL", ex);
                }

                try 
                {
                    FormMainSalvaventas mainWindow = new FormMainSalvaventas(ip, password, codLocal);
                    mainWindow.Show();
                    this.Close();
                }
                catch (Exception winEx)
                {
                    Logger.Error("Error al abrir la ventana principal", winEx);
                    CinetCore.Utils.Alert.Show($"Error interno al cargar la ventana principal:\n{winEx.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error al intentar conectar con la IP {ip}", ex);
                lblStatus.Text = "Error de conexión.";
                lblStatus.ForeColor = Color.Red;
                CinetCore.Utils.Alert.Show($"No se pudo conectar a la base de datos.\n\nDetalles:\n{ex.Message}", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoading(false, "");
            }
        }

        private void SetLoading(bool isLoading, string statusText)
        {
            btnConectar.Enabled = !isLoading;
            txtIp.Enabled = !isLoading;
            txtPassword.Enabled = !isLoading;
            chkRecordar.Enabled = !isLoading;
            lblStatus.Text = statusText;
            lblStatus.ForeColor = Color.Gray;
        }
    }
}
