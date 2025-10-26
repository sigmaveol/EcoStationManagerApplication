    using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class TogglerSidebar
    {

        public static int CollapsedWidth = 60;
        public static int ExpandedWidth = 200;
        public static int ResponsiveThreshold = 750;
        private static bool isCollapsed = false;

        public static void Toggle(Guna2GradientPanel sidebar, Control[] meunuLabels = null)
        {
            sidebar.Width = isCollapsed ? CollapsedWidth : ExpandedWidth;

            if (meunuLabels != null) 
            {
                foreach (Control label in meunuLabels) 
                {
                    label.Visible = !isCollapsed;
                }
            }
        }

        public static void Collapse(Guna2GradientPanel sidebar, Control[] meunuLabels = null)
        {

        } 

        public static void HandleResponsive(Form parentForm, Control sidebar)
        {
            if (parentForm == null || sidebar == null) return;

            parentForm.Resize += (s, e) =>
            {
                if (parentForm.Width < ResponsiveThreshold && !isCollapsed)
                {
                    sidebar.Width = CollapsedWidth;
                    ToggleMenuItems(sidebar, true);
                    isCollapsed = true;
                }
                else if (parentForm.Width >= ResponsiveThreshold && isCollapsed)
                {
                    sidebar.Width = ExpandedWidth;
                    ToggleMenuItems(sidebar, false);
                    isCollapsed = false;
                }
            };
        }

        private static void ToggleMenuItems(Control sidebar, bool expand)
        {
            foreach (Control c in sidebar.Controls) 
            { 
                if (c == null) continue;

                if (c is Guna2Button btn)
                {
                    btn.Text = expand ? btn.Tag?.ToString() ?? btn.Text : "";
                    btn.ImageAlign = expand ? HorizontalAlignment.Left : HorizontalAlignment.Center;
                    btn.TextAlign = expand ? HorizontalAlignment.Left : HorizontalAlignment.Center;
                    btn.Padding = expand ? new Padding(15, 0, 0, 0) : Padding.Empty;
                }
            }
        }

    }
}
