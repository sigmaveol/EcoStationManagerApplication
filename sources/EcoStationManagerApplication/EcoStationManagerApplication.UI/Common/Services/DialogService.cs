using EcoStationManagerApplication.Models.Results;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common.Services
{
    /// <summary>
    /// Service quản lý hiển thị các dialog trong ứng dụng
    /// </summary>
    public class DialogService
    {
        private static DialogService _instance;
        private Form _parentForm;

        private DialogService()
        {
        }

        public static DialogService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DialogService();
                return _instance;
            }
        }

        /// <summary>
        /// Khởi tạo DialogService với parent form
        /// </summary>
        public void Initialize(Form parentForm)
        {
            _parentForm = parentForm ?? throw new ArgumentNullException(nameof(parentForm));
        }

        /// <summary>
        /// Hiển thị dialog và trả về kết quả
        /// </summary>
        public DialogResult ShowDialog(Form dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

            if (_parentForm != null)
            {
                dialog.StartPosition = FormStartPosition.CenterParent;
                return dialog.ShowDialog(_parentForm);
            }
            else
            {
                dialog.StartPosition = FormStartPosition.CenterScreen;
                return dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Hiển thị message box với các tùy chọn
        /// </summary>
        public DialogResult ShowMessage(string message, string title = "Thông báo", 
            MessageBoxButtons buttons = MessageBoxButtons.OK, 
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            if (_parentForm != null)
            {
                return MessageBox.Show(_parentForm, message, title, buttons, icon);
            }
            else
            {
                return MessageBox.Show(message, title, buttons, icon);
            }
        }

        /// <summary>
        /// Hiển thị dialog xác nhận
        /// </summary>
        public bool ShowConfirm(string message, string title = "Xác nhận")
        {
            var result = ShowMessage(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Hiển thị dialog cảnh báo
        /// </summary>
        public void ShowWarning(string message, string title = "Cảnh báo")
        {
            ShowMessage(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Hiển thị dialog lỗi
        /// </summary>
        public void ShowError(string message, string title = "Lỗi")
        {
            ShowMessage(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Hiển thị dialog thành công
        /// </summary>
        public void ShowSuccess(string message, string title = "Thành công")
        {
            ShowMessage(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Hiển thị dialog với exception
        /// </summary>
        public void ShowException(Exception ex, string operationName = "thực hiện thao tác")
        {
            var message = $"Đã xảy ra lỗi khi {operationName}:\n{ex.Message}";
            if (ex.InnerException != null)
            {
                message += $"\n\nChi tiết:\n{ex.InnerException.Message}";
            }
            ShowError(message, "Lỗi");
        }

        /// <summary>
        /// Xử lý kết quả từ service và hiển thị thông báo phù hợp
        /// </summary>
        public void HandleServiceResult<T>(Result<T> result, Action<T> onSuccess = null)
        {
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ShowSuccess(result.Message);
                }
                onSuccess?.Invoke(result.Data);
            }
            else
            {
                if (result.Errors != null && result.Errors.Any())
                {
                    ShowError(string.Join("\n", result.Errors));
                }
                else
                {
                    ShowError(result.Message);
                }
            }
        }

        /// <summary>
        /// Xử lý kết quả từ service và hiển thị thông báo phù hợp (không có data)
        /// </summary>
        public void HandleServiceResult(Result result, Action onSuccess = null)
        {
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    ShowSuccess(result.Message);
                }
                onSuccess?.Invoke();
            }
            else
            {
                if (result.Errors != null && result.Errors.Any())
                {
                    ShowError(string.Join("\n", result.Errors));
                }
                else
                {
                    ShowError(result.Message);
                }
            }
        }
    }
}

