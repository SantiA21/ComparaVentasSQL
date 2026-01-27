namespace ComparaVentasExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsuariosMostazaERP));
            btnVolver = new Button();
            lblVersion = new Label();
            dgvUsuarios = new DataGridView();
            txtBuscar = new TextBox();
            cbCategoria = new ComboBox();
            cbEstado = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
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
            // cbCategoria
            // 
            cbCategoria.FormattingEnabled = true;
            cbCategoria.Location = new Point(289, 12);
            cbCategoria.Name = "cbCategoria";
            cbCategoria.Size = new Size(121, 23);
            cbCategoria.TabIndex = 16;
            cbCategoria.SelectedIndexChanged += cbCategoria_SelectedIndexChanged;
            // 
            // cbEstado
            // 
            cbEstado.FormattingEnabled = true;
            cbEstado.Location = new Point(478, 12);
            cbEstado.Name = "cbEstado";
            cbEstado.Size = new Size(73, 23);
            cbEstado.TabIndex = 17;
            cbEstado.SelectedIndexChanged += cbEstado_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(222, 15);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 18;
            label1.Text = "Categoria:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(427, 15);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 19;
            label2.Text = "Estado:";
            // 
            // FormUsuariosMostazaERP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbEstado);
            Controls.Add(cbCategoria);
            Controls.Add(txtBuscar);
            Controls.Add(dgvUsuarios);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormUsuariosMostazaERP";
            Text = "Usuarios Mostaza_ERP";
            Load += FormUsuariosGmgERP_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private DataGridView dgvUsuarios;
        private TextBox txtBuscar;
        private ComboBox cbCategoria;
        private ComboBox cbEstado;
        private Label label1;
        private Label label2;
    }
}