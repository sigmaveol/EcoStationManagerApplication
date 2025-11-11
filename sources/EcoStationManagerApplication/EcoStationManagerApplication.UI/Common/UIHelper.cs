using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common
{
    public static class UIHelper
    {
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
    }
}
