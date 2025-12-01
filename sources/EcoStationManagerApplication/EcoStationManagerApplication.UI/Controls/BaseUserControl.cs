using EcoStationManagerApplication.UI.Common;
using System;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    /// <summary>
    /// Base class cho tất cả UserControl trong ứng dụng
    /// Cung cấp các tính năng chung như refresh, loading state, error handling
    /// </summary>
    public abstract class BaseUserControl : UserControl, IRefreshableControl
    {
        protected bool _isLoading;
        protected bool _isInitialized;

        public BaseUserControl()
        {
            InitializeControl();
        }

        /// <summary>
        /// Khởi tạo control (override trong derived class)
        /// </summary>
        protected virtual void InitializeControl()
        {
            _isInitialized = true;
        }

        /// <summary>
        /// Refresh data (implement từ IRefreshableControl)
        /// </summary>
        public virtual void RefreshData()
        {
            if (!_isInitialized)
                return;

            try
            {
                LoadDataAsync();
            }
            catch (Exception ex)
            {
                HandleError(ex, "tải dữ liệu");
            }
        }

        /// <summary>
        /// Load data async (override trong derived class)
        /// </summary>
        protected virtual async void LoadDataAsync()
        {
            // Override trong derived class
            await System.Threading.Tasks.Task.CompletedTask;
        }

        /// <summary>
        /// Hiển thị trạng thái loading
        /// </summary>
        protected virtual void ShowLoading(bool show)
        {
            _isLoading = show;
            Cursor = show ? Cursors.WaitCursor : Cursors.Default;
            Enabled = !show;
        }

        /// <summary>
        /// Xử lý lỗi
        /// </summary>
        protected virtual void HandleError(Exception ex, string operationName = "thực hiện thao tác")
        {
            UIHelper.ShowExceptionError(ex, operationName);
        }

        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        protected virtual bool ValidateData()
        {
            return true;
        }

        /// <summary>
        /// Cleanup resources khi control bị dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cleanup();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Cleanup (override trong derived class nếu cần)
        /// </summary>
        protected virtual void Cleanup()
        {
            // Override trong derived class để cleanup resources
        }
    }
}

