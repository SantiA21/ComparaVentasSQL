using CinetCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CinetCore.Services.Salvaventas;
using CinetCore.Infrastructure;

namespace CinetCore.Forms.Salvaventas
{
    public partial class FormMainSalvaventas : Form
    {
        private List<ResultGroup> _lastResultados;
        private string _lastEquipo;
        private bool _isBackoffice;
        private string _foundDbName;

        private string _ip;
        private string _password;

        private Label lblLocal;
        private Button btnDesconectar;

        private TextBox txtSucursal;
        private TextBox txtNumero;
        private ComboBox cmbTipo;
        private ComboBox cmbValCodigo;
        private TextBox txtHostname;

        private Button btnBuscar;
        private Button btnInsertarValMov;
        private Button btnReinsertar;

        private Label lblStatus;
        private Label lblEquipoEncontrado;
        private FlowLayoutPanel panelResultados;

        public FormMainSalvaventas(string ip, string password, string codLocal = "Desconocido")
        {
            _ip = ip;
            _password = password;
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
            lblLocal.Text = $"Estás conectado a: {codLocal}";
        }

        private void InitializeComponent()
        {
            this.Text = "Salvaventas - Búsqueda y Re-inserción";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            var panelTop = new Panel() { Dock = DockStyle.Top, Height = 230, BackColor = Color.FromArgb(245, 246, 248) };
            
            lblLocal = new Label() { Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 122, 204) };
            
            btnDesconectar = new Button() { Text = "Desconectar", Location = new Point(860, 15), Width = 100, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnDesconectar.FlatAppearance.BorderSize = 0;
            btnDesconectar.Click += BtnDesconectar_Click;

            var groupParams = new GroupBox() { Text = "Parámetros de Búsqueda", Location = new Point(20, 60), Size = new Size(940, 100), Font = new Font("Segoe UI Semibold", 9F), ForeColor = Color.FromArgb(64, 64, 64) };
            
            groupParams.Controls.Add(new Label() { Text = "Sucursal:", Location = new Point(20, 35), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            txtSucursal = new TextBox() { Location = new Point(85, 33), Width = 100, Font = new Font("Segoe UI", 9F) };
            groupParams.Controls.Add(txtSucursal);
            
            groupParams.Controls.Add(new Label() { Text = "Número:", Location = new Point(210, 35), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            txtNumero = new TextBox() { Location = new Point(270, 33), Width = 150, Font = new Font("Segoe UI", 9F) };
            groupParams.Controls.Add(txtNumero);

            groupParams.Controls.Add(new Label() { Text = "Hostname (Opcional):", Location = new Point(450, 35), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            txtHostname = new TextBox() { Location = new Point(585, 33), Width = 150, Font = new Font("Segoe UI", 9F) };
            groupParams.Controls.Add(txtHostname);

            groupParams.Controls.Add(new Label() { Text = "Tipo:", Location = new Point(20, 65), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            cmbTipo = new ComboBox() { Location = new Point(85, 63), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbTipo.Items.AddRange(new object[] { "FAB", "FAA" });
            cmbTipo.SelectedIndex = 0;
            groupParams.Controls.Add(cmbTipo);

            groupParams.Controls.Add(new Label() { Text = "Val Código:", Location = new Point(210, 65), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            cmbValCodigo = new ComboBox() { Location = new Point(285, 63), Width = 135, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cmbValCodigo.Items.AddRange(new object[] { "MERPAGO", "EFECTIVO", "HNC" });
            cmbValCodigo.SelectedIndex = 0;
            groupParams.Controls.Add(cmbValCodigo);

            panelTop.Controls.Add(lblLocal);
            panelTop.Controls.Add(btnDesconectar);
            panelTop.Controls.Add(groupParams);

            btnBuscar = new Button() { Text = "BUSCAR VENTA", Location = new Point(20, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += async (s, e) => await BtnBuscar_Click(s, e);
            
            btnInsertarValMov = new Button() { Text = "AGREGAR VAL_MOV", Location = new Point(180, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand, Enabled = false };
            btnInsertarValMov.FlatAppearance.BorderSize = 0;
            btnInsertarValMov.Click += async (s, e) => await BtnInsertarValMov_Click(s, e);

            btnReinsertar = new Button() { Text = "REINSERTAR VENTA", Location = new Point(340, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand, Enabled = false };
            btnReinsertar.FlatAppearance.BorderSize = 0;
            btnReinsertar.Click += async (s, e) => await BtnReinsertar_Click(s, e);

            lblStatus = new Label() { Location = new Point(510, 185), AutoSize = true, ForeColor = Color.FromArgb(100, 100, 100), Font = new Font("Segoe UI", 9.5F) };

            panelTop.Controls.Add(btnBuscar);
            panelTop.Controls.Add(btnInsertarValMov);
            panelTop.Controls.Add(btnReinsertar);
            panelTop.Controls.Add(lblStatus);

            var panelBottom = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            lblEquipoEncontrado = new Label() { Text = "", Location = new Point(20, 10), AutoSize = true, ForeColor = Color.FromArgb(40, 167, 69), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold) };
            
            panelResultados = new FlowLayoutPanel() { Location = new Point(20, 40), Size = new Size(940, 400), AutoScroll = true, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White };

            panelBottom.Controls.Add(lblEquipoEncontrado);
            panelBottom.Controls.Add(panelResultados);

            this.Controls.Add(panelBottom);
            this.Controls.Add(panelTop);
        }

        private async Task BtnBuscar_Click(object sender, EventArgs e)
        {
            string sucCodigo = txtSucursal.Text.Trim();
            string veneNumero = txtNumero.Text.Trim();
            string cbteeCodigo = cmbTipo.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(sucCodigo) || string.IsNullOrEmpty(veneNumero) || string.IsNullOrEmpty(cbteeCodigo))
            {
                CinetCore.Utils.Alert.Show("Por favor, complete todos los campos requeridos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetLoading(true, "Conectando a base de datos inicial...");
                lblEquipoEncontrado.Text = "";
                panelResultados.Controls.Clear();
                _lastResultados = null;
                _lastEquipo = null;
                _isBackoffice = false;
                _foundDbName = null;

                var dbService = new DatabaseService(_ip, _password);

                lblStatus.Text = "Validando existencia en backoffice...";
                var (existsBackoffice, messageBackoffice, isCentralized) = await dbService.ValidarVentaExistenteBackofficeAsync(sucCodigo, veneNumero, cbteeCodigo);

                if (existsBackoffice)
                {
                    lblStatus.Text = "Obteniendo registros de backoffice...";
                    var resultadosPrincipales = await dbService.SearchVentaPrincipalesBackofficeAsync(sucCodigo, veneNumero, cbteeCodigo);
                    
                    ShowResultados(resultadosPrincipales);
                    btnReinsertar.Enabled = false;

                    bool isMissingVal = !resultadosPrincipales.Any(r => r.TableName.Contains("Val Movimientos", StringComparison.OrdinalIgnoreCase));
                    btnInsertarValMov.Enabled = isMissingVal;
                    _isBackoffice = true;

                    if (isCentralized)
                        CinetCore.Utils.Alert.Show($"La venta ya existe en backoffice.\n{messageBackoffice}\n\n¡ATENCIÓN: centralizada!", "Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        CinetCore.Utils.Alert.Show($"La venta ya existe en backoffice.\n{messageBackoffice}", "Venta Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    lblStatus.Text = "Venta encontrada en backoffice.";
                    return;
                }

                string hostnameManual = txtHostname.Text.Trim();
                string equipo;

                if (!string.IsNullOrEmpty(hostnameManual))
                {
                    equipo = hostnameManual;
                    lblStatus.Text = $"Utilizando hostname manual: {equipo}...";
                }
                else
                {
                    lblStatus.Text = "Buscando equipo responsable...";
                    equipo = await dbService.FindEquipoAsync(sucCodigo, veneNumero, cbteeCodigo);

                    if (string.IsNullOrEmpty(equipo))
                    {
                        CinetCore.Utils.Alert.Show("No se encontró ningún equipo.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblStatus.Text = "Búsqueda finalizada sin resultados.";
                        return;
                    }
                }

                lblStatus.Text = $"Preparando conexión al equipo {equipo}...";
                await dbService.EnsureLinkedServerAsync(equipo);

                lblStatus.Text = $"Validando existencia en el equipo {equipo}...";
                var (exists, message, foundDb) = await dbService.CheckVentaExistenteGlobalAsync(equipo, sucCodigo, veneNumero, cbteeCodigo);

                if (exists)
                {
                    lblStatus.Text = "Obteniendo registros...";
                    var resultadosPrincipales = await dbService.SearchVentaPrincipalesAsync(equipo, sucCodigo, veneNumero, cbteeCodigo);
                    
                    ShowResultados(resultadosPrincipales);
                    btnReinsertar.Enabled = false; 

                    bool isMissingVal = !resultadosPrincipales.Any(r => r.TableName.Contains("Val Movimientos", StringComparison.OrdinalIgnoreCase));
                    btnInsertarValMov.Enabled = isMissingVal;
                    _isBackoffice = false;
                    _foundDbName = foundDb;
                    _lastEquipo = equipo;

                    CinetCore.Utils.Alert.Show($"La venta ya existe en ({foundDb}).\n{message}", "Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "La venta ya existe.";
                    return;
                }

                CinetCore.Utils.Alert.Show("Búsqueda en tablas de rescate.", "Rescate", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblEquipoEncontrado.Text = $"Equipo: {equipo}";
                lblStatus.Text = $"Buscando en Linked Server ({equipo})...";

                var resultados = await dbService.SearchVentaInLinkedServerAsync(equipo, sucCodigo, veneNumero, cbteeCodigo);

                if (resultados.Count > 0)
                {
                    _lastResultados = resultados;
                    _lastEquipo = equipo;
                    btnReinsertar.Enabled = true;
                    ShowResultados(resultados);
                    lblStatus.Text = "Búsqueda completada exitosamente.";
                }
                else
                {
                    lblStatus.Text = "No se encontraron registros.";
                    string valCodigo = cmbValCodigo.SelectedItem?.ToString() ?? "EFECTIVO";

                    if (CinetCore.Utils.Alert.Show($"La venta no existe. ¿Desea insertarla manualmente con Val Código: {valCodigo}?", "Insertar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var window = new FormInsertarVenta(_ip, _password, sucCodigo, veneNumero, cbteeCodigo, valCodigo);
                        window.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnBuscar_Click", ex);
                CinetCore.Utils.Alert.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error.";
            }
            finally
            {
                SetLoading(false, lblStatus.Text);
            }
        }

        private void ShowResultados(List<ResultGroup> resultados)
        {
            panelResultados.Controls.Clear();
            foreach (var r in resultados)
            {
                var lbl = new Label() { Text = r.TableName.ToUpper(), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 15, 0, 5), ForeColor = Color.FromArgb(0, 122, 204) };
                var dgv = new DataGridView() { 
                    DataSource = r.Data, 
                    ReadOnly = true, 
                    AllowUserToAddRows = false, 
                    Width = 910, 
                    Height = 160,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    EnableHeadersVisualStyles = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 248);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 248);
                dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
                
                panelResultados.Controls.Add(lbl);
                panelResultados.Controls.Add(dgv);
            }
        }

        private async Task BtnReinsertar_Click(object sender, EventArgs e)
        {
            if (_lastResultados == null || _lastResultados.Count == 0 || string.IsNullOrEmpty(_lastEquipo))
                return;

            string sucCodigo = txtSucursal.Text.Trim();
            string veneNumero = txtNumero.Text.Trim();
            string cbteeCodigo = cmbTipo.SelectedItem?.ToString() ?? "";
            string valCodigo = cmbValCodigo.SelectedItem?.ToString();

            try
            {
                SetLoading(true, "Reinsertando venta...");
                var dbService = new DatabaseService(_ip, _password);
                await dbService.InsertarVentasRescatadasAsync(_lastEquipo, _lastResultados, sucCodigo, veneNumero, cbteeCodigo, valCodigo);
                CinetCore.Utils.Alert.Show("Las ventas se reinsertaron correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Re-inserción completada.";
                btnReinsertar.Enabled = false; 
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnReinsertar", ex);
                CinetCore.Utils.Alert.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error en re-inserción.";
                btnReinsertar.Enabled = true;
            }
            finally
            {
                SetLoading(false, lblStatus.Text);
            }
        }

        private async Task BtnInsertarValMov_Click(object sender, EventArgs e)
        {
            string sucCodigo = txtSucursal.Text.Trim();
            string veneNumero = txtNumero.Text.Trim();
            string cbteeCodigo = cmbTipo.SelectedItem?.ToString() ?? "";
            string valCodigo = cmbValCodigo.SelectedItem?.ToString() ?? "EFECTIVO";

            try
            {
                SetLoading(true, "Insertando Val_Movimientos...");
                var dbService = new DatabaseService(_ip, _password);
                await dbService.InsertarValMovimientosFaltanteAsync(_isBackoffice, _lastEquipo, _foundDbName, sucCodigo, veneNumero, cbteeCodigo, valCodigo);
                CinetCore.Utils.Alert.Show("Val_Movimientos insertado correctamente. Vuelva a buscar.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Inserción completada.";
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnInsertarValMov", ex);
                CinetCore.Utils.Alert.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error.";
                btnInsertarValMov.Enabled = true;
            }
            finally
            {
                SetLoading(false, lblStatus.Text);
            }
        }

        private void SetLoading(bool isLoading, string msg)
        {
            btnBuscar.Enabled = !isLoading;
            if (isLoading)
            {
                btnReinsertar.Enabled = false;
                btnInsertarValMov.Enabled = false;
            }
            lblStatus.Text = msg;
        }

        private void BtnDesconectar_Click(object sender, EventArgs e)
        {
            var formConexion = new FormConexionSalvaventas();
            formConexion.Show();
            this.Close();
        }
    }
}
