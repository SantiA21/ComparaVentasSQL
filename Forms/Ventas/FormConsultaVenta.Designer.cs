namespace CinetCore
{
    partial class FormConsultaVenta
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConsultaVenta));
            btnVolver = new Button();
            lblVersion = new Label();
            label2 = new Label();
            label3 = new Label();
            txtSucursal = new TextBox();
            txtComprobante = new TextBox();
            cbTipo = new ComboBox();
            btnConsultar = new Button();
            dgvVenta = new DataGridView();
            label4 = new Label();
            label1 = new Label();
            label5 = new Label();
            cbBaseDatos = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvVenta).BeginInit();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(61, 306);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 34);
            btnVolver.TabIndex = 12;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(304, 325);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 13;
            lblVersion.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(135, 79);
            label2.Name = "label2";
            label2.Size = new Size(128, 15);
            label2.TabIndex = 15;
            label2.Text = "Num de comprobante:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(135, 133);
            label3.Name = "label3";
            label3.Size = new Size(124, 15);
            label3.TabIndex = 16;
            label3.Text = "Tipo de comprobante:";
            // 
            // txtSucursal
            // 
            txtSucursal.Cursor = Cursors.IBeam;
            txtSucursal.Location = new Point(135, 43);
            txtSucursal.Name = "txtSucursal";
            txtSucursal.Size = new Size(121, 23);
            txtSucursal.TabIndex = 18;
            // 
            // txtComprobante
            // 
            txtComprobante.Cursor = Cursors.IBeam;
            txtComprobante.Location = new Point(135, 97);
            txtComprobante.Name = "txtComprobante";
            txtComprobante.Size = new Size(121, 23);
            txtComprobante.TabIndex = 19;
            // 
            // cbTipo
            // 
            cbTipo.Cursor = Cursors.Hand;
            cbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipo.FlatStyle = FlatStyle.Flat;
            cbTipo.FormattingEnabled = true;
            cbTipo.Items.AddRange(new object[] { "FAB", "FAA" });
            cbTipo.Location = new Point(135, 151);
            cbTipo.Name = "cbTipo";
            cbTipo.Size = new Size(121, 23);
            cbTipo.TabIndex = 20;
            // 
            // btnConsultar
            // 
            btnConsultar.BackColor = Color.FromArgb(43, 108, 176);
            btnConsultar.Cursor = Cursors.Hand;
            btnConsultar.FlatStyle = FlatStyle.Flat;
            btnConsultar.ForeColor = SystemColors.Window;
            btnConsultar.Location = new Point(135, 180);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(121, 23);
            btnConsultar.TabIndex = 21;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = false;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // dgvVenta
            // 
            dgvVenta.AllowUserToAddRows = false;
            dgvVenta.BackgroundColor = Color.Silver;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvVenta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvVenta.DefaultCellStyle = dataGridViewCellStyle2;
            dgvVenta.Location = new Point(61, 232);
            dgvVenta.Name = "dgvVenta";
            dgvVenta.ReadOnly = true;
            dgvVenta.RowHeadersVisible = false;
            dgvVenta.RowTemplate.Height = 22;
            dgvVenta.Size = new Size(281, 68);
            dgvVenta.TabIndex = 22;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(61, 214);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 26;
            label4.Text = "Venta";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(135, 25);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 30;
            label1.Text = "Sucursal:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 25);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 32;
            label5.Text = "Base de datos:";
            // 
            // cbBaseDatos
            // 
            cbBaseDatos.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaseDatos.FlatStyle = FlatStyle.Flat;
            cbBaseDatos.FormattingEnabled = true;
            cbBaseDatos.Location = new Point(12, 43);
            cbBaseDatos.Name = "cbBaseDatos";
            cbBaseDatos.Size = new Size(101, 23);
            cbBaseDatos.TabIndex = 31;
            // 
            // FormConsultaVenta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(397, 358);
            Controls.Add(label5);
            Controls.Add(cbBaseDatos);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(dgvVenta);
            Controls.Add(btnConsultar);
            Controls.Add(cbTipo);
            Controls.Add(txtComprobante);
            Controls.Add(txtSucursal);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormConsultaVenta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ConsultarVenta";
            Load += FormConsultaVenta_Load;
            ((System.ComponentModel.ISupportInitialize)dgvVenta).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;

        private Label label2;
        private Label label3;
        private TextBox txtSucursal;
        private TextBox txtComprobante;
        private ComboBox cbTipo;
        private Button btnConsultar;
        private DataGridView dgvVenta;
        private Label label4;
        private Label label1;
        private Label label5;
        private ComboBox cbBaseDatos;
    }
}