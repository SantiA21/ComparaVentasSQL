namespace CinetCore.Forms.Usuarios
{
    partial class FormResultadosBackOffice
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
            dgvResultados = new DataGridView();
            btnVolver = new Button();
            lblVersion = new Label();
            label2 = new Label();
            label1 = new Label();
            cbEstado = new ComboBox();
            cbCategoria = new ComboBox();
            txtBuscar = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
            SuspendLayout();
            // 
            // dgvResultados
            // 
            dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResultados.Location = new Point(72, 41);
            dgvResultados.Name = "dgvResultados";
            dgvResultados.Size = new Size(637, 290);
            dgvResultados.TabIndex = 0;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(72, 355);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 13;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Cursor = Cursors.Help;
            lblVersion.FlatStyle = FlatStyle.Popup;
            lblVersion.ForeColor = SystemColors.ControlText;
            lblVersion.Location = new Point(637, 363);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 14;
            lblVersion.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(446, 15);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 24;
            label2.Text = "Estado:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(241, 15);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 23;
            label1.Text = "Categoria:";
            // 
            // cbEstado
            // 
            cbEstado.FormattingEnabled = true;
            cbEstado.Location = new Point(497, 12);
            cbEstado.Name = "cbEstado";
            cbEstado.Size = new Size(73, 23);
            cbEstado.TabIndex = 22;
            cbEstado.SelectedIndexChanged += cbEstado_SelectedIndexChanged;
            // 
            // cbCategoria
            // 
            cbCategoria.FormattingEnabled = true;
            cbCategoria.Location = new Point(308, 12);
            cbCategoria.Name = "cbCategoria";
            cbCategoria.Size = new Size(121, 23);
            cbCategoria.TabIndex = 21;
            cbCategoria.SelectedIndexChanged += cbCategoria_SelectedIndexChanged;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(77, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por usuario, nombre o apellido";
            txtBuscar.Size = new Size(146, 23);
            txtBuscar.TabIndex = 20;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // FormResultadosBackOffice
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(754, 401);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbEstado);
            Controls.Add(cbCategoria);
            Controls.Add(txtBuscar);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            Controls.Add(dgvResultados);
            Name = "FormResultadosBackOffice";
            Text = "FormResultadosBackOffice";
            Load += FormResultadosBackOffice_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvResultados;
        private Button btnVolver;
        private Label lblVersion;
        private Label label2;
        private Label label1;
        private ComboBox cbEstado;
        private ComboBox cbCategoria;
        private TextBox txtBuscar;
    }
}