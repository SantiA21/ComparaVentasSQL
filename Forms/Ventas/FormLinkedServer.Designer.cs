namespace CinetCore
{
    partial class FormLinkedServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLinkedServer));
            txtServidorMadre = new TextBox();
            btnCargarEquipos = new Button();
            cbEquipos = new ComboBox();
            btnConsultarEquipo = new Button();
            btnVolver = new Button();
            label1 = new Label();
            label2 = new Label();
            lblVersion = new Label();
            btnSucursales = new Button();
            SuspendLayout();
            // 
            // txtServidorMadre
            // 
            txtServidorMadre.Location = new Point(112, 63);
            txtServidorMadre.Name = "txtServidorMadre";
            txtServidorMadre.Size = new Size(114, 23);
            txtServidorMadre.TabIndex = 0;
            // 
            // btnCargarEquipos
            // 
            btnCargarEquipos.BackColor = Color.FromArgb(43, 108, 176);
            btnCargarEquipos.Cursor = Cursors.Hand;
            btnCargarEquipos.FlatStyle = FlatStyle.Flat;
            btnCargarEquipos.ForeColor = SystemColors.Window;
            btnCargarEquipos.Location = new Point(112, 92);
            btnCargarEquipos.Name = "btnCargarEquipos";
            btnCargarEquipos.Size = new Size(75, 23);
            btnCargarEquipos.TabIndex = 1;
            btnCargarEquipos.Text = "Conectar";
            btnCargarEquipos.UseVisualStyleBackColor = false;
            btnCargarEquipos.Click += btnCargarEquipos_Click;
            // 
            // cbEquipos
            // 
            cbEquipos.FormattingEnabled = true;
            cbEquipos.Location = new Point(112, 182);
            cbEquipos.Name = "cbEquipos";
            cbEquipos.Size = new Size(121, 23);
            cbEquipos.TabIndex = 2;
            // 
            // btnConsultarEquipo
            // 
            btnConsultarEquipo.BackColor = Color.FromArgb(43, 108, 176);
            btnConsultarEquipo.Cursor = Cursors.Hand;
            btnConsultarEquipo.FlatStyle = FlatStyle.Flat;
            btnConsultarEquipo.ForeColor = SystemColors.Window;
            btnConsultarEquipo.Location = new Point(112, 211);
            btnConsultarEquipo.Name = "btnConsultarEquipo";
            btnConsultarEquipo.Size = new Size(75, 23);
            btnConsultarEquipo.TabIndex = 3;
            btnConsultarEquipo.Text = "Consultar";
            btnConsultarEquipo.UseVisualStyleBackColor = false;
            btnConsultarEquipo.Click += btnConsultarEquipo_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(43, 108, 176);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = SystemColors.Window;
            btnVolver.Location = new Point(24, 280);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(116, 23);
            btnVolver.TabIndex = 12;
            btnVolver.Text = "Regresar al inicio";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(112, 45);
            label1.Name = "label1";
            label1.Size = new Size(17, 15);
            label1.TabIndex = 13;
            label1.Text = "IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(112, 164);
            label2.Name = "label2";
            label2.Size = new Size(74, 15);
            label2.TabIndex = 14;
            label2.Text = "LinkedServer";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(266, 284);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 15;
            lblVersion.Text = "label3";
            // 
            // btnSucursales
            // 
            btnSucursales.BackColor = Color.FromArgb(43, 108, 176);
            btnSucursales.Cursor = Cursors.Hand;
            btnSucursales.FlatStyle = FlatStyle.Flat;
            btnSucursales.ForeColor = SystemColors.Window;
            btnSucursales.Location = new Point(112, 138);
            btnSucursales.Name = "btnSucursales";
            btnSucursales.Size = new Size(121, 23);
            btnSucursales.TabIndex = 16;
            btnSucursales.Text = "Sucursales";
            btnSucursales.UseVisualStyleBackColor = false;
            btnSucursales.Click += btnSucursales_Click;
            // 
            // FormLinkedServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(365, 315);
            Controls.Add(btnSucursales);
            Controls.Add(lblVersion);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnVolver);
            Controls.Add(btnConsultarEquipo);
            Controls.Add(cbEquipos);
            Controls.Add(btnCargarEquipos);
            Controls.Add(txtServidorMadre);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormLinkedServer";
            Text = "Ventas con CAEA";
            Load += FormLinkedServer_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtServidorMadre;
        private Button btnCargarEquipos;
        private ComboBox cbEquipos;
        private Button btnConsultarEquipo;
        private Button btnVolver;
        private Label label1;
        private Label label2;
        private Label lblVersion;
        private Button btnSucursales;
    }
}