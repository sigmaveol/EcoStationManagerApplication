using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class Helper
    {
        public static void SetHover(Control container, Color hoverColor, Color normalColor, EventHandler clickAction, params Control[] exceptions)
        {
            // Hover cho container
            container.MouseEnter += (s, e) => SetFillColor(container, hoverColor, exceptions);
            container.MouseLeave += (s, e) => SetFillColor(container, normalColor, exceptions);
            container.Click += clickAction;

            // Hover + click cho tất cả control con
            foreach (Control c in container.Controls)
            {
                if (exceptions != null && Array.Exists(exceptions, ex => ex == c)) continue;

                c.MouseMove += (s, e) => SetFillColor(container, hoverColor, exceptions);
                c.MouseLeave += (s, e) => SetFillColor(container, normalColor, exceptions);
                c.Click += clickAction;
            }
        }

        private static void SetFillColor(Control container, Color color, Control[] exceptions)
        {
            // Nếu là Guna2Panel thì đổi FillColor
            if (container is Guna2Panel pnl) pnl.FillColor = color;

            // Đổi BackColor tất cả control con ngoại trừ exceptions
            foreach (Control c in container.Controls)
            {
                if (exceptions != null && Array.Exists(exceptions, ex => ex == c)) continue;
                c.BackColor = color;
            }
        }
    }
}
