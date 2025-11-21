using EcoStationManagerApplication.UI.Controls;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Common.Services
{
    /// <summary>
    /// Service quản lý điều hướng giữa các UserControl trong ứng dụng
    /// </summary>
    public class NavigationService
    {
        private static NavigationService _instance;
        private Dictionary<string, UserControl> _contentCache;
        private Control _contentContainer;
        private string _currentView;

        private NavigationService()
        {
            _contentCache = new Dictionary<string, UserControl>();
        }

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NavigationService();
                return _instance;
            }
        }

        /// <summary>
        /// Khởi tạo NavigationService với container control (Panel, UserControl, etc.)
        /// </summary>
        public void Initialize(Control contentContainer)
        {
            _contentContainer = contentContainer ?? throw new ArgumentNullException(nameof(contentContainer));
        }

        /// <summary>
        /// Điều hướng đến một view cụ thể
        /// </summary>
        public void NavigateTo(string viewName)
        {
            if (_contentContainer == null)
                throw new InvalidOperationException("NavigationService chưa được khởi tạo. Gọi Initialize() trước.");

            var key = viewName?.ToLower() ?? "";
            _currentView = key;

            // Lấy container thực tế để thêm controls
            Control targetContainer = GetTargetContainer(_contentContainer);

            // Xóa tất cả controls hiện tại
            targetContainer.Controls.Clear();

            // Lấy hoặc tạo control từ cache
            if (!_contentCache.TryGetValue(key, out var control))
            {
                control = CreateUserControlForView(key);
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                    _contentCache[key] = control;
                }
            }

            if (control != null)
            {
                targetContainer.Controls.Add(control);
                control.BringToFront();

                // Refresh nếu control implement IRefreshableControl
                if (control is IRefreshableControl refreshable)
                {
                    refreshable.RefreshData();
                }
            }

            OnViewChanged?.Invoke(this, new ViewChangedEventArgs(key, control));
        }

        /// <summary>
        /// Lấy container thực tế để thêm controls
        /// Nếu là ContentControl, trả về ContentPanel bên trong
        /// </summary>
        private Control GetTargetContainer(Control container)
        {
            // Nếu là ContentControl, lấy ContentPanel bên trong
            if (container is ContentControl contentControl)
            {
                return contentControl.ContentPanel ?? container;
            }

            return container;
        }

        /// <summary>
        /// Tạo UserControl tương ứng với view name
        /// </summary>
        private UserControl CreateUserControlForView(string viewName)
        {
            UserControl control = null;
            var key = viewName?.ToLower() ?? "";

            switch (key)
            {
                case "dashboard":
                    control = new DashboardControl();
                    break;
                case "orders":
                    control = new OrdersControl();
                    break;
                case "products":
                    control = new ProductsControl();
                    break;
                case "suppliers":
                    control = new SuppliersControl();
                    break;
                case "customers":
                    control = new CustomersControl();
                    break;
                case "inventory":
                    control = new InventoryControl();
                    break;
                case "stockin":
                    control = new StockInManagementControl();
                    break;
                case "stockout":
                    control = new StockOutManagementControl();
                    break;
                case "staffs":
                    control = new StaffControl();
                    break;
                case "reports":
                    control = new ReportControl(
                        AppServices.ReportService,
                        AppServices.OrderService,
                        AppServices.OrderDetailService);
                    break;
                case "systemsettings":
                    control = new SystemSettingsControl();
                    break;
                default:
                    control = new DashboardControl(); // Default: trả về DashboardControl
                    break;
            }

            // Đăng ký events cho DashboardControl
            if (control is DashboardControl dashboard)
            {
                dashboard.ViewAllOrdersClicked += (s, e) => NavigateTo("orders");
            }

            return control;
        }

        /// <summary>
        /// Xóa cache của một view cụ thể
        /// </summary>
        public void ClearCache(string viewName = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                // Xóa tất cả cache
                foreach (var control in _contentCache.Values)
                {
                    if (control is IDisposable disposable)
                        disposable.Dispose();
                }
                _contentCache.Clear();
            }
            else
            {
                var key = viewName.ToLower();
                if (_contentCache.TryGetValue(key, out var control))
                {
                    if (control is IDisposable disposable)
                        disposable.Dispose();
                    _contentCache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Lấy view hiện tại
        /// </summary>
        public string CurrentView => _currentView;

        /// <summary>
        /// Event được gọi khi view thay đổi
        /// </summary>
        public event EventHandler<ViewChangedEventArgs> OnViewChanged;
    }

    /// <summary>
    /// EventArgs cho event ViewChanged
    /// </summary>
    public class ViewChangedEventArgs : EventArgs
    {
        public string ViewName { get; }
        public UserControl Control { get; }

        public ViewChangedEventArgs(string viewName, UserControl control)
        {
            ViewName = viewName;
            Control = control;
        }
    }
}

