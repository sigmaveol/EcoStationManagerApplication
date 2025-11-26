using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Common.Services;
using EcoStationManagerApplication.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            // Khởi tạo NavigationService với content panel
            AppServices.Navigation.Initialize(contentControl);
            
            // Khởi tạo DialogService với MainForm làm parent
            AppServices.Dialog.Initialize(this);
            
            // Đăng ký event từ NavigationService
            AppServices.Navigation.OnViewChanged += NavigationService_OnViewChanged;
        }

        private void NavigationService_OnViewChanged(object sender, ViewChangedEventArgs e)
        {
            // Xử lý khi view thay đổi (nếu cần)
            // Ví dụ: cập nhật title, log navigation, etc.
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeSidebar();
            InitializeSidebarMenu();
            // Sử dụng NavigationService thay vì ShowContent trực tiếp
            AppServices.Navigation.NavigateTo("dashboard");
        }

        private void InitializeSidebar()
        {
            // Đăng ký sự kiện từ SidebarControl
            sidebarControl.MenuClicked += OnSidebarMenuClicked;

            // Thiết lập thông tin cho sidebar
            sidebarControl.SetAppName("Eco Station");
            sidebarControl.SetUsername(AppUserContext.CurrentUsername);
             sidebarControl.SetAvatar(Properties.Resources.logo_pm);
            sidebarControl.SetLogo(Properties.Resources.logo_pm);

        }

        private void InitializeSidebarMenu() 
        {
            sidebarControl.ClearMenuItems();
            // Thêm các menu item - sử dụng icon từ resources
            sidebarControl.AddMenuItem("Dashboard", Properties.Resources.dashboard_icon, "dashboard");
            sidebarControl.AddMenuItem("Đơn hàng", Properties.Resources.order_icon, "Orders");
            sidebarControl.AddMenuItem("Sản phẩm && Bao bì", Properties.Resources.product_icon, "Products");
            sidebarControl.AddMenuItem("Nhà cung cấp", Properties.Resources.supplier_icon, "Suppliers");
            sidebarControl.AddMenuItem("Khách hàng", Properties.Resources.customer_icon, "Customers");
            sidebarControl.AddMenuItem("Tồn Kho", Properties.Resources.inventory_icon, "Inventory");
            sidebarControl.AddMenuItem("Nhập Kho", Properties.Resources.stockin_icon, "Stockin");
            sidebarControl.AddMenuItem("Xuất Kho", Properties.Resources.stockout_icon, "Stockout");
            sidebarControl.AddMenuItem("Nhân sự giao vận", Properties.Resources.station_icon, "Staffs");
            sidebarControl.AddMenuItem("Backup dữ liệu", Properties.Resources.payment_icon, "Backup");
            sidebarControl.AddMenuItem("Báo cáo", Properties.Resources.report_icon, "Reports");
            sidebarControl.AddMenuItem("Cài đặt hệ thống", Properties.Resources.setting_icon, "SystemSettings");
        }

        private void OnSidebarMenuClicked(object sender, string menuName)
        {
            // Sử dụng NavigationService để điều hướng
            AppServices.Navigation.NavigateTo(menuName);
        }

        /// <summary>
        /// Public method để các control khác có thể chuyển tab
        /// </summary>
        public void NavigateToTab(string menuName)
        {
            AppServices.Navigation.NavigateTo(menuName);
        }

        /// <summary>
        /// Chuyển tab và mở form tạo phiếu nhập kho
        /// </summary>
        public void NavigateToStockInAndOpenCreateForm()
        {
            AppServices.Navigation.NavigateTo("stockin");
            
            // Delay nhỏ để đảm bảo control đã được load và hiển thị
            System.Threading.Tasks.Task.Delay(100).ContinueWith(_ =>
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        // Tìm StockInManagementControl từ contentControl
                        foreach (Control ctrl in contentControl.Controls)
                        {
                            if (ctrl is StockInManagementControl stockInControl)
                            {
                                stockInControl.OpenCreateStockInForm();
                                return;
                            }
                        }
                    }));
                }
                else
                {
                    // Tìm StockInManagementControl từ contentControl
                    foreach (Control ctrl in contentControl.Controls)
                    {
                        if (ctrl is StockInManagementControl stockInControl)
                        {
                            stockInControl.OpenCreateStockInForm();
                            return;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Chuyển tab và mở form tạo phiếu xuất kho
        /// </summary>
        public void NavigateToStockOutAndOpenCreateForm()
        {
            AppServices.Navigation.NavigateTo("stockout");
            
            // Delay nhỏ để đảm bảo control đã được load và hiển thị
            System.Threading.Tasks.Task.Delay(100).ContinueWith(_ =>
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        // Tìm StockOutManagementControl từ contentControl
                        foreach (Control ctrl in contentControl.Controls)
                        {
                            if (ctrl is StockOutManagementControl stockOutControl)
                            {
                                stockOutControl.OpenCreateStockOutForm();
                                return;
                            }
                        }
                    }));
                }
                else
                {
                    // Tìm StockOutManagementControl từ contentControl
                    foreach (Control ctrl in contentControl.Controls)
                    {
                        if (ctrl is StockOutManagementControl stockOutControl)
                        {
                            stockOutControl.OpenCreateStockOutForm();
                            return;
                        }
                    }
                }
            });
        }
    }
}
