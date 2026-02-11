using CinetCore.Infrastructure;
using CinetCore.Utils;
using CinetCore.Data;
using CinetCore.Models;
using CinetCore.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinetCore.Forms.Usuarios
{
    public partial class FormConexionRemota : Form
    {
        public FormConexionRemota()
        {
            InitializeComponent();
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIp.Text))
            {
                MessageBox.Show("Ingrese una IP");
                return;
            }

            var config = new ConexionBackOffice
            {
                Ip = txtIp.Text.Trim(),
                Database = "BackOffice",
                Usuario = "sa",
                Password = "cinettorcel"
            };

            var service = new BackOfficeService(new DataAccess());

            try
            {
                var resultados = service.EjecutarConsultaUsuarios(config);

                var frm = new FormResultadosBackOffice(resultados);
                frm.Show();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                MessageBox.Show(
                    UserMessageHelper.GetFriendlyMessage("al conectarse al Backoffice remoto o ejecutar la consulta de usuarios", ex),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
