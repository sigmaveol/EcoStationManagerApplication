using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.Models.Results;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class UIHelper
    {
        /// <summary>
        /// Áp dụng UIConfig cho tất cả control trong parent (recursive)
        /// </summary>
        public static void ApplyUIConfig(Control parent)
        {
            if (parent == null) return;

            var uiConfig = ConfigManager.GetUIConfig();

            foreach (Control ctrl in parent.Controls)
            {
                // Panel, Guna2Panel, Guna2GradientPanel
                if (ctrl is Panel || ctrl is Guna2Panel || ctrl is Guna2GradientPanel)
                {
                    ctrl.BackColor = ColorTranslator.FromHtml(uiConfig.PrimaryColor);
                }

                // Button, Guna2Button
                else if (ctrl is Button || ctrl is Guna2Button)
                {
                    ctrl.BackColor = ColorTranslator.FromHtml(uiConfig.SecondaryColor);
                    ctrl.ForeColor = Color.White;
                    ctrl.Font = new Font(uiConfig.FontFamily, uiConfig.FontSize);
                }

                // Label, Guna2HtmlLabel
                else if (ctrl is Label || ctrl is Guna2HtmlLabel)
                {
                    ctrl.Font = new Font(uiConfig.FontFamily, uiConfig.FontSize);
                    ctrl.ForeColor = uiConfig.GunaTheme == "Dark" ? Color.LightGray : Color.Black;
                }

                // Các Guna2CirclePictureBox hoặc PictureBox: không thay đổi màu nền, giữ nguyên
                // Nếu muốn highlight border, có thể dùng ctrl.BackColor hoặc ShadowDecoration

                // Recursive cho child controls
                if (ctrl.HasChildren)
                    ApplyUIConfig(ctrl);
            }

            // Animation (ví dụ cho Guna2Transition)
            if (uiConfig.EnableAnimations)
            {
                var transition = new Guna2Transition();
                transition.AnimationType = Guna.UI2.AnimatorNS.AnimationType.HorizSlide;
                transition.Interval = 300;

                // Áp dụng cho panel chính nếu muốn
                if (parent is Guna2Panel || parent is Guna2GradientPanel)
                {
                    transition.ShowSync(parent);
                }
            }
        }

        public static void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool ShowConfirmDialog(string message, string title = "Xác nhận")
        {
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public static void HandleServiceResult<T>(Result<T> result, Action<T> onSuccess = null)
        {
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ShowSuccessMessage(result.Message);
                }
                onSuccess?.Invoke(result.Data);
            }
            else
            {
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    ShowErrorMessage(string.Join("\n", result.Errors));
                }
                else
                {
                    ShowErrorMessage(result.Message);
                }
            }
        }

        public static void HandleServiceResult(Result result, Action onSuccess = null)
        {
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ShowSuccessMessage(result.Message);
                }
                onSuccess?.Invoke();
            }
            else
            {
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    ShowErrorMessage(string.Join("\n", result.Errors));
                }
                else
                {
                    ShowErrorMessage(result.Message);
                }
            }
        }

        public static void ShowExceptionError(Exception ex, string operationName)
        {
            var errorMessage = $"Đã xảy ra lỗi khi {operationName}:\n{ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
            }
            ShowErrorMessage(errorMessage);
        }

        public static void SetControlEnabled(Control control, bool enabled)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, bool>(SetControlEnabled), control, enabled);
            }
            else
            {
                control.Enabled = enabled;
            }
        }

        public static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static Image CreateInitialAvatar(string fullName, Size size)
        {
            var initials = GetInitials(fullName);
            var bmp = new Bitmap(Math.Max(size.Width, 52), Math.Max(size.Height, 50));
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(GetColorFromName(fullName));
                var font = new Font("Segoe UI Semibold", 20, FontStyle.Bold);
                var textSize = g.MeasureString(initials, font);
                var x = (bmp.Width - textSize.Width) / 2f;
                var y = (bmp.Height - textSize.Height) / 2f;
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString(initials, font, brush, x, y);
                }
            }
            return bmp;
        }

        private static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "?";
            var parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return "?";
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            var first = parts[0].Substring(0, 1).ToUpper();
            var last = parts[parts.Length - 1].Substring(0, 1).ToUpper();
            return first + last;
        }

        private static Color GetColorFromName(string name)
        {
            if (string.IsNullOrEmpty(name)) return Color.FromArgb(120, 120, 120);
            var hash = name.GetHashCode();
            var r = 100 + (Math.Abs(hash) % 156);
            var g = 100 + (Math.Abs(hash / 3) % 156);
            var b = 100 + (Math.Abs(hash / 7) % 156);
            return Color.FromArgb(r, g, b);
        }
    }
}
