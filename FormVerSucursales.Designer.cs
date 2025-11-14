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
            btnVolver = new Button();
            lblVersion = new Label();
            dgvSucursales = new DataGridView();
            lblEstado = new Label();
            txtBuscar = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).BeginInit();
            SuspendLayout();
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(12, 398);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 23);
            btnVolver.TabIndex = 13;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = true;
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
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(142, 368);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(38, 15);
            lblEstado.TabIndex = 16;
            lblEstado.Text = "label1";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(142, 30);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(240, 23);
            txtBuscar.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(142, 12);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 18;
            label1.Text = "Buscador";
            // 
            // FormVerSucursales
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(526, 433);
            Controls.Add(label1);
            Controls.Add(txtBuscar);
            Controls.Add(lblEstado);
            Controls.Add(dgvSucursales);
            Controls.Add(lblVersion);
            Controls.Add(btnVolver);
            Name = "FormVerSucursales";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormVerSucursales";
            Load += FormVerSucursales_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnVolver;
        private Label lblVersion;
        private DataGridView dgvSucursales;
        private Label lblEstado;
        private TextBox txtBuscar;
        private Label label1;
    }
}