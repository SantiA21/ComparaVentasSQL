namespace CinetCore
{
    partial class FormVerSucursalesV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVerSucursalesV2));
            dgvSucursales = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).BeginInit();
            SuspendLayout();
            // 
            // dgvSucursales
            // 
            dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSucursales.Dock = DockStyle.Fill;
            dgvSucursales.Location = new Point(0, 0);
            dgvSucursales.Name = "dgvSucursales";
            dgvSucursales.ReadOnly = true;
            dgvSucursales.Size = new Size(365, 315);
            dgvSucursales.TabIndex = 0;
            // 
            // FormVerSucursalesV2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(365, 315);
            Controls.Add(dgvSucursales);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormVerSucursalesV2";
            Text = "Sucursales";
            Load += FormVerSucursalesV2_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvSucursales;
    }
}