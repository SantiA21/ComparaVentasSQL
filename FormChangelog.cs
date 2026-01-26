using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ComparaVentasExcel
{
    public partial class FormChangelog : Form
    {
        private const string CHANGELOG_URL =
            "https://raw.githubusercontent.com/SantiA21/ComparaVentasSQL/main/repo/changelog.txt";

        public FormChangelog()
        {
            InitializeComponent();
            CargarChangelogAsync();
        }

        private async void CargarChangelogAsync()
        {
            txtChangelog.Text = "Cargando novedades...";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);

                    string contenido = await client.GetStringAsync(CHANGELOG_URL);

                    contenido = contenido.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

                    txtChangelog.Text = string.IsNullOrWhiteSpace(contenido)
                        ? "No hay novedades para mostrar."
                        : contenido;
                }
            }
            catch (Exception ex)
            {
                txtChangelog.Text =
                    "No se pudieron cargar las novedades.\r\n" +
                    "Verifique su conexión a internet.\r\n\r\n" +
                    ex.Message;
            }
        }
    }

}