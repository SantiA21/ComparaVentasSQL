namespace ComparaVentasExcel.Forms.Sucursales
{
    partial class FormInsertarSucursalFE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsertarSucursalFE));
            lblVersion = new Label();
            label5 = new Label();
            cbBaseDatos = new ComboBox();
            label1 = new Label();
            btnInsertar = new Button();
            txtSucursal = new TextBox();
            btnVolver = new Button();
            SuspendLayout();
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(212, 173);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 57;
            lblVersion.Text = "label3";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 43);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 54;
            label5.Text = "Base de datos:";
            // 
            // cbBaseDatos
            // 
            cbBaseDatos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaseDatos.FlatStyle = FlatStyle.Flat;
            cbBaseDatos.FormattingEnabled = true;
            cbBaseDatos.Location = new Point(27, 61);
            cbBaseDatos.Name = "cbBaseDatos";
            cbBaseDatos.Size = new Size(101, 23);
            cbBaseDatos.TabIndex = 53;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(135, 43);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 52;
            label1.Text = "Sucursal:";
            // 
            // btnInsertar
            // 
            btnInsertar.BackColor = Color.FromArgb(43, 108, 176);
            btnInsertar.Cursor = Cursors.Hand;
            btnInsertar.FlatStyle = FlatStyle.Flat;
            btnInsertar.ForeColor = SystemColors.Window;
            btnInsertar.Location = new Point(135, 99);
            btnInsertar.Name = "btnInsertar";
            btnInsertar.Size = new Size(121, 23);
            btnInsertar.TabIndex = 51;
            btnInsertar.Text = "Insertar";
            btnInsertar.UseVisualStyleBackColor = false;
            btnInsertar.Click += btnInsertar_Click;
            // 
            // txtSucursal
            // 
            txtSucursal.Cursor = Cursors.IBeam;
            txtSucursal.Location = new Point(135, 61);
            txtSucursal.Name = "txtSucursal";
            txtSucursal.Size = new Size(121, 23);
            txtSucursal.TabIndex = 48;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(12, 154);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 45;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FormInsertarSucursalFE
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(295, 197);
            Controls.Add(lblVersion);
            Controls.Add(label5);
            Controls.Add(cbBaseDatos);
            Controls.Add(label1);
            Controls.Add(btnInsertar);
            Controls.Add(txtSucursal);
            Controls.Add(btnVolver);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormInsertarSucursalFE";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Insertar Sucursal FE";
            Load += FormInsertarSucursalFE_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblVersion;
        private TextBox txtImporte;
        private Label label4;
        private Label label5;
        private ComboBox cbBaseDatos;
        private Label label1;
        private Button btnInsertar;
        private ComboBox cbTipo;
        private TextBox txtComprobante;
        private TextBox txtSucursal;
        private Label label3;
        private Label label2;
        private Button btnVolver;
    }
}