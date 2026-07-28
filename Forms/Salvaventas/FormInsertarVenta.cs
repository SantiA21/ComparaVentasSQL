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

        public FormInsertarVenta(string ip, string password, string sucCodigo, string veneNumero, string cbteeCodigo, string valCodigo, decimal? importe = null, string cae = null, DateTime? fecha = null)
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

            if (importe.HasValue && importe.Value > 0)
            {
                txtImporte.Text = (importe.Value % 1 == 0)
                    ? ((long)importe.Value).ToString()
                    : importe.Value.ToString("0.##");
            }
            if (!string.IsNullOrEmpty(cae))
            {
                txtCae.Text = cae;
            }
            if (fecha.HasValue && fecha.Value > DateTime.MinValue)
            {
                dpFecha.Value = fecha.Value;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Insertar Venta Manual";
            this.Size = new Size(460, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var panelHeader = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(0, 122, 204)
            };
            var lblHeader = new Label()
            {
                Text = "CARGA MANUAL DE VENTA",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 14)
            };
            panelHeader.Controls.Add(lblHeader);
            this.Controls.Add(panelHeader);

            int y = 75;
            var fontLabel = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            var fontInput = new Font("Segoe UI", 10F);

            this.Controls.Add(new Label() { Text = "Sucursal:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtSucursal = new TextBox() { Location = new Point(140, y), Width = 260, ReadOnly = true, Font = fontInput, BackColor = Color.FromArgb(240, 242, 245) };
            this.Controls.Add(txtSucursal);

            y += 42;
            this.Controls.Add(new Label() { Text = "Número:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtNumero = new TextBox() { Location = new Point(140, y), Width = 260, ReadOnly = true, Font = fontInput, BackColor = Color.FromArgb(240, 242, 245) };
            this.Controls.Add(txtNumero);

            y += 42;
            this.Controls.Add(new Label() { Text = "Tipo:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtTipo = new TextBox() { Location = new Point(140, y), Width = 260, ReadOnly = true, Font = fontInput, BackColor = Color.FromArgb(240, 242, 245) };
            this.Controls.Add(txtTipo);

            y += 42;
            this.Controls.Add(new Label() { Text = "Núm Caja:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtNumCaja = new TextBox() { Location = new Point(140, y), Width = 260, Text = "1", Font = fontInput };
            this.Controls.Add(txtNumCaja);

            y += 42;
            this.Controls.Add(new Label() { Text = "Fecha:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            dpFecha = new DateTimePicker() { Location = new Point(140, y), Width = 260, Format = DateTimePickerFormat.Short, Font = fontInput };
            this.Controls.Add(dpFecha);

            y += 42;
            this.Controls.Add(new Label() { Text = "Importe:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtImporte = new TextBox() { Location = new Point(140, y), Width = 260, Font = fontInput };
            this.Controls.Add(txtImporte);

            y += 42;
            this.Controls.Add(new Label() { Text = "CAE:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            txtCae = new TextBox() { Location = new Point(140, y), Width = 260, Font = fontInput };
            this.Controls.Add(txtCae);

            y += 42;
            this.Controls.Add(new Label() { Text = "Medio Pago:", Location = new Point(30, y + 3), AutoSize = true, Font = fontLabel });
            cmbValCodigo = new ComboBox() { Location = new Point(140, y), Width = 260, DropDownStyle = ComboBoxStyle.DropDownList, Font = fontInput };
            cmbValCodigo.Items.AddRange(new object[] { "MERPAGO", "EFECTIVO", "HNC" });
            this.Controls.Add(cmbValCodigo);

            y += 55;
            btnCancelar = new Button() { Text = "CANCELAR", Location = new Point(80, y), Width = 130, Height = 40, Font = fontLabel, Cursor = Cursors.Hand };
            btnCancelar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancelar);

            btnInsertar = new Button() { Text = "INSERTAR", Location = new Point(240, y), Width = 140, Height = 40, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = fontLabel, Cursor = Cursors.Hand };
            btnInsertar.FlatAppearance.BorderSize = 0;
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

            if (!decimal.TryParse(importeText.Replace(".", ","), out decimal importeTotal) &&
                !decimal.TryParse(importeText.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out importeTotal))
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
