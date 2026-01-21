namespace ComparaVentasExcel
{
    partial class FormVerSucursales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVerSucursales));
            btnVolver = new Button();
            lblVersion = new Label();
            dgvSucursales = new DataGridView();
            txtBuscar = new TextBox();
            label1 = new Label();
            label2 = new Label();
            cbBaseDatos = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).BeginInit();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(12, 394);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 27);
            btnVolver.TabIndex = 13;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(432, 402);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 14;
            lblVersion.Text = "label3";
            // 
            // dgvSucursales
            // 
            dgvSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSucursales.Location = new Point(142, 59);
            dgvSucursales.Name = "dgvSucursales";
            dgvSucursales.Size = new Size(240, 306);
            dgvSucursales.TabIndex = 15;
            // 
            // txtBuscar
            // 
            txtBuscar.Cursor = Cursors.IBeam;
            txtBuscar.Location = new Point(280, 30);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(102, 23);
            txtBuscar.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(280, 12);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 18;
            label1.Text = "Buscador";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(142, 9);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 20;
            label2.Text = "Base de datos:";
            // 
            // cbBaseDatos
            // 
            cbBaseDatos.Cursor = Cursors.Hand;
            cbBaseDatos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaseDatos.FlatStyle = FlatStyle.Popup;
            cbBaseDatos.FormattingEnabled = true;
            cbBaseDatos.Location = new Point(142, 27);
            cbBaseDatos.Name = "cbBaseDatos";
            cbBaseDatos.Size = new Size(101, 23);
            cbBaseDatos.TabIndex = 19;
            // 
            // FormVerSucursales
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(526, 433);
            Controls.Add(label2);
            Controls.Add(cbBaseDatos);
            Controls.Add(label1);
            Controls.Add(txtBuscar);
            Controls.Add(dgvSucursales);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormVerSucursales";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sucursales";
            Load += FormVerSucursales_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private DataGridView dgvSucursales;
        private TextBox txtBuscar;
        private Label label1;
        private Label label2;
        private ComboBox cbBaseDatos;
    }
}