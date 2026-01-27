namespace ComparaVentasExcel
{
    partial class FormModifImporte
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModifImporte));
            btnVolver = new Button();
            label5 = new Label();
            cbBaseDatos = new ComboBox();
            label1 = new Label();
            btnModificar = new Button();
            cbTipo = new ComboBox();
            txtComprobante = new TextBox();
            txtSucursal = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label4 = new Label();
            txtImporte = new TextBox();
            lblVersion = new Label();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(12, 300);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 12;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 23);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 41;
            label5.Text = "Base de datos:";
            // 
            // cbBaseDatos
            // 
            cbBaseDatos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaseDatos.FlatStyle = FlatStyle.Flat;
            cbBaseDatos.FormattingEnabled = true;
            cbBaseDatos.Location = new Point(17, 41);
            cbBaseDatos.Name = "cbBaseDatos";
            cbBaseDatos.Size = new Size(101, 23);
            cbBaseDatos.TabIndex = 40;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(140, 23);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 39;
            label1.Text = "Sucursal:";
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(43, 108, 176);
            btnModificar.Cursor = Cursors.Hand;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.ForeColor = SystemColors.Window;
            btnModificar.Location = new Point(140, 241);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(121, 23);
            btnModificar.TabIndex = 38;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // cbTipo
            // 
            cbTipo.Cursor = Cursors.Hand;
            cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipo.FlatStyle = FlatStyle.Flat;
            cbTipo.FormattingEnabled = true;
            cbTipo.Items.AddRange(new object[] { "FAB", "FAA" });
            cbTipo.Location = new Point(140, 149);
            cbTipo.Name = "cbTipo";
            cbTipo.Size = new Size(121, 23);
            cbTipo.TabIndex = 37;
            // 
            // txtComprobante
            // 
            txtComprobante.Cursor = Cursors.IBeam;
            txtComprobante.Location = new Point(140, 95);
            txtComprobante.Name = "txtComprobante";
            txtComprobante.Size = new Size(121, 23);
            txtComprobante.TabIndex = 36;
            // 
            // txtSucursal
            // 
            txtSucursal.Cursor = Cursors.IBeam;
            txtSucursal.Location = new Point(140, 41);
            txtSucursal.Name = "txtSucursal";
            txtSucursal.Size = new Size(121, 23);
            txtSucursal.TabIndex = 35;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(140, 131);
            label3.Name = "label3";
            label3.Size = new Size(124, 15);
            label3.TabIndex = 34;
            label3.Text = "Tipo de comprobante:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(140, 77);
            label2.Name = "label2";
            label2.Size = new Size(128, 15);
            label2.TabIndex = 33;
            label2.Text = "Num de comprobante:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(140, 185);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 42;
            label4.Text = "Importe";
            // 
            // txtImporte
            // 
            txtImporte.Cursor = Cursors.IBeam;
            txtImporte.Location = new Point(140, 203);
            txtImporte.Name = "txtImporte";
            txtImporte.Size = new Size(121, 23);
            txtImporte.TabIndex = 43;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(312, 316);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 44;
            lblVersion.Text = "label3";
            // 
            // FormModifImporte
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(410, 343);
            Controls.Add(lblVersion);
            Controls.Add(txtImporte);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(cbBaseDatos);
            Controls.Add(label1);
            Controls.Add(btnModificar);
            Controls.Add(cbTipo);
            Controls.Add(txtComprobante);
            Controls.Add(txtSucursal);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnVolver);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormModifImporte";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Modificar Importe";
            Load += FormModifImporte_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label label5;
        private ComboBox cbBaseDatos;
        private Label label1;
        private Button btnModificar;
        private ComboBox cbTipo;
        private TextBox txtComprobante;
        private TextBox txtSucursal;
        private Label label3;
        private Label label2;
        private Label label4;
        private TextBox txtImporte;
        private Label lblVersion;
    }
}