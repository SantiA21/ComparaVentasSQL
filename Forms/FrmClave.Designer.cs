namespace ComparaVentasExcel
{
    partial class FrmClave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmClave));
            txtClave = new TextBox();
            label1 = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // txtClave
            // 
            txtClave.Location = new Point(62, 42);
            txtClave.Name = "txtClave";
            txtClave.PasswordChar = '*';
            txtClave.Size = new Size(132, 23);
            txtClave.TabIndex = 0;
            txtClave.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(62, 24);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 1;
            label1.Text = "Ingrese la clave";
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.FromArgb(43, 108, 176);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.ForeColor = SystemColors.Window;
            btnAceptar.Location = new Point(131, 71);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(75, 38);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(43, 108, 176);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = SystemColors.Window;
            btnCancelar.Location = new Point(50, 71);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 38);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmClave
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(252, 121);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(label1);
            Controls.Add(txtClave);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FrmClave";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Clave";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtClave;
        private Label label1;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}