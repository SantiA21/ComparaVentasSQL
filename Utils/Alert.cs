using CinetCore.Forms.Comunes;
using System;
using System.Windows.Forms;

namespace CinetCore.Utils
{
    public static class Alert
    {
        public static DialogResult ShowInfo(string message, string title = "Información")
        {
            using (var alert = new FormAlert(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information))
            {
                return alert.ShowDialog();
            }
        }

        public static DialogResult ShowError(string message, string title = "Error")
        {
            using (var alert = new FormAlert(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                return alert.ShowDialog();
            }
        }
        
        public static DialogResult ShowError(Exception ex, string title = "Error")
        {
            using (var alert = new FormAlert(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error))
            {
                return alert.ShowDialog();
            }
        }

        public static DialogResult ShowWarning(string message, string title = "Advertencia")
        {
            using (var alert = new FormAlert(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning))
            {
                return alert.ShowDialog();
            }
        }

        public static DialogResult Ask(string message, string title = "Confirmar")
        {
            using (var alert = new FormAlert(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return alert.ShowDialog();
            }
        }
        
        // Generic catch-all to replace MessageBox.Show
        public static DialogResult Show(string message, string title = "Mensaje", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            using (var alert = new FormAlert(message, title, buttons, icon))
            {
                return alert.ShowDialog();
            }
        }
    }
}
