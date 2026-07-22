using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinetCore.Utils
{
    public static class UIHelper
    {
        public static void ApplyModernTheme(Form form)
        {
            if (form == null) return;

            form.BackColor = Color.FromArgb(244, 246, 249);
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            string versionSuffix = $"(v{AppInfo.Version})";
            if (!string.IsNullOrWhiteSpace(form.Text) && !form.Text.EndsWith(versionSuffix))
            {
                form.Text = $"{form.Text} {versionSuffix}";
            }

            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is DataGridView dgv)
                {
                    dgv.BackgroundColor = Color.White;
                    dgv.BorderStyle = BorderStyle.None;
                    dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 162, 232);
                    dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;

                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 246, 249);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 5, 0, 5);

                    dgv.RowHeadersVisible = false;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                    dgv.RowTemplate.Height = 35;
                }
                else if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                    btn.Cursor = Cursors.Hand;

                    if (btn.BackColor == SystemColors.Control || btn.BackColor.ToArgb() == SystemColors.Control.ToArgb())
                    {
                        btn.BackColor = Color.FromArgb(0, 162, 232); // Celeste
                        btn.ForeColor = Color.White;
                    }
                    else if (btn.BackColor == Color.LightBlue || btn.BackColor == Color.LightCyan)
                    {
                        btn.BackColor = Color.FromArgb(0, 162, 232); // Celeste
                        btn.ForeColor = Color.White;
                    }
                    // For buttons that might be "danger" or "success", if they have custom colors like Red/Green, adapt them to the palette
                    else if (btn.BackColor == Color.Red || btn.BackColor == Color.IndianRed || btn.BackColor == Color.FromArgb(220, 53, 69))
                    {
                        btn.BackColor = Color.FromArgb(108, 117, 125); // Gris
                        btn.ForeColor = Color.White;
                    }
                    else if (btn.BackColor == Color.Green || btn.BackColor == Color.LightGreen || btn.BackColor == Color.FromArgb(40, 167, 69))
                    {
                        btn.BackColor = Color.FromArgb(0, 162, 232); // Celeste
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        // Any other unhandled custom back color defaults to Celeste to match brand
                        btn.BackColor = Color.FromArgb(0, 162, 232); // Celeste
                        btn.ForeColor = Color.White;
                    }
                }

                else if (ctrl is GroupBox gb)
                {
                    gb.Font = new Font("Segoe UI Semibold", 9F);
                    gb.ForeColor = Color.FromArgb(64, 64, 64);
                    ApplyToControls(gb.Controls);
                }
                else if (ctrl is Label lbl)
                {
                    if (lbl.Font.Bold)
                    {
                        lbl.Font = new Font("Segoe UI Semibold", lbl.Font.Size, FontStyle.Bold);
                        lbl.ForeColor = Color.FromArgb(0, 162, 232);
                    }
                    else
                    {
                        lbl.Font = new Font("Segoe UI", lbl.Font.Size);
                    }
                }
                else if (ctrl is TextBox || ctrl is ComboBox || ctrl is CheckBox)
                {
                    ctrl.Font = new Font("Segoe UI", 9F);
                }
                else if (ctrl is MenuStrip || ctrl is ToolStrip)
                {
                    ctrl.BackColor = Color.FromArgb(245, 246, 248);
                    ctrl.Font = new Font("Segoe UI", 9F);
                }

                if (ctrl.Controls.Count > 0 && !(ctrl is GroupBox))
                {
                    ApplyToControls(ctrl.Controls);
                }
            }
        }
    }
}
