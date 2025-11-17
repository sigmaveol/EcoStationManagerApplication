using EcoStationManagerApplication.UI.Common;
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

        private Dictionary<string, UserControl> _contentCache = new Dictionary<string, UserControl>();

        public MainForm()
        {
            InitializeComponent();
        }

        //private void SetupEventHandlers()
        //{
        //    if (sidebarControl != null)
        //    {
        //        sidebarControl.MenuClicked += OnMenuClicked;
        //    }
        //}

        //private void OnMenuClicked(object sender, string menuName)
        //{
        //    // menuName chính là "dashboard", "reports", "devices"...
        //    ShowContent(menuName);
        //}



        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeSidebar();
            InitializeSidebarMenu();
            ShowContent("dashboard");

            //sidebarControl.MenuClicked += SidebarControl_MenuClicked;

            //TogglerSidebar.HandleResponsive(this, sidebarControl);
        }

        private void InitializeSidebar()
        {
            // Đăng ký sự kiện từ SidebarControl
            sidebarControl.MenuClicked += OnSidebarMenuClicked;

            // Thiết lập thông tin cho sidebar
            sidebarControl.SetAppName("Eco Station");
            sidebarControl.SetUsername("Hoàng Sinh Hùng");
             sidebarControl.SetAvatar(Properties.Resources.logo_pm);
            //sidebarControl.SetLogo(Properties.Resources.app_logo);

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
            //sidebarControl.AddMenuItem("Trạm && Bồn", Properties.Resources.station_icon, "Stations");
            sidebarControl.AddMenuItem("Thanh Toán", Properties.Resources.payment_icon, "Payment");
            sidebarControl.AddMenuItem("Báo cáo", Properties.Resources.report_icon, "Reports");
            sidebarControl.AddMenuItem("Cài đặt hệ thống", Properties.Resources.setting_icon, "SystemSettings");
        }

        private void OnSidebarMenuClicked(object sender, string menuName)
        {
            // Xử lý khi menu được click
            ShowContent(menuName);

        }

        private void ShowContent(string menuName)
        {
            contentControl.Controls.Clear();

            var key = menuName.ToLower();
            if (!_contentCache.TryGetValue(key, out var ctrl))
            {
                // Tạo mới lần đầu
                ctrl = CreateUserControlForMenu(key);
                ctrl.Dock = DockStyle.Fill;

                // Nếu là DashboardControl, đăng ký event ViewAllOrders
                if (ctrl is DashboardControl dashboard)
                {
                    dashboard.ViewAllOrdersClicked += (s, e) => ShowContent("orders");
                    // nếu có thêm event khác, cũng đăng ký ở đây
                }

                _contentCache[key] = ctrl; // lưu vào cache
            }

            contentControl.Controls.Add(ctrl);

        }


        private UserControl CreateUserControlForMenu(string menuName)
        {
            try
            {
                Console.WriteLine($"Attempting to create control for: {menuName}");

                switch (menuName.ToLower())
                {
                    case "dashboard":
                        return new DashboardControl();
                    case "orders":
                        return new OrdersControl();
                    case "products":
                        return new ProductsControl();
                    case "suppliers":
                        return new SuppliersControl();
                    case "customers":
                        return new CustomersControl();
                    case "inventory":
                        return new InventoryControl();
                    case "stockin":
                        return new StockInManagementControl();
                    case "stockout":
                        return new StockOutManagementControl();
                    case "stations":
                        return new StationsControl();
                    case "payment":
                        return new PaymentControl();
                    case "reports":
                        return new ReportControl();
                    case "systemsettings":
                        return new SystemSettingsControl();
                    default:
                        return new DashboardControl();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR creating control: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                // Trả về một UserControl đơn giản để không bị crash
                return CreateFallbackControl($"Error: {ex.Message}");
            }
        }

        private UserControl CreateFallbackControl(string message)
        {
            var panel = new UserControl();
            panel.BackColor = Color.LightCoral;
            panel.Dock = DockStyle.Fill;

            var label = new Label();
            label.Text = message;
            label.AutoSize = true;
            label.Location = new Point(20, 20);
            label.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label.ForeColor = Color.DarkRed;

            panel.Controls.Add(label);
            return panel;
        }
    }
}
