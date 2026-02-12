namespace CinetCore.Forms.Precios
{
    partial class FormPreciosGmg_ERP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreciosGmg_ERP));
            btnVolver = new Button();
            lblVersion = new Label();
            label2 = new Label();
            label1 = new Label();
            btnBuscar = new Button();
            dgvPrecios = new DataGridView();
            txtArticulo = new TextBox();
            txtLista = new TextBox();
            progressBarLoading = new ProgressBar();
            lblLoading = new Label();
            pnlLoading = new Panel();
            nudPagina = new NumericUpDown();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).BeginInit();
            pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPagina).BeginInit();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(20, 366);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 20;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Cursor = Cursors.Help;
            lblVersion.Location = new Point(472, 366);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 19;
            lblVersion.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(202, 15);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 18;
            label2.Text = "Cod. Articulo:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 15);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 17;
            label1.Text = "Lista precio:";
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(43, 108, 176);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = SystemColors.Window;
            btnBuscar.Location = new Point(422, 15);
            btnBuscar.Margin = new Padding(4, 3, 4, 3);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(88, 27);
            btnBuscar.TabIndex = 16;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // dgvPrecios
            // 
            dgvPrecios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrecios.Location = new Point(20, 60);
            dgvPrecios.Name = "dgvPrecios";
            dgvPrecios.Size = new Size(533, 296);
            dgvPrecios.TabIndex = 15;
            // 
            // txtArticulo
            // 
            txtArticulo.Location = new Point(288, 15);
            txtArticulo.Name = "txtArticulo";
            txtArticulo.Size = new Size(100, 23);
            txtArticulo.TabIndex = 14;
            txtArticulo.TextChanged += txtFiltros_TextChanged;
            // 
            // txtLista
            // 
            txtLista.Location = new Point(96, 12);
            txtLista.Name = "txtLista";
            txtLista.Size = new Size(100, 23);
            txtLista.TabIndex = 13;
            txtLista.TextChanged += txtFiltros_TextChanged;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(159, 184);
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(250, 23);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 3;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLoading.Location = new Point(187, 151);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(190, 30);
            lblLoading.TabIndex = 2;
            lblLoading.Text = "Cargando precios...";
            // 
            // pnlLoading
            // 
            pnlLoading.BackColor = Color.White;
            pnlLoading.Controls.Add(progressBarLoading);
            pnlLoading.Controls.Add(lblLoading);
            pnlLoading.Dock = DockStyle.Fill;
            pnlLoading.Location = new Point(0, 0);
            pnlLoading.Name = "pnlLoading";
            pnlLoading.Size = new Size(573, 408);
            pnlLoading.TabIndex = 21;
            pnlLoading.Visible = false;
            // 
            // nudPagina
            // 
            nudPagina.BorderStyle = BorderStyle.FixedSingle;
            nudPagina.Location = new Point(284, 372);
            nudPagina.Name = "nudPagina";
            nudPagina.Size = new Size(60, 23);
            nudPagina.TabIndex = 22;
            nudPagina.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(231, 374);
            label3.Name = "label3";
            label3.Size = new Size(47, 17);
            label3.TabIndex = 23;
            label3.Text = "Pagina";
            // 
            // FormPreciosGmg_ERP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(573, 408);
            Controls.Add(label3);
            Controls.Add(nudPagina);
            Controls.Add(pnlLoading);
            Controls.Add(btnVolver);
            Controls.Add(lblVersion);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnBuscar);
            Controls.Add(dgvPrecios);
            Controls.Add(txtArticulo);
            Controls.Add(txtLista);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormPreciosGmg_ERP";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Precios GMG";
            Load += FormPreciosGmg_ERP_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).EndInit();
            pnlLoading.ResumeLayout(false);
            pnlLoading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPagina).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private Label label2;
        private Label label1;
        private Button btnBuscar;
        private DataGridView dgvPrecios;
        private TextBox txtArticulo;
        private TextBox txtLista;
        private ProgressBar progressBarLoading;
        private Label lblLoading;
        private Panel pnlLoading;
        private NumericUpDown nudPagina;
        private Label label3;
    }
}