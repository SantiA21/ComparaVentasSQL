namespace CinetCore.Forms.Usuarios
{
    partial class FormConexionRemota
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConexionRemota));
            txtIp = new TextBox();
            btnConectar = new Button();
            SuspendLayout();
            // 
            // txtIp
            // 
            txtIp.Location = new Point(58, 92);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(154, 23);
            txtIp.TabIndex = 0;
            // 
            // btnConectar
            // 
            btnConectar.Location = new Point(96, 121);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(75, 23);
            btnConectar.TabIndex = 1;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = true;
            btnConectar.Click += btnConectar_Click;
            // 
            // FormConexionRemota
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(272, 277);
            Controls.Add(btnConectar);
            Controls.Add(txtIp);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormConexionRemota";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Conexion a BackOffice";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIp;
        private Button btnConectar;
    }
}