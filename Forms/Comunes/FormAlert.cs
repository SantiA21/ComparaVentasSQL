using CinetCore.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinetCore.Forms.Comunes
{
    public class FormAlert : Form
    {
        private Label lblMessage;
        private Button btnOk;
        private Button btnCancel;
        private PictureBox iconBox;

        public FormAlert(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent(message, title, buttons, icon);
            UIHelper.ApplyModernTheme(this);
            
            // Adjust specific UI elements after theme application
            this.BackColor = Color.White;
            lblMessage.BackColor = Color.Transparent;
            
            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.OKCancel)
            {
                btnCancel.BackColor = Color.FromArgb(108, 117, 125); // Gray for secondary action
                btnCancel.ForeColor = Color.White;
            }
        }

        private void InitializeComponent(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            this.lblMessage = new Label();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.iconBox = new PictureBox();

            // Form properties
            this.Text = title;
            this.ClientSize = new Size(400, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            // Icon
            this.iconBox.Size = new Size(48, 48);
            this.iconBox.Location = new Point(20, 20);
            this.iconBox.SizeMode = PictureBoxSizeMode.Zoom;
            
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    this.iconBox.Image = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Warning:
                    this.iconBox.Image = SystemIcons.Warning.ToBitmap();
                    break;
                case MessageBoxIcon.Information:
                    this.iconBox.Image = SystemIcons.Information.ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    this.iconBox.Image = SystemIcons.Question.ToBitmap();
                    break;
                default:
                    this.iconBox.Visible = false;
                    break;
            }

            // Message
            this.lblMessage.Text = message;
            this.lblMessage.Location = new Point(this.iconBox.Visible ? 80 : 20, 25);
            this.lblMessage.Size = new Size(this.iconBox.Visible ? 300 : 360, 60);
            this.lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            this.lblMessage.Font = new Font("Segoe UI", 10F);

            // Button OK/Yes
            this.btnOk.Text = (buttons == MessageBoxButtons.YesNo) ? "Sí" : "Aceptar";
            this.btnOk.Size = new Size(90, 35);
            this.btnOk.DialogResult = (buttons == MessageBoxButtons.YesNo) ? DialogResult.Yes : DialogResult.OK;
            this.btnOk.Location = new Point(this.ClientSize.Width - 110, this.ClientSize.Height - 50);

            // Button Cancel/No
            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.OKCancel)
            {
                this.btnCancel.Text = (buttons == MessageBoxButtons.YesNo) ? "No" : "Cancelar";
                this.btnCancel.Size = new Size(90, 35);
                this.btnCancel.DialogResult = (buttons == MessageBoxButtons.YesNo) ? DialogResult.No : DialogResult.Cancel;
                
                this.btnOk.Location = new Point(this.ClientSize.Width - 210, this.ClientSize.Height - 50);
                this.btnCancel.Location = new Point(this.ClientSize.Width - 110, this.ClientSize.Height - 50);
                this.Controls.Add(this.btnCancel);
            }

            this.Controls.Add(this.iconBox);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnOk);

            this.AcceptButton = this.btnOk;
            if (this.btnCancel.Visible)
            {
                this.CancelButton = this.btnCancel;
            }
        }
    }
}
