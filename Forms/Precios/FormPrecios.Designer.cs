namespace ComparaVentasExcel.Forms.Precios
{
    partial class FormPrecios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrecios));
            txtLista = new TextBox();
            txtArticulo = new TextBox();
            dgvPrecios = new DataGridView();
            btnBuscar = new Button();
            label1 = new Label();
            label2 = new Label();
            lblVersion = new Label();
            btnVolver = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).BeginInit();
            SuspendLayout();
            // 
            // txtLista
            // 
            txtLista.Location = new Point(97, 12);
            txtLista.Name = "txtLista";
            txtLista.Size = new Size(100, 23);
            txtLista.TabIndex = 0;
            // 
            // txtArticulo
            // 
            txtArticulo.Location = new Point(289, 15);
            txtArticulo.Name = "txtArticulo";
            txtArticulo.Size = new Size(100, 23);
            txtArticulo.TabIndex = 1;
            // 
            // dgvPrecios
            // 
            dgvPrecios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrecios.Location = new Point(21, 60);
            dgvPrecios.Name = "dgvPrecios";
            dgvPrecios.Size = new Size(533, 296);
            dgvPrecios.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(43, 108, 176);
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = SystemColors.Window;
            btnBuscar.Location = new Point(423, 15);
            btnBuscar.Margin = new Padding(4, 3, 4, 3);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(88, 27);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 15);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 5;
            label1.Text = "Lista precio:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(203, 15);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 6;
            label2.Text = "Cod. Articulo:";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Cursor = Cursors.Help;
            lblVersion.Location = new Point(473, 366);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 11;
            lblVersion.Text = "label3";
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(21, 366);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 31);
            btnVolver.TabIndex = 12;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FormPrecios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(573, 408);
            Controls.Add(btnVolver);
            Controls.Add(lblVersion);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnBuscar);
            Controls.Add(dgvPrecios);
            Controls.Add(txtArticulo);
            Controls.Add(txtLista);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormPrecios";
            Text = "Precios";
            Load += FormPrecios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPrecios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLista;
        private TextBox txtArticulo;
        private DataGridView dgvPrecios;
        private Button btnBuscar;
        private Label label1;
        private Label label2;
        private Label lblVersion;
        private Button btnVolver;
    }
}