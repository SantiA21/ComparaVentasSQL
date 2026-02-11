using CinetCore.Data;
using CinetCore.Infrastructure;
using CinetCore.Services.Sucursales;
using CinetCore.Utils;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CinetCore
{
    public partial class FormVerSucursales : Form
    {
        private readonly DataAccess dataAccess;
        private readonly SucursalService sucursalService;

        private DataTable dtSucursales;
        private string selectedDbKey;

        public FormVerSucursales()
        {
            InitializeComponent();

            dataAccess = new DataAccess();
            sucursalService = new SucursalService(dataAccess);

            InicializarComboBases();
            InicializarEventos();
        }

        private void FormVerSucursales_Load(object sender, EventArgs e)
        {
            CargarSucursales();
            MostrarVersion();
        }

        private void InicializarComboBases()
        {
            var keys = dataAccess.GetKeys();
            cbBaseDatos.Items.AddRange(keys);

            if (cbBaseDatos.Items.Count > 0)
            {
                cbBaseDatos.SelectedIndex = 0;
                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
            }
        }

        private void InicializarEventos()
        {
            cbBaseDatos.SelectedIndexChanged += (s, e) =>
            {
                if (cbBaseDatos.SelectedItem == null) return;

                selectedDbKey = cbBaseDatos.SelectedItem.ToString();
                CargarSucursales();
                AplicarFiltro();
            };

            txtBuscar.TextChanged += (s, e) => AplicarFiltro();
        }

        private void CargarSucursales()
        {
            try
            {
                dtSucursales = sucursalService.ObtenerSucursales(selectedDbKey);
                dgvSucursales.DataSource = dtSucursales;

                ConfigurarGrid();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show(
                    UserMessageHelper.GetFriendlyMessage("al cargar las sucursales desde la base de datos", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void ConfigurarGrid()
        {
            dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSucursales.AllowUserToAddRows = false;
            dgvSucursales.ReadOnly = true;
            dgvSucursales.RowHeadersVisible = false;
        }

        private void AplicarFiltro()
        {
            if (dtSucursales == null) return;

            string filtro = txtBuscar.Text.Trim().Replace("'", "''");

            var vista = dtSucursales.DefaultView;

            vista.RowFilter = string.IsNullOrEmpty(filtro)
                ? ""
                : $"Convert(Sucursal, 'System.String') LIKE '%{filtro}%' " +
                  $"OR Convert(Local, 'System.String') LIKE '%{filtro}%'";
        }

        private void MostrarVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
