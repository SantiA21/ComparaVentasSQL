namespace ComparaVentasExcel
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lblArchivo = new Label();
            txtArchivo = new TextBox();
            btnExaminar = new Button();
            btnProcesar = new Button();
            dgvResultados = new DataGridView();
            lblEstado = new Label();
            btnExportar = new Button();
            chkMostrarTodos = new CheckBox();
            chkSoloExistentes = new CheckBox();
            chkSoloNoExistentes = new CheckBox();
            lblVersion = new Label();
            btnVolver = new Button();
            cbBaseDatos = new ComboBox();
            label1 = new Label();
            cbLocal = new ComboBox();
            cbSucursal = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            btnLimpiarFiltros = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
            SuspendLayout();
            // 
            // lblArchivo
            // 
            lblArchivo.AutoSize = true;
            lblArchivo.Location = new Point(148, 20);
            lblArchivo.Margin = new Padding(4, 0, 4, 0);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(81, 15);
            lblArchivo.TabIndex = 0;
            lblArchivo.Text = "Archivo Excel:";
            // 
            // txtArchivo
            // 
            txtArchivo.Location = new Point(148, 38);
            txtArchivo.Margin = new Padding(4, 3, 4, 3);
            txtArchivo.Name = "txtArchivo";
            txtArchivo.Size = new Size(116, 23);
            txtArchivo.TabIndex = 1;
            // 
            // btnExaminar
            // 
            btnExaminar.BackColor = SystemColors.ButtonFace;
            btnExaminar.Location = new Point(272, 35);
            btnExaminar.Margin = new Padding(4, 3, 4, 3);
            btnExaminar.Name = "btnExaminar";
            btnExaminar.Size = new Size(88, 27);
            btnExaminar.TabIndex = 2;
            btnExaminar.Text = "Examinar";
            btnExaminar.UseVisualStyleBackColor = false;
            btnExaminar.Click += btnExaminar_Click;
            // 
            // btnProcesar
            // 
            btnProcesar.BackColor = SystemColors.ButtonFace;
            btnProcesar.Location = new Point(368, 35);
            btnProcesar.Margin = new Padding(4, 3, 4, 3);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.Size = new Size(88, 27);
            btnProcesar.TabIndex = 3;
            btnProcesar.Text = "Procesar";
            btnProcesar.UseVisualStyleBackColor = false;
            btnProcesar.Click += btnProcesar_Click;
            // 
            // dgvResultados
            // 
            dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResultados.Location = new Point(148, 108);
            dgvResultados.Margin = new Padding(4, 3, 4, 3);
            dgvResultados.Name = "dgvResultados";
            dgvResultados.Size = new Size(679, 312);
            dgvResultados.TabIndex = 4;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(148, 432);
            lblEstado.Margin = new Padding(4, 0, 4, 0);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(0, 15);
            lblEstado.TabIndex = 5;
            // 
            // btnExportar
            // 
            btnExportar.BackColor = SystemColors.ButtonFace;
            btnExportar.Location = new Point(739, 426);
            btnExportar.Margin = new Padding(4, 3, 4, 3);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(88, 27);
            btnExportar.TabIndex = 6;
            btnExportar.Text = "Exportar";
            btnExportar.UseVisualStyleBackColor = false;
            btnExportar.Click += btnExportar_Click;
            // 
            // chkMostrarTodos
            // 
            chkMostrarTodos.AutoSize = true;
            chkMostrarTodos.Checked = true;
            chkMostrarTodos.CheckState = CheckState.Checked;
            chkMostrarTodos.Location = new Point(13, 107);
            chkMostrarTodos.Name = "chkMostrarTodos";
            chkMostrarTodos.Size = new Size(100, 19);
            chkMostrarTodos.TabIndex = 7;
            chkMostrarTodos.Text = "Mostrar todos";
            chkMostrarTodos.UseVisualStyleBackColor = true;
            chkMostrarTodos.CheckedChanged += chkMostrarTodos_CheckedChanged;
            // 
            // chkSoloExistentes
            // 
            chkSoloExistentes.AutoSize = true;
            chkSoloExistentes.Location = new Point(12, 132);
            chkSoloExistentes.Name = "chkSoloExistentes";
            chkSoloExistentes.Size = new Size(56, 19);
            chkSoloExistentes.TabIndex = 8;
            chkSoloExistentes.Text = "Existe";
            chkSoloExistentes.UseVisualStyleBackColor = true;
            chkSoloExistentes.CheckedChanged += chkSoloExistentes_CheckedChanged;
            // 
            // chkSoloNoExistentes
            // 
            chkSoloNoExistentes.AutoSize = true;
            chkSoloNoExistentes.Location = new Point(12, 157);
            chkSoloNoExistentes.Name = "chkSoloNoExistentes";
            chkSoloNoExistentes.Size = new Size(75, 19);
            chkSoloNoExistentes.TabIndex = 9;
            chkSoloNoExistentes.Text = "No existe";
            chkSoloNoExistentes.UseVisualStyleBackColor = true;
            chkSoloNoExistentes.CheckedChanged += chkSoloNoExistentes_CheckedChanged;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(760, 462);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 10;
            lblVersion.Text = "label3";
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(148, 454);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 23);
            btnVolver.TabIndex = 11;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // cbBaseDatos
            // 
            cbBaseDatos.FormattingEnabled = true;
            cbBaseDatos.Location = new Point(13, 38);
            cbBaseDatos.Name = "cbBaseDatos";
            cbBaseDatos.Size = new Size(121, 23);
            cbBaseDatos.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 20);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 13;
            label1.Text = "Base de datos:";
            // 
            // cbLocal
            // 
            cbLocal.FormattingEnabled = true;
            cbLocal.Location = new Point(12, 208);
            cbLocal.Name = "cbLocal";
            cbLocal.Size = new Size(121, 23);
            cbLocal.TabIndex = 14;
            // 
            // cbSucursal
            // 
            cbSucursal.FormattingEnabled = true;
            cbSucursal.Location = new Point(12, 254);
            cbSucursal.Name = "cbSucursal";
            cbSucursal.Size = new Size(121, 23);
            cbSucursal.TabIndex = 16;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 190);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 17;
            label2.Text = "Local:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 236);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 18;
            label3.Text = "Sucursal:";
            // 
            // btnLimpiarFiltros
            // 
            btnLimpiarFiltros.Location = new Point(12, 298);
            btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            btnLimpiarFiltros.Size = new Size(121, 23);
            btnLimpiarFiltros.TabIndex = 19;
            btnLimpiarFiltros.Text = "Limpiar filtros";
            btnLimpiarFiltros.UseVisualStyleBackColor = true;
            btnLimpiarFiltros.Click += btnLimpiarFiltros_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(859, 497);
            Controls.Add(btnLimpiarFiltros);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cbSucursal);
            Controls.Add(cbLocal);
            Controls.Add(label1);
            Controls.Add(cbBaseDatos);
            Controls.Add(btnVolver);
            Controls.Add(lblVersion);
            Controls.Add(chkSoloNoExistentes);
            Controls.Add(chkSoloExistentes);
            Controls.Add(chkMostrarTodos);
            Controls.Add(btnExportar);
            Controls.Add(lblEstado);
            Controls.Add(dgvResultados);
            Controls.Add(btnProcesar);
            Controls.Add(btnExaminar);
            Controls.Add(txtArchivo);
            Controls.Add(lblArchivo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ComparaVentas";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.DataGridView dgvResultados;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnExportar;
        private CheckBox chkMostrarTodos;
        private CheckBox chkSoloExistentes;
        private CheckBox chkSoloNoExistentes;
        private Label lblVersion;
        private Button btnVolver;
        private ComboBox cbBaseDatos;
        private Label label1;
        private ComboBox cbLocal;
        private ComboBox cbSucursal;
        private Label label2;
        private Label label3;
        private Button btnLimpiarFiltros;
    }
}

