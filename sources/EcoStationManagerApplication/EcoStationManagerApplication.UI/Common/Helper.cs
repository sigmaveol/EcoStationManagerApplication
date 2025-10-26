
using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public class Helper
    {
        public static void HandleCtrlShortcuts(Guna2TextBox textBox, KeyEventArgs e)
        {
            int cursorPos = textBox.SelectionStart;
            string text = textBox.Text;

            // Ctrl + Backspace: xóa từ bên trái
            if (e.Control && e.KeyCode == Keys.Back)
            {
                if (cursorPos == 0) return;

                int lastSpace = text.LastIndexOf(' ', cursorPos - 1);
                int startDelete = lastSpace >= 0 ? lastSpace : 0;
                int length = cursorPos - startDelete;

                textBox.BeginInvoke((Action)(() =>
                {
                    textBox.Text = text.Remove(startDelete, length);
                    textBox.SelectionStart = startDelete;
                }));

                e.Handled = true;
            }

            // Ctrl + Delete: xóa từ bên phải
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                if (cursorPos >= text.Length) return;

                int nextSpace = text.IndexOf(' ', cursorPos);
                int endDelete = nextSpace >= 0 ? nextSpace : text.Length;
                int length = endDelete - cursorPos;

                textBox.BeginInvoke((Action)(() =>
                {
                    textBox.Text = text.Remove(cursorPos, length);
                    textBox.SelectionStart = cursorPos;
                }));

                e.Handled = true;
            }

            // Ctrl + Left: di chuyển con trỏ sang trái theo từ
            else if (e.Control && e.KeyCode == Keys.Left)
            {
                if (cursorPos == 0) return;

                int prevSpace = text.LastIndexOf(' ', cursorPos - 1);
                textBox.SelectionStart = prevSpace >= 0 ? prevSpace : 0;
                e.Handled = true;
            }

            // Ctrl + Right: di chuyển con trỏ sang phải theo từ
            else if (e.Control && e.KeyCode == Keys.Right)
            {
                if (cursorPos >= text.Length) return;

                int nextSpace = text.IndexOf(' ', cursorPos);
                textBox.SelectionStart = nextSpace >= 0 ? nextSpace : text.Length;
                e.Handled = true;
            }
        }
    
        
    }
}
