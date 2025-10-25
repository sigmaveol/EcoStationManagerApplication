using System.Drawing;
using Guna.UI2.WinForms;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class ThemeManager
    {
        public static Color PrimaryColor = Color.FromArgb(0, 98, 102);
        public static Color SecondaryColor = Color.FromArgb(30, 33, 57);
        public static Color SuccessColor = Color.FromArgb(46, 125, 50);
        public static Color WarningColor = Color.FromArgb(237, 108, 2);
        public static Color DangerColor = Color.FromArgb(198, 40, 40);
        public static Color HoverColor { get; set; } = Color.FromArgb(40, 40, 40);
        public static Color HoverTextColor { get; set; } = Color.White;

        public static void ApplyTheme(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl is Guna2Button button)
                {
                    ApplyButtonTheme(button);
                }
                else if (childControl is Guna2TextBox textBox)
                {
                    ApplyTextBoxTheme(textBox);
                }
                else if (childControl is Guna2DataGridView dataGridView)
                {
                    ApplyDataGridViewTheme(dataGridView);
                }

                // Recursively apply to child controls
                if (childControl.HasChildren)
                {
                    ApplyTheme(childControl);
                }
            }
        }

        private static void ApplyButtonTheme(Guna2Button button)
        {
            button.FillColor = PrimaryColor;
            button.ForeColor = Color.White;
            button.BorderRadius = 5;
            button.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        private static void ApplyTextBoxTheme(Guna2TextBox textBox)
        {
            textBox.BorderColor = Color.Gray;
            textBox.BorderRadius = 5;
            textBox.FocusedState.BorderColor = PrimaryColor;
            textBox.HoverState.BorderColor = PrimaryColor;
        }

        private static void ApplyDataGridViewTheme(Guna2DataGridView dataGridView)
        {
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 247, 247);
            dataGridView.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(247, 247, 247);
            dataGridView.ThemeStyle.HeaderStyle.BackColor = PrimaryColor;
            dataGridView.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridView.ThemeStyle.HeaderStyle.ForeColor = Color.White;
        }
    }
}