namespace CinetCore
{
    partial class FormUsuariosGmgERP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsuariosGmgERP));
            btnVolver = new Button();
            lblVersion = new Label();
            dgvUsuarios = new DataGridView();
            txtBuscar = new TextBox();
            nudPagina = new NumericUpDown();
            lblPagina = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPagina).BeginInit();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(36, 397);
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
            lblVersion.Location = new Point(690, 405);
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
            // nudPagina
            // 
            nudPagina.BorderStyle = BorderStyle.FixedSingle;
            nudPagina.Location = new Point(384, 403);
            nudPagina.Name = "nudPagina";
            nudPagina.Size = new Size(53, 23);
            nudPagina.TabIndex = 26;
            nudPagina.TextAlign = HorizontalAlignment.Center;
            nudPagina.ValueChanged += nudPagina_ValueChanged;
            // 
            // lblPagina
            // 
            lblPagina.AutoSize = true;
            lblPagina.Location = new Point(335, 405);
            lblPagina.Name = "lblPagina";
            lblPagina.Size = new Size(43, 15);
            lblPagina.TabIndex = 25;
            lblPagina.Text = "Pagina";
            // 
            // FormUsuariosGmgERP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 442);
            Controls.Add(nudPagina);
            Controls.Add(lblPagina);
            Controls.Add(txtBuscar);
            Controls.Add(dgvUsuarios);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormUsuariosGmgERP";
            Text = "Usuarios GMG_ERP";
            Load += FormUsuariosGmgERP_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPagina).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private DataGridView dgvUsuarios;
        private TextBox txtBuscar;
        private NumericUpDown nudPagina;
        private Label lblPagina;
    }
}