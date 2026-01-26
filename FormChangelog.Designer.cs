namespace ComparaVentasExcel
{
    partial class FormChangelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangelog));
            txtChangelog = new TextBox();
            SuspendLayout();
            // 
            // txtChangelog
            // 
            txtChangelog.AcceptsReturn = true;
            txtChangelog.Dock = DockStyle.Fill;
            txtChangelog.Location = new Point(0, 0);
            txtChangelog.Multiline = true;
            txtChangelog.Name = "txtChangelog";
            txtChangelog.ReadOnly = true;
            txtChangelog.ScrollBars = ScrollBars.Vertical;
            txtChangelog.Size = new Size(410, 301);
            txtChangelog.TabIndex = 0;
            // 
            // FormChangelog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(410, 301);
            Controls.Add(txtChangelog);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormChangelog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Notas de parche";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtChangelog;
    }
}