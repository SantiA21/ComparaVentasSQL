using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormLogin : Form
    {
        private readonly DataAccess dataAccess;

        public FormLogin()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Obtiene la versión del ensamblado actual
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Opcional: convertirlo a texto amigable
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete ambos campos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conexion = dataAccess.GetConnection())
                {
                    conexion.Open();

                    // Ajustá el nombre de la tabla y columnas según tu base de datos
                    string query = @"
                        SELECT COUNT(*) 
                        FROM usuarios 
                        WHERE usuario = @usuario AND clave = @password and usu_categoria = 'ADMIN' and estado = 'A'";

                    using (var cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Login exitoso
                            this.Hide();
                            FormInicio mainForm = new FormInicio();
                            mainForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
