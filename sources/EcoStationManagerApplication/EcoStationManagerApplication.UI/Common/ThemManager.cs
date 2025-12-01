using EcoStationManagerApplication.UI.Controls;
using Guna.UI2.WinForms;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class ThemeManager
    {
        // Primary Colors - Xanh lá đậm theo thiết kế
        public static Color PrimaryColor = Color.FromArgb(31, 107, 59);     // #1f6b3b
        public static Color PrimaryHoverColor = Color.FromArgb(33, 140, 73); // #218c49
        public static Color PrimaryForegroundColor = Color.White;
        public static Color PrimaryLightColor = Color.FromArgb(240, 248, 242); // rgba(31, 107, 59, 0.1)

        // Background & Card Colors
        public static Color BackgroundColor = Color.FromArgb(245, 247, 250); // #f5f7fa
        public static Color CardColor = Color.White;
        public static Color ForegroundColor = Color.FromArgb(51, 51, 51);    // #333
        public static Color MutedForegroundColor = Color.FromArgb(85, 85, 85); // #555
        public static Color BorderColor = Color.FromArgb(204, 204, 204);     // #ccc
        public static Color BorderLightColor = Color.FromArgb(242, 242, 242); // rgba(0, 0, 0, 0.05)

        // Status Colors
        public static Color SuccessColor = Color.FromArgb(31, 107, 59);      // #1f6b3b
        public static Color WarningColor = Color.FromArgb(249, 115, 22);     // #f97316
        public static Color ErrorColor = Color.FromArgb(212, 24, 61);        // #d4183d
        public static Color InfoColor = Color.FromArgb(59, 130, 246);        // #3b82f6

        // Sidebar Colors
        public static Color SidebarColor = Color.FromArgb(31, 107, 59);      // #1f6b3b
        public static Color SidebarForegroundColor = Color.White;
        public static Color SidebarAccentColor = Color.FromArgb(22, 90, 47); // #165a2f
        public static Color SidebarBorderColor = Color.FromArgb(255, 255, 255, 25); // rgba(255, 255, 255, 0.1)

        // Font Sizes (approximate conversion from rem to points)
        public static float TextXs = 9f;      // 0.75rem ≈ 12px ≈ 9pt
        public static float TextSm = 10.5f;   // 0.875rem ≈ 14px ≈ 10.5pt
        public static float TextBase = 12f;   // 1rem ≈ 16px ≈ 12pt
        public static float TextLg = 13.5f;   // 1.125rem ≈ 18px ≈ 13.5pt
        public static float TextXl = 15f;     // 1.25rem ≈ 20px ≈ 15pt
        public static float Text2Xl = 18f;    // 1.5rem ≈ 24px ≈ 18pt

        // Font Weights
        public static FontStyle FontWeightNormal = FontStyle.Regular;
        public static FontStyle FontWeightMedium = FontStyle.Bold;

        // Border Radius
        public static int BorderRadius = 10;      // 0.625rem ≈ 10px
        public static int BorderRadiusSm = 6;     // 0.375rem ≈ 6px
        public static int BorderRadiusMd = 8;     // 0.5rem ≈ 8px
        public static int BorderRadiusLg = 10;    // 0.625rem ≈ 10px
        public static int BorderRadiusXl = 14;    // 0.875rem ≈ 14px

        public static void ApplyTheme(Control control)
        {
            // Set background for form/container
            if (control is Form form)
            {
                form.BackColor = BackgroundColor;
            }
            else if (control is SidebarControl sidebar)
            {
                ApplySidebarTheme(sidebar);
            }

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
                else if (childControl is Guna2Panel panel)
                {
                    ApplyPanelTheme(panel);
                }
                else if (childControl is Label label)
                {
                    ApplyLabelTheme(label);
                }
                else if (childControl is Guna2GroupBox groupBox)
                {
                    ApplyGroupBoxTheme(groupBox);
                }
                else if (childControl is CardControl card)
                {
                    ApplyCardTheme(card);
                }

                // Recursively apply to child controls
                if (childControl.HasChildren)
                {
                    ApplyTheme(childControl);
                }
            }
        }

        public static void ApplyButtonTheme(Guna2Button button, ButtonVariant variant = ButtonVariant.Primary)
        {
            switch (variant)
            {
                case ButtonVariant.Primary:
                    button.FillColor = PrimaryColor;
                    button.ForeColor = PrimaryForegroundColor;
                    break;
                case ButtonVariant.Secondary:
                    button.FillColor = CardColor;
                    button.ForeColor = ForegroundColor;
                    button.BorderColor = BorderColor;
                    button.BorderThickness = 1;
                    break;
                case ButtonVariant.Success:
                    button.FillColor = SuccessColor;
                    button.ForeColor = PrimaryForegroundColor;
                    break;
                case ButtonVariant.Warning:
                    button.FillColor = WarningColor;
                    button.ForeColor = PrimaryForegroundColor;
                    break;
                case ButtonVariant.Danger:
                    button.FillColor = ErrorColor;
                    button.ForeColor = PrimaryForegroundColor;
                    break;
                case ButtonVariant.Ghost:
                    button.FillColor = Color.Transparent;
                    button.ForeColor = PrimaryColor;
                    button.BorderColor = Color.Transparent;
                    break;
            }

            button.BorderRadius = BorderRadius;
            button.Font = new Font("Segoe UI", TextBase, FontWeightNormal);

            // Hover states
            button.HoverState.FillColor = variant == ButtonVariant.Ghost ? PrimaryLightColor : PrimaryHoverColor;
            button.HoverState.ForeColor = variant == ButtonVariant.Ghost ? PrimaryColor : PrimaryForegroundColor;
        }

        public static void ApplyTextBoxTheme(Guna2TextBox textBox)
        {
            textBox.BorderColor = BorderColor;
            textBox.BorderRadius = BorderRadius;
            textBox.FillColor = CardColor;
            textBox.ForeColor = ForegroundColor;
            textBox.Font = new Font("Segoe UI", TextBase, FontWeightNormal);

            textBox.FocusedState.BorderColor = PrimaryColor;
            textBox.FocusedState.FillColor = CardColor;
            textBox.HoverState.BorderColor = PrimaryColor;
        }

        public static void ApplyPanelTheme(Guna2Panel panel, bool isCardStyle = false)
        {
            if (isCardStyle)
            {
                panel.FillColor = CardColor;
                panel.BorderColor = BorderLightColor;
                panel.BorderThickness = 1;
                panel.BorderRadius = BorderRadius;
            }
            else
            {
                panel.FillColor = Color.Transparent;
            }
        }

        public static void ApplySidebarTheme(SidebarControl sidebar)
        {
            // Apply theme to specific named panels (more reliable)
            var mainPanel = sidebar.Controls.Find("guna2PanelSidebar", true).FirstOrDefault();
            if (mainPanel is Guna2GradientPanel gradientPanel)
            {
                gradientPanel.FillColor = SidebarColor;
                gradientPanel.BorderColor = SidebarBorderColor;
                gradientPanel.BorderRadius = 0;
            }

            // Also theme other panels in the sidebar
            var menuPanel = sidebar.Controls.Find("guna2PanelMenuSidebar", true).FirstOrDefault();
            if (menuPanel is Guna2Panel panel)
            {
                panel.FillColor = SidebarColor;
                panel.BorderColor = SidebarBorderColor;
            }

            var userPanel = sidebar.Controls.Find("guna2PanelUserSidebar", true).FirstOrDefault();
            if (userPanel is Guna2Panel userSidebarPanel)
            {
                userSidebarPanel.FillColor = SidebarColor;
                userSidebarPanel.BorderColor = SidebarBorderColor;
            }

            var headerPanel = sidebar.Controls.Find("guna2PanelHeaderSidebar", true).FirstOrDefault();
            if (headerPanel is Guna2Panel headerSidebarPanel)
            {
                headerSidebarPanel.FillColor = SidebarColor;
                headerSidebarPanel.BorderColor = SidebarBorderColor;
            }
        }

        public static void ApplySidebarButtonTheme(Guna2Button button, bool isActive = false)
        {
            button.FillColor = isActive ? SidebarAccentColor : Color.Transparent;
            button.ForeColor = SidebarForegroundColor;
            button.BorderRadius = BorderRadiusMd;
            button.TextAlign = HorizontalAlignment.Left;
            button.Font = new Font("Segoe UI", TextBase, FontWeightNormal);

            button.HoverState.FillColor = SidebarAccentColor;
            button.HoverState.ForeColor = SidebarForegroundColor;
        }

        private static void ApplyDataGridViewTheme(Guna2DataGridView dataGridView)
        {
            // Header styling
            dataGridView.ThemeStyle.HeaderStyle.BackColor = PrimaryColor;
            dataGridView.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", TextSm, FontWeightMedium);
            dataGridView.ThemeStyle.HeaderStyle.ForeColor = PrimaryForegroundColor;

            // Row styling
            dataGridView.ThemeStyle.RowsStyle.BackColor = CardColor;
            dataGridView.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", TextSm, FontWeightNormal);
            dataGridView.ThemeStyle.RowsStyle.ForeColor = ForegroundColor;
            dataGridView.ThemeStyle.RowsStyle.SelectionBackColor = PrimaryLightColor;
            dataGridView.ThemeStyle.RowsStyle.SelectionForeColor = ForegroundColor;

            // Alternating rows
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 247, 247);
            dataGridView.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(247, 247, 247);
            dataGridView.ThemeStyle.AlternatingRowsStyle.Font = new Font("Segoe UI", TextSm, FontWeightNormal);
            dataGridView.ThemeStyle.AlternatingRowsStyle.ForeColor = ForegroundColor;

            // Grid lines and border
            dataGridView.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private static void ApplyLabelTheme(Label label)
        {
            label.ForeColor = ForegroundColor;
            label.Font = new Font("Segoe UI", TextBase, FontWeightNormal);

            // Auto-detect label types by common names or text
            if (label.Name?.ToLower().Contains("title") == true ||
                label.Name?.ToLower().Contains("header") == true)
            {
                label.Font = new Font("Segoe UI", TextXl, FontWeightMedium);
            }
            else if (label.Name?.ToLower().Contains("subtitle") == true)
            {
                label.Font = new Font("Segoe UI", TextLg, FontWeightNormal);
            }
            else if (label.Name?.ToLower().Contains("muted") == true)
            {
                label.ForeColor = MutedForegroundColor;
                label.Font = new Font("Segoe UI", TextSm, FontWeightNormal);
            }
        }

        private static void ApplyGroupBoxTheme(Guna2GroupBox groupBox)
        {
            groupBox.FillColor = CardColor;
            groupBox.ForeColor = ForegroundColor;
            groupBox.BorderColor = BorderLightColor;
            groupBox.BorderRadius = BorderRadius;
            groupBox.Font = new Font("Segoe UI", TextLg, FontWeightMedium);
        }

        private static void ApplyCardTheme(CardControl card)
        {
            //card.FillColor = CardColor;
            //card.BorderColor = BorderLightColor;
            //card.BorderRadius = BorderRadius;
            //card.ShadowDecoration.Enabled = true;
            //card.ShadowDecoration.Color = Color.FromArgb(0, 0, 0, 10);
            //card.ShadowDecoration.Blur = 4;
            //card.ShadowDecoration.Shadow = new Padding(0, 1, 2, 2);
        }

        // Utility method to apply status colors
        public static Color GetStatusColor(string status)
        {
            if (status == null)
                return MutedForegroundColor;

            string lowerStatus = status.ToLower();

            switch (lowerStatus)
            {
                case "active":
                case "completed":
                case "success":
                case "good":
                    return SuccessColor;
                case "pending":
                case "warning":
                case "scheduled":
                    return WarningColor;
                case "inactive":
                case "cancelled":
                case "error":
                case "expired":
                    return ErrorColor;
                case "processing":
                case "info":
                case "in_progress":
                    return InfoColor;
                default:
                    return MutedForegroundColor;
            }
        }

        // Method to apply specific module themes
        public static void ApplyModuleTheme(Control control, string moduleName)
        {
            // Module-specific theming can be added here
            ApplyTheme(control);
        }
    }

    public enum ButtonVariant
    {
        Primary,
        Secondary,
        Success,
        Warning,
        Danger,
        Ghost
    }
}