using CinetCore.Utils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinetCore.Services.Salvaventas;
using CinetCore.Infrastructure;

namespace CinetCore.Forms.Salvaventas
{
    public partial class FormInsertarVenta : Form
    {
        private string _ip;
        private string _password;

        private TextBox txtSucursal;
        private TextBox txtNumero;
        private TextBox txtTipo;
        private TextBox txtNumCaja;
        private DateTimePicker dpFecha;
        private TextBox txtImporte;
        private TextBox txtCae;
        private ComboBox cmbValCodigo;
        private Button btnInsertar;
        private Button btnCancelar;

        public FormInsertarVenta(string ip, string password, string sucCodigo, string veneNumero, string cbteeCodigo, string valCodigo)
        {
            _ip = ip;
            _password = password;
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);

            txtSucursal.Text = sucCodigo;
            txtNumero.Text = veneNumero;
            txtTipo.Text = cbteeCodigo;
            
            if (!string.IsNullOrEmpty(valCodigo))
            {
                cmbValCodigo.SelectedItem = valCodigo;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Insertar Venta Manual";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;

            this.Controls.Add(new Label() { Text = "Sucursal:", Location = new Point(20, y), AutoSize = true });
            txtSucursal = new TextBox() { Location = new Point(120, y - 2), Width = 230, ReadOnly = true };
            this.Controls.Add(txtSucursal);

            y += 40;
            this.Controls.Add(new Label() { Text = "Número:", Location = new Point(20, y), AutoSize = true });
            txtNumero = new TextBox() { Location = new Point(120, y - 2), Width = 230, ReadOnly = true };
            this.Controls.Add(txtNumero);

            y += 40;
            this.Controls.Add(new Label() { Text = "Tipo:", Location = new Point(20, y), AutoSize = true });
            txtTipo = new TextBox() { Location = new Point(120, y - 2), Width = 230, ReadOnly = true };
            this.Controls.Add(txtTipo);

            y += 40;
            this.Controls.Add(new Label() { Text = "Núm Caja:", Location = new Point(20, y), AutoSize = true });
            txtNumCaja = new TextBox() { Location = new Point(120, y - 2), Width = 230, Text = "1" };
            this.Controls.Add(txtNumCaja);

            y += 40;
            this.Controls.Add(new Label() { Text = "Fecha:", Location = new Point(20, y), AutoSize = true });
            dpFecha = new DateTimePicker() { Location = new Point(120, y - 2), Width = 230, Format = DateTimePickerFormat.Short };
            this.Controls.Add(dpFecha);

            y += 40;
            this.Controls.Add(new Label() { Text = "Importe:", Location = new Point(20, y), AutoSize = true });
            txtImporte = new TextBox() { Location = new Point(120, y - 2), Width = 230 };
            this.Controls.Add(txtImporte);

            y += 40;
            this.Controls.Add(new Label() { Text = "CAE:", Location = new Point(20, y), AutoSize = true });
            txtCae = new TextBox() { Location = new Point(120, y - 2), Width = 230 };
            this.Controls.Add(txtCae);

            y += 40;
            this.Controls.Add(new Label() { Text = "Val Código:", Location = new Point(20, y), AutoSize = true });
            cmbValCodigo = new ComboBox() { Location = new Point(120, y - 2), Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbValCodigo.Items.AddRange(new object[] { "MERPAGO", "EFECTIVO", "HNC" });
            this.Controls.Add(cmbValCodigo);

            y += 50;
            btnCancelar = new Button() { Text = "CANCELAR", Location = new Point(70, y), Width = 120, Height = 35 };
            btnCancelar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancelar);

            btnInsertar = new Button() { Text = "INSERTAR", Location = new Point(200, y), Width = 120, Height = 35, BackColor = Color.LightBlue };
            btnInsertar.Click += async (s, e) => await BtnInsertar_Click(s, e);
            this.Controls.Add(btnInsertar);
        }

        private async Task BtnInsertar_Click(object sender, EventArgs e)
        {
            string sucCodigo = txtSucursal.Text.Trim();
            string veneNumero = txtNumero.Text.Trim();
            string cbteeCodigo = txtTipo.Text.Trim();
            string numCaja = txtNumCaja.Text.Trim();
            string importeText = txtImporte.Text.Trim();
            string cae = txtCae.Text.Trim();
            string valCodigo = cmbValCodigo.SelectedItem?.ToString();
            DateTime fecha = dpFecha.Value;

            if (string.IsNullOrEmpty(numCaja) || string.IsNullOrEmpty(importeText) || string.IsNullOrEmpty(valCodigo))
            {
                CinetCore.Utils.Alert.Show("Por favor, complete todos los campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(importeText.Replace(".", ","), out decimal importeTotal))
            {
                CinetCore.Utils.Alert.Show("El importe ingresado no es válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnInsertar.Enabled = false;
                var dbService = new DatabaseService(_ip, _password);

                if (valCodigo == "HNC")
                {
                    bool existsHnc = await dbService.CheckHNCExistsAsync();
                    if (!existsHnc)
                    {
                        if (CinetCore.Utils.Alert.Show("El registro 'HNC' no existe en VALORES_TIPOS del backoffice.\n¿Desea insertarlo ahora?", "Registro Inexistente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            await dbService.InsertarHNCAsync();
                        }
                    }
                }

                await dbService.InsertarVentaManualAsync(sucCodigo, veneNumero, cbteeCodigo, fecha, importeTotal, cae, int.Parse(numCaja), valCodigo);

                CinetCore.Utils.Alert.Show("La venta se insertó manualmente en el Backoffice de manera exitosa.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnInsertar_Click (Manual)", ex);
                CinetCore.Utils.Alert.Show($"Ocurrió un error al intentar insertar la venta manualmente:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnInsertar.Enabled = true;
            }
        }
    }
}
