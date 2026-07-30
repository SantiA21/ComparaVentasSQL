using DocumentFormat.OpenXml.Office.Word;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using CinetCore.Utils;
using CinetCore.Infrastructure;

namespace CinetCore
{
    public partial class FormVerSucursalesV2 : Form
    {
        private readonly string _connectionString;

        public FormVerSucursalesV2(string connectionString)
        {
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
            _connectionString = connectionString;
        }

        private void FormVerSucursalesV2_Load(object sender, EventArgs e)
        {
            CargarSucursales();
        }

        private void CargarSucursales()
        {
            try
            {
                DataTable dt = CinetCore.Services.Sucursales.SucursalService.ObtenerSucursalesRemotas(_connectionString);
                dgvSucursales.DataSource = dt;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                CinetCore.Utils.Alert.Show(
                    UserMessageHelper.GetFriendlyMessage("al cargar las sucursales desde el servidor madre", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
