namespace CinetCore
{
    partial class FormUsuariosMostazaERP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsuariosMostazaERP));
            btnVolver = new Button();
            lblVersion = new Label();
            dgvUsuarios = new DataGridView();
            txtBuscar = new TextBox();
            lblPagina = new Label();
            nudPagina = new NumericUpDown();
            pnlLoading = new Panel();
            lblLoading = new Label();
            progressBarLoading = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPagina).BeginInit();
            pnlLoading.SuspendLayout();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(37, 407);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 12;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Cursor = Cursors.Help;
            lblVersion.FlatStyle = FlatStyle.Popup;
            lblVersion.ForeColor = SystemColors.ControlText;
            lblVersion.Location = new Point(690, 415);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 13;
            lblVersion.Text = "label3";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(58, 41);
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.Size = new Size(679, 340);
            dgvUsuarios.TabIndex = 14;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(58, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por usuario, nombre o apellido";
            txtBuscar.Size = new Size(146, 23);
            txtBuscar.TabIndex = 15;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // lblPagina
            // 
            lblPagina.AutoSize = true;
            lblPagina.Location = new Point(352, 403);
            lblPagina.Name = "lblPagina";
            lblPagina.Size = new Size(43, 15);
            lblPagina.TabIndex = 20;
            lblPagina.Text = "Pagina";
            // 
            // nudPagina
            // 
            nudPagina.BorderStyle = BorderStyle.FixedSingle;
            nudPagina.Location = new Point(401, 401);
            nudPagina.Name = "nudPagina";
            nudPagina.Size = new Size(53, 23);
            nudPagina.TabIndex = 24;
            nudPagina.TextAlign = HorizontalAlignment.Center;
            nudPagina.ValueChanged += nudPagina_ValueChanged;
            // 
            // pnlLoading
            // 
            pnlLoading.BackColor = Color.White;
            pnlLoading.Controls.Add(progressBarLoading);
            pnlLoading.Controls.Add(lblLoading);
            pnlLoading.Dock = DockStyle.Fill;
            pnlLoading.Location = new Point(0, 0);
            pnlLoading.Name = "pnlLoading";
            pnlLoading.Size = new Size(800, 450);
            pnlLoading.TabIndex = 25;
            pnlLoading.Visible = false;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLoading.Location = new Point(290, 171);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(201, 30);
            lblLoading.TabIndex = 0;
            lblLoading.Text = "Cargando usuarios...";
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(267, 204);
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(250, 23);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 1;
            // 
            // FormUsuariosMostazaERP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlLoading);
            Controls.Add(nudPagina);
            Controls.Add(lblPagina);
            Controls.Add(txtBuscar);
            Controls.Add(dgvUsuarios);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormUsuariosMostazaERP";
            Text = "Usuarios Mostaza_ERP";
            Load += FormUsuariosMostazaERP_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPagina).EndInit();
            pnlLoading.ResumeLayout(false);
            pnlLoading.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private DataGridView dgvUsuarios;
        private TextBox txtBuscar;
        private Label lblPagina;
        private NumericUpDown nudPagina;
        private Panel pnlLoading;
        private ProgressBar progressBarLoading;
        private Label lblLoading;
    }
}