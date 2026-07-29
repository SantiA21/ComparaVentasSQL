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
        private Button btnVerCajas;
        private FormVerSucursalesV2 _formSucursales;

        private Label lblStatus;
        private Label lblEquipoEncontrado;
        private FlowLayoutPanel panelResultados;
        private Panel panelTop;

        private bool _modoLote;
        private List<VentaRescueRequest> _listaLote = new List<VentaRescueRequest>();
        private DataGridView dgvLote;
        private GroupBox groupParams;
        private GroupBox groupLote;
        private Button btnBuscarLote;
        private Button btnReinsertarLote;
        private Button btnAgregarFilaLote;
        private Button btnQuitarFilaLote;
        private Button btnPegarListaLote;
        private Button btnInsertarManualLote;
        private RadioButton rdoIndividual;
        private RadioButton rdoLote;

        public FormMainSalvaventas(string ip, string password, string codLocal = "Desconocido", List<VentaRescueRequest> ventasLote = null, bool modoLote = false)
        {
            _ip = ip;
            _password = password;
            _modoLote = modoLote;
            if (ventasLote != null && ventasLote.Count > 0)
            {
                _listaLote = ventasLote;
                _modoLote = true;
            }
            InitializeComponent();
            CinetCore.Utils.UIHelper.ApplyModernTheme(this);
            lblLocal.Text = $"Estás conectado a: {codLocal}";
            ActualizarModoUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Salvaventas - Búsqueda y Re-inserción";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            panelTop = new Panel() { Dock = DockStyle.Top, Height = 230, BackColor = Color.FromArgb(245, 246, 248) };
            
            lblLocal = new Label() { Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(0, 122, 204) };
            
            btnDesconectar = new Button() { Text = "Desconectar", Location = new Point(860, 15), Width = 100, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnDesconectar.FlatAppearance.BorderSize = 0;
            btnDesconectar.Click += BtnDesconectar_Click;

            rdoIndividual = new RadioButton() { Text = "Venta Individual", Location = new Point(320, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F), Checked = !_modoLote };
            rdoLote = new RadioButton() { Text = "Múltiples Ventas (Lote)", Location = new Point(470, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.5F), Checked = _modoLote };
            rdoIndividual.CheckedChanged += (s, e) => { if (rdoIndividual.Checked) { _modoLote = false; ActualizarModoUI(); } };
            rdoLote.CheckedChanged += (s, e) => { if (rdoLote.Checked) { _modoLote = true; ActualizarModoUI(); } };
            panelTop.Controls.Add(rdoIndividual);
            panelTop.Controls.Add(rdoLote);

            groupParams = new GroupBox() { Text = "Parámetros de Búsqueda", Location = new Point(20, 50), Size = new Size(940, 115), Font = new Font("Segoe UI Semibold", 9F), ForeColor = Color.FromArgb(64, 64, 64) };
            
            groupParams.Controls.Add(new Label() { Text = "Sucursal:", Location = new Point(20, 35), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            txtSucursal = new TextBox() { Location = new Point(85, 33), Width = 100, Font = new Font("Segoe UI", 9F) };
            txtSucursal.Leave += (s, e) => { txtSucursal.Text = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(txtSucursal.Text); };
            groupParams.Controls.Add(txtSucursal);
            
            groupParams.Controls.Add(new Label() { Text = "Número:", Location = new Point(210, 35), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.Black });
            txtNumero = new TextBox() { Location = new Point(270, 33), Width = 150, Font = new Font("Segoe UI", 9F) };
            txtNumero.Leave += (s, e) => { txtNumero.Text = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(txtNumero.Text); };
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

            groupLote = new GroupBox() { Text = "Acciones de Lote (Lista de Ventas)", Location = new Point(20, 50), Size = new Size(940, 75), Font = new Font("Segoe UI Semibold", 9F), ForeColor = Color.FromArgb(64, 64, 64) };
            
            btnAgregarFilaLote = new Button() { Text = "+ Agregar Fila", Location = new Point(15, 25), Width = 110, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnAgregarFilaLote.FlatAppearance.BorderSize = 0;
            btnAgregarFilaLote.Click += BtnAgregarFilaLote_Click;

            btnQuitarFilaLote = new Button() { Text = "- Quitar Fila", Location = new Point(135, 25), Width = 110, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnQuitarFilaLote.FlatAppearance.BorderSize = 0;
            btnQuitarFilaLote.Click += BtnQuitarFilaLote_Click;

            btnPegarListaLote = new Button() { Text = "📋 Pegar Lista", Location = new Point(255, 25), Width = 120, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(23, 162, 184), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnPegarListaLote.FlatAppearance.BorderSize = 0;
            btnPegarListaLote.Click += BtnPegarListaLote_Click;

            btnBuscarLote = new Button() { Text = "🔍 BUSCAR EN LOTE", Location = new Point(385, 25), Width = 145, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnBuscarLote.FlatAppearance.BorderSize = 0;
            btnBuscarLote.Click += async (s, e) => await BtnBuscarLote_Click(s, e);

            btnReinsertarLote = new Button() { Text = "🚀 REINSERTAR LOTE", Location = new Point(540, 25), Width = 175, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand, Enabled = false };
            btnReinsertarLote.FlatAppearance.BorderSize = 0;
            btnReinsertarLote.Click += async (s, e) => await BtnReinsertarLote_Click(s, e);

            btnInsertarManualLote = new Button() { Text = "📝 INSERTAR MANUAL", Location = new Point(725, 25), Width = 195, Height = 35, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(253, 126, 20), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnInsertarManualLote.FlatAppearance.BorderSize = 0;
            btnInsertarManualLote.Click += BtnInsertarManualLote_Click;

            groupLote.Controls.Add(btnAgregarFilaLote);
            groupLote.Controls.Add(btnQuitarFilaLote);
            groupLote.Controls.Add(btnPegarListaLote);
            groupLote.Controls.Add(btnBuscarLote);
            groupLote.Controls.Add(btnReinsertarLote);
            groupLote.Controls.Add(btnInsertarManualLote);
            panelTop.Controls.Add(groupLote);

            btnBuscar = new Button() { Text = "BUSCAR VENTA", Location = new Point(20, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += async (s, e) => await BtnBuscar_Click(s, e);
            
            btnInsertarValMov = new Button() { Text = "AGREGAR VAL_MOV", Location = new Point(180, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand, Enabled = false };
            btnInsertarValMov.FlatAppearance.BorderSize = 0;
            btnInsertarValMov.Click += async (s, e) => await BtnInsertarValMov_Click(s, e);

            btnReinsertar = new Button() { Text = "REINSERTAR VENTA", Location = new Point(340, 175), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand, Enabled = false };
            btnReinsertar.FlatAppearance.BorderSize = 0;
            btnReinsertar.Click += async (s, e) => await BtnReinsertar_Click(s, e);

            btnVerCajas = new Button() { Text = "VER CAJAS", Location = new Point(500, 175), Width = 130, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(23, 162, 184), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9F), Cursor = Cursors.Hand };
            btnVerCajas.FlatAppearance.BorderSize = 0;
            btnVerCajas.Click += BtnVerCajas_Click;

            lblStatus = new Label() { Location = new Point(645, 185), AutoSize = true, ForeColor = Color.FromArgb(100, 100, 100), Font = new Font("Segoe UI", 9.5F) };

            panelTop.Controls.Add(btnBuscar);
            panelTop.Controls.Add(btnInsertarValMov);
            panelTop.Controls.Add(btnReinsertar);
            panelTop.Controls.Add(btnVerCajas);
            panelTop.Controls.Add(lblStatus);

            var panelBottom = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            
            lblEquipoEncontrado = new Label() { Text = "", Location = new Point(20, 10), AutoSize = true, ForeColor = Color.FromArgb(40, 167, 69), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold) };
            
            panelResultados = new FlowLayoutPanel() { Location = new Point(20, 40), Size = new Size(940, 400), AutoScroll = true, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right, BackColor = Color.White, FlowDirection = FlowDirection.TopDown, WrapContents = false };

            dgvLote = new DataGridView() {
                Location = new Point(20, 15),
                Size = new Size(940, 420),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeight = 38,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = true,
                RowTemplate = { Height = 32 },
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 9.5F)
            };
            dgvLote.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 108, 176);
            dgvLote.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLote.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dgvLote.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLote.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 252);
            dgvLote.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 230, 255);
            dgvLote.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLote.CellDoubleClick += DgvLote_CellDoubleClick;
            dgvLote.CellFormatting += (s, e) => {
                if (dgvLote.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
                {
                    string val = e.Value.ToString();
                    if (val.Contains("RESCATADA") || val.Contains("Reinsertada"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(40, 167, 69); // Verde éxito
                        e.CellStyle.Font = new Font(dgvLote.Font, FontStyle.Bold);
                    }
                    else if (val.Contains("Rescatable") || val.Contains("RESCATABLE") || val.Contains("🚀"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(0, 122, 204); // Azul
                        e.CellStyle.Font = new Font(dgvLote.Font, FontStyle.Bold);
                    }
                    else if (val.Contains("Error") || val.Contains("✖") || val.Contains("No encontrada"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(220, 53, 69); // Rojo
                        e.CellStyle.Font = new Font(dgvLote.Font, FontStyle.Bold);
                    }
                    else if (val.Contains("Ya existe") || val.Contains("✔"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(40, 110, 60); // Verde normal
                        e.CellStyle.Font = new Font(dgvLote.Font, FontStyle.Bold);
                    }
                }
            };

            panelBottom.Controls.Add(lblEquipoEncontrado);
            panelBottom.Controls.Add(panelResultados);
            panelBottom.Controls.Add(dgvLote);

            this.Controls.Add(panelBottom);
            this.Controls.Add(panelTop);
        }

        private async Task BtnBuscar_Click(object sender, EventArgs e)
        {
            txtSucursal.Text = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(txtSucursal.Text);
            txtNumero.Text = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(txtNumero.Text);

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
                    string tablasEncontradas = string.Join(", ", resultados.Select(r => r.TableName));
                    lblStatus.Text = $"[🚀] RESCATABLE de: {tablasEncontradas}";
                    lblStatus.ForeColor = Color.FromArgb(0, 122, 204);
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
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
                };
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 248);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 248);
                dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
                
                panelResultados.Controls.Add(lbl);
                panelResultados.SetFlowBreak(lbl, true);
                panelResultados.Controls.Add(dgv);
                panelResultados.SetFlowBreak(dgv, true);
            }
        }

        private async Task BtnReinsertar_Click(object sender, EventArgs e)
        {
            if (_lastResultados == null || _lastResultados.Count == 0 || string.IsNullOrEmpty(_lastEquipo))
                return;

            txtSucursal.Text = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(txtSucursal.Text);
            txtNumero.Text = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(txtNumero.Text);

            string sucCodigo = txtSucursal.Text.Trim();
            string veneNumero = txtNumero.Text.Trim();
            string cbteeCodigo = cmbTipo.SelectedItem?.ToString() ?? "";
            string valCodigo = cmbValCodigo.SelectedItem?.ToString();

            try
            {
                SetLoading(true, "Reinsertando venta...");
                var dbService = new DatabaseService(_ip, _password);
                await dbService.InsertarVentasRescatadasAsync(_lastEquipo, _lastResultados, sucCodigo, veneNumero, cbteeCodigo, valCodigo);
                string tablasRescate = _lastResultados != null ? string.Join(", ", _lastResultados.Select(r => r.TableName)) : "N/A";
                CinetCore.Utils.Alert.Show(
                    $"✅ [✔] VENTA RESCATADA CON ÉXITO\n\nLa venta {sucCodigo}-{veneNumero} ({cbteeCodigo}) se salvó y reinsertó en el equipo {_lastEquipo}.\n\n" +
                    $"📋 RESCATADA de tabla(s):\n• {string.Join("\n• ", _lastResultados?.Select(r => r.TableName) ?? new List<string>())}",
                    "Venta RESCATADA con Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                lblStatus.Text = $"[✔] RESCATADA de: {tablasRescate}";
                lblStatus.ForeColor = Color.FromArgb(40, 167, 69);
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
            txtSucursal.Text = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(txtSucursal.Text);
            txtNumero.Text = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(txtNumero.Text);

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
            btnVerCajas.Enabled = !isLoading;
            if (btnBuscarLote != null) btnBuscarLote.Enabled = !isLoading;
            if (btnInsertarManualLote != null) btnInsertarManualLote.Enabled = !isLoading;
            if (btnAgregarFilaLote != null) btnAgregarFilaLote.Enabled = !isLoading;
            if (btnQuitarFilaLote != null) btnQuitarFilaLote.Enabled = !isLoading;
            if (btnPegarListaLote != null) btnPegarListaLote.Enabled = !isLoading;
            if (rdoIndividual != null) rdoIndividual.Enabled = !isLoading;
            if (rdoLote != null) rdoLote.Enabled = !isLoading;
            if (dgvLote != null) dgvLote.Enabled = !isLoading;

            if (isLoading)
            {
                btnReinsertar.Enabled = false;
                btnInsertarValMov.Enabled = false;
                if (btnReinsertarLote != null) btnReinsertarLote.Enabled = false;
            }
            lblStatus.Text = msg;
        }

        private void BtnDesconectar_Click(object sender, EventArgs e)
        {
            var formConexion = new FormConexionSalvaventas();
            formConexion.Show();
            this.Close();
        }

        private string GetMotherServerConnectionString()
        {
            string server = _ip.Contains(",") ? _ip : $"{_ip},1433";
            return $"Server={server};Database=backoffice;User Id=sa;Password={_password};TrustServerCertificate=True;";
        }

        private void BtnVerCajas_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_ip))
            {
                CinetCore.Utils.Alert.Show("Debe cargar primero el servidor.");
                return;
            }

            if (_formSucursales == null || _formSucursales.IsDisposed)
            {
                string connectionString = GetMotherServerConnectionString();
                _formSucursales = new FormVerSucursalesV2(connectionString);
                _formSucursales.Show();
            }
            else
            {
                _formSucursales.BringToFront();
            }
        }

        private void ActualizarModoUI()
        {
            if (groupParams == null || groupLote == null) return;

            groupParams.Visible = !_modoLote;
            btnBuscar.Visible = !_modoLote;
            btnInsertarValMov.Visible = !_modoLote;
            btnReinsertar.Visible = !_modoLote;
            btnVerCajas.Visible = !_modoLote;

            groupLote.Visible = _modoLote;

            if (_modoLote)
            {
                panelTop.Height = 140;
                lblStatus.Location = new Point(25, 125);
                lblStatus.BringToFront();

                panelResultados.Visible = false;
                lblEquipoEncontrado.Visible = false;
                dgvLote.Visible = true;
                dgvLote.BringToFront();
                RefrescarGrillaLote();
            }
            else
            {
                panelTop.Height = 230;
                lblStatus.Location = new Point(645, 185);

                panelResultados.Visible = true;
                lblEquipoEncontrado.Visible = true;
                dgvLote.Visible = false;
            }
        }

        private void RefrescarGrillaLote()
        {
            dgvLote.DataSource = null;
            dgvLote.DataSource = _listaLote;
            if (dgvLote.Columns.Contains("ResultadosRescate"))
                dgvLote.Columns["ResultadosRescate"].Visible = false;
            if (dgvLote.Columns.Contains("YaExiste"))
                dgvLote.Columns["YaExiste"].Visible = false;
            if (dgvLote.Columns.Contains("Rescatable"))
                dgvLote.Columns["Rescatable"].Visible = false;
            dgvLote.Refresh();
        }

        private void BtnAgregarFilaLote_Click(object sender, EventArgs e)
        {
            _listaLote.Add(new VentaRescueRequest { SucCodigo = "0001", VeneNumero = "", CbteeCodigo = "FAB", ValCodigo = "EFECTIVO" });
            RefrescarGrillaLote();
        }

        private void BtnQuitarFilaLote_Click(object sender, EventArgs e)
        {
            if (dgvLote.CurrentRow != null && dgvLote.CurrentRow.Index >= 0 && dgvLote.CurrentRow.Index < _listaLote.Count)
            {
                _listaLote.RemoveAt(dgvLote.CurrentRow.Index);
                RefrescarGrillaLote();
            }
        }

        private void BtnPegarListaLote_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(texto))
                {
                    CinetCore.Utils.Alert.Show("El portapapeles está vacío.");
                    return;
                }

                var lineas = texto.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int agregadas = 0;
                foreach (var linea in lineas)
                {
                    var partes = linea.Split(new[] { ',', '\t', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (partes.Length >= 2)
                    {
                        _listaLote.Add(new VentaRescueRequest
                        {
                            SucCodigo = partes[0].Trim(),
                            VeneNumero = partes[1].Trim(),
                            CbteeCodigo = partes.Length >= 3 ? partes[2].Trim() : "FAB",
                            ValCodigo = partes.Length >= 4 ? partes[3].Trim() : "EFECTIVO"
                        });
                        agregadas++;
                    }
                }
                RefrescarGrillaLote();
                CinetCore.Utils.Alert.Show($"Se pegaron {agregadas} ventas desde el portapapeles.");
            }
            catch (Exception ex)
            {
                CinetCore.Utils.Alert.Show("Error al pegar desde el portapapeles: " + ex.Message);
            }
        }

        private void BtnInsertarManualLote_Click(object sender, EventArgs e)
        {
            VentaRescueRequest obj = null;
            if (dgvLote.CurrentRow != null && dgvLote.CurrentRow.Index >= 0 && dgvLote.CurrentRow.Index < _listaLote.Count)
            {
                obj = _listaLote[dgvLote.CurrentRow.Index];
            }
            else
            {
                obj = _listaLote.FirstOrDefault(x => !x.YaExiste && !x.Rescatable) ?? _listaLote.FirstOrDefault();
            }

            if (obj == null)
            {
                CinetCore.Utils.Alert.Show("No hay ventas en la lista para insertar manualmente.");
                return;
            }

            AbrirInsertarManualLote(obj);
        }

        private void DgvLote_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _listaLote.Count)
            {
                var obj = _listaLote[e.RowIndex];
                if (CinetCore.Utils.Alert.Show($"¿Desea abrir el cargador manual para la venta {obj.SucCodigo}-{obj.VeneNumero}?", "Insertar Manualmente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    AbrirInsertarManualLote(obj);
                }
            }
        }

        private void AbrirInsertarManualLote(VentaRescueRequest obj)
        {
            try
            {
                var window = new FormInsertarVenta(_ip, _password, obj.SucCodigo, obj.VeneNumero, obj.CbteeCodigo, obj.ValCodigo, obj.Importe, obj.CAE, obj.Fecha);
                window.FormClosed += (s, args) =>
                {
                    if (window.DialogResult == DialogResult.OK)
                    {
                        obj.YaExiste = true;
                        obj.Rescatable = false;
                        obj.Estado = "[✔] Insertada en Backoffice";
                        RefrescarGrillaLote();
                    }
                };
                window.Show();
            }
            catch (Exception ex)
            {
                CinetCore.Utils.Alert.Show($"Error al abrir cargador manual: {ex.Message}");
            }
        }

        private async Task BtnBuscarLote_Click(object sender, EventArgs e)
        {
            if (_listaLote == null || _listaLote.Count == 0)
            {
                CinetCore.Utils.Alert.Show("Agregue o pegue al menos una venta en la lista.");
                return;
            }

            try
            {
                SetLoading(true, "Buscando en lote en servidores remotos...");
                var dbService = new DatabaseService(_ip, _password);

                foreach (var item in _listaLote)
                {
                    item.SucCodigo = CinetCore.Utils.IdUnicoParser.NormalizarSucursal(item.SucCodigo);
                    item.VeneNumero = CinetCore.Utils.IdUnicoParser.NormalizarComprobante(item.VeneNumero);

                    if (string.IsNullOrWhiteSpace(item.SucCodigo) || string.IsNullOrWhiteSpace(item.VeneNumero))
                    {
                        item.Estado = "[✖] Sucursal o Número inválido";
                        continue;
                    }

                    try
                    {
                        item.Estado = "Validando en Backoffice...";
                        RefrescarGrillaLote();

                        var boCheck = await dbService.ValidarVentaExistenteBackofficeAsync(item.SucCodigo, item.VeneNumero, item.CbteeCodigo);
                        if (boCheck.Exists)
                        {
                            item.YaExiste = true;
                            item.Rescatable = false;
                            item.Estado = "[✔] " + boCheck.Message;
                            item.Equipo = "BACKOFFICE";
                            continue;
                        }

                        item.Estado = "Detectando equipo...";
                        RefrescarGrillaLote();

                        string equipo = await dbService.FindEquipoAsync(item.SucCodigo, item.VeneNumero, item.CbteeCodigo);
                        if (string.IsNullOrEmpty(equipo))
                        {
                            item.Estado = "[✖] No encontrado (Sin equipo)";
                            item.Equipo = "";
                            continue;
                        }
                        item.Equipo = equipo;
                    }
                    catch (Exception exItem)
                    {
                        Logger.Error($"Error en verificación inicial de {item.SucCodigo}-{item.VeneNumero}", exItem);
                        item.Estado = $"[✖] Error: {exItem.Message}";
                    }
                }

                var porEquipo = _listaLote.Where(x => !x.YaExiste && !string.IsNullOrEmpty(x.Equipo)).GroupBy(x => x.Equipo);

                foreach (var grupoEquipo in porEquipo)
                {
                    string equipo = grupoEquipo.Key;
                    try
                    {
                        lblStatus.Text = $"Conectando al equipo {equipo}...";
                        await dbService.EnsureLinkedServerAsync(equipo);
                    }
                    catch (Exception exEquipo)
                    {
                        Logger.Error($"Error conectando al equipo {equipo}", exEquipo);
                        foreach (var item in grupoEquipo)
                        {
                            item.Estado = $"[✖] Error de conexión al equipo {equipo}";
                        }
                        continue;
                    }

                    foreach (var item in grupoEquipo)
                    {
                        try
                        {
                            item.Estado = $"Validando en {equipo}...";
                            RefrescarGrillaLote();

                            var (exists, message, foundDb) = await dbService.CheckVentaExistenteGlobalAsync(equipo, item.SucCodigo, item.VeneNumero, item.CbteeCodigo);
                            if (exists)
                            {
                                item.YaExiste = true;
                                item.Rescatable = false;
                                item.Estado = $"[✔] Ya existe ({foundDb})";
                                continue;
                            }

                            var resultados = await dbService.SearchVentaInLinkedServerAsync(equipo, item.SucCodigo, item.VeneNumero, item.CbteeCodigo);
                            if (resultados != null && resultados.Count > 0)
                            {
                                item.YaExiste = false;
                                item.Rescatable = true;
                                item.ResultadosRescate = resultados;
                                string tablasEncontradas = string.Join(", ", resultados.Select(r => r.TableName));
                                item.Estado = $"[🚀] Rescatable de: {tablasEncontradas}";
                            }
                            else
                            {
                                item.YaExiste = false;
                                item.Rescatable = false;
                                item.Estado = "[✖] No encontrada en servidor";
                            }
                        }
                        catch (Exception exVenta)
                        {
                            Logger.Error($"Error en búsqueda remota de {item.SucCodigo}-{item.VeneNumero}", exVenta);
                            item.Estado = $"[✖] Error remoto: {exVenta.Message}";
                        }
                    }
                }

                RefrescarGrillaLote();
                var existentes = _listaLote.Where(x => x.YaExiste).Select(x => $"{x.SucCodigo}-{x.VeneNumero}" + (string.IsNullOrEmpty(x.Equipo) ? " (Backoffice)" : $" ({x.Equipo})")).ToList();
                var listRescatables = _listaLote.Where(x => x.Rescatable).Select(x => $"{x.SucCodigo}-{x.VeneNumero} [Equipo: {x.Equipo} | Encontrada en: {(x.ResultadosRescate != null ? string.Join(", ", x.ResultadosRescate.Select(r => r.TableName)) : "N/A")}]").ToList();
                var faltantes = _listaLote.Where(x => !x.YaExiste && !x.Rescatable).Select(x => $"{x.SucCodigo}-{x.VeneNumero}" + (string.IsNullOrEmpty(x.Equipo) ? "" : $" ({x.Equipo})")).ToList();

                int rescatables = listRescatables.Count;
                int noEncontradas = faltantes.Count;
                btnReinsertarLote.Enabled = (rescatables > 0);
                lblStatus.Text = $"Búsqueda en lote finalizada. ({rescatables} rescatables de {_listaLote.Count})";

                string FormatearLista(List<string> lista, int max = 10)
                {
                    if (lista.Count == 0) return "";
                    var mostradas = lista.Take(max).ToList();
                    string res = "• " + string.Join("\n• ", mostradas);
                    if (lista.Count > max)
                        res += $"\n• ... y {lista.Count - max} más";
                    return res;
                }

                string resumen = $"Resumen de búsqueda en lote ({_listaLote.Count} ventas):\n\n";

                if (existentes.Count > 0)
                {
                    resumen += $"✔ YA EXISTEN ({existentes.Count}):\n" + FormatearLista(existentes) + "\n\n";
                }
                if (listRescatables.Count > 0)
                {
                    resumen += $"🚀 RESCATABLES ({listRescatables.Count}):\n" + FormatearLista(listRescatables) + "\n\n";
                }
                if (faltantes.Count > 0)
                {
                    resumen += $"✖ NO EXISTEN / FALTANTES ({faltantes.Count}):\n" + FormatearLista(faltantes) + "\n\n";
                }

                if (rescatables == 0 && noEncontradas > 0)
                {
                    resumen += "¿Desea abrir el formulario de Inserción Manual para cargar las ventas faltantes?";
                    var resp = CinetCore.Utils.Alert.Show(
                        resumen.TrimEnd(),
                        "0 Rescatables - ¿Insertar Manualmente?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        var primeraNoEncontrada = _listaLote.First(x => !x.YaExiste && !x.Rescatable);
                        AbrirInsertarManualLote(primeraNoEncontrada);
                    }
                }
                else
                {
                    resumen += "Puede reinsertar las rescatables presionando '🚀 REINSERTAR LOTE' o usar '📝 INSERTAR MANUAL' para las faltantes.";
                    CinetCore.Utils.Alert.Show(
                        resumen.TrimEnd(),
                        "Resultado de Búsqueda en Lote",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnBuscarLote_Click", ex);
                CinetCore.Utils.Alert.Show($"Error en búsqueda por lote: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error en búsqueda por lote.";
            }
            finally
            {
                SetLoading(false, lblStatus.Text);
            }
        }

        private async Task BtnReinsertarLote_Click(object sender, EventArgs e)
        {
            var rescatables = _listaLote.Where(x => x.Rescatable && x.ResultadosRescate != null && x.ResultadosRescate.Count > 0).ToList();
            if (rescatables.Count == 0)
            {
                CinetCore.Utils.Alert.Show("No hay ventas rescatables en el lote.");
                return;
            }

            try
            {
                SetLoading(true, $"Reinsertando {rescatables.Count} ventas en lote...");
                var dbService = new DatabaseService(_ip, _password);
                var listExitosas = new List<string>();
                var listFallidas = new List<string>();

                foreach (var item in rescatables)
                {
                    try
                    {
                        item.Estado = "Reinsertando...";
                        RefrescarGrillaLote();

                        await dbService.InsertarVentasRescatadasAsync(item.Equipo, item.ResultadosRescate, item.SucCodigo, item.VeneNumero, item.CbteeCodigo, item.ValCodigo);
                        string tablasSalvadas = item.ResultadosRescate != null ? string.Join(", ", item.ResultadosRescate.Select(r => r.TableName)) : "N/A";
                        item.Estado = $"[✔] RESCATADA de: {tablasSalvadas}";
                        item.Rescatable = false;
                        listExitosas.Add($"{item.SucCodigo}-{item.VeneNumero} [✔ RESCATADA de: {tablasSalvadas} | Equipo: {item.Equipo}]");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error al reinsertar en lote {item.SucCodigo}-{item.VeneNumero}", ex);
                        item.Estado = $"[✖] Error: {ex.Message}";
                        listFallidas.Add($"{item.SucCodigo}-{item.VeneNumero}: {ex.Message}");
                    }
                    RefrescarGrillaLote();
                }

                btnReinsertarLote.Enabled = false;
                lblStatus.Text = $"Reinserción en lote completada. ({listExitosas.Count}/{rescatables.Count})";

                string mensajeAlert = $"Reinserción en Lote finalizada ({listExitosas.Count} de {rescatables.Count}).\n\n";
                if (listExitosas.Count > 0)
                {
                    mensajeAlert += $"✅ Ventas reinsertadas exitosamente:\n• " + string.Join("\n• ", listExitosas) + "\n\n";
                }
                if (listFallidas.Count > 0)
                {
                    mensajeAlert += $"❌ Ventas con error:\n• " + string.Join("\n• ", listFallidas);
                }

                CinetCore.Utils.Alert.Show(mensajeAlert.TrimEnd(), "Detalle de Reinserción en Lote", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Error("Error en BtnReinsertarLote_Click", ex);
                CinetCore.Utils.Alert.Show($"Error en reinserción de lote: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoading(false, lblStatus.Text);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_formSucursales != null && !_formSucursales.IsDisposed)
            {
                _formSucursales.Close();
            }
            base.OnFormClosed(e);
        }
    }
}
