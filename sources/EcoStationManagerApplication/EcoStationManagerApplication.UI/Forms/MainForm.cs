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
        private UserControl currentContent;

        public MainForm()
        {
            InitializeComponent();
        }

        private void SetupEventHandlers()
        {
            if (sidebarControl != null)
            {
                sidebarControl.MenuClicked += OnMenuClicked;
            }
        }

        private void OnMenuClicked(object sender, string menuName)
        {
            // menuName chính là "dashboard", "reports", "devices"... 
            // mà bạn đã truyền khi gọi AddMenuItem
            ShowContent(menuName);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeSidebar();
            InitializeSidebarMenu();
            ShowDefaultContent();

            

            //sidebarControl.MenuClicked += SidebarControl_MenuClicked;
            ThemeManager.ApplyTheme(this);

            //TogglerSidebar.HandleResponsive(this, sidebarControl);
        }

        private void InitializeSidebar()
        {
            // Đăng ký sự kiện từ SidebarControl - QUAN TRỌNG!
            sidebarControl.MenuClicked += OnSidebarMenuClicked;

            // Thiết lập thông tin cho sidebar
            sidebarControl.SetAppName("Eco Station");
            sidebarControl.SetUsername("Hoàng Sinh Hùng");
             sidebarControl.SetAvatar(Properties.Resources.admin_avatar);
            //sidebarControl.SetLogo(Properties.Resources.app_logo);

        }

        private void InitializeSidebarMenu() 
        {
            sidebarControl.ClearMenuItems();
            // Thêm các menu item - sử dụng icon từ resources
            sidebarControl.AddMenuItem("Dashboard", Properties.Resources.dashboard_icon, "dashboard");
            sidebarControl.AddMenuItem("Đơn hàng", Properties.Resources.order_icon, "Orders");
            sidebarControl.AddMenuItem("Sản phẩm && Phiên bản", Properties.Resources.product_icon, "Products");
            sidebarControl.AddMenuItem("Nhà cung cấp", Properties.Resources.supplier_icon, "Suppliers");
            sidebarControl.AddMenuItem("Khách hàng", Properties.Resources.customer_icon, "Customers");
            sidebarControl.AddMenuItem("Tồn Kho", Properties.Resources.inventory_icon, "Inventory");
            sidebarControl.AddMenuItem("Nhập Kho", Properties.Resources.stockout_icon, "Stockout");
            sidebarControl.AddMenuItem("Xuất Kho", Properties.Resources.stockin_icon, "Stockin");
            sidebarControl.AddMenuItem("Trạm && Bồn", Properties.Resources.station_icon, "Stations");
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
            // Xóa content hiện tại
            contentControl.Controls.Clear();

            // Tạo UserControl tương ứng
            currentContent = CreateUserControlForMenu(menuName);

            if (currentContent != null)
            {
                currentContent.Dock = DockStyle.Fill;
                contentControl.Controls.Add(currentContent);
            }
            

            // Cập nhật visibility của các control khác
            UpdateControlVisibility(menuName);
            
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
                        return new StockInControl();
                    case "stockout":
                        return new StockOutControl();
                    case "stations":
                        return new StationsControl();
                    case "payment":
                        return new PaymentControl();
                    case "reports":
                        return new ReportsControl();
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

        private void UpdateControlVisibility(string menuName)
        {

            // Hiển thị search control chỉ trên một số trang cụ thể
            bool showSearch = false;
            string placeholderText = "Tìm kiếm...";

            switch (menuName.ToLower())
            {
                case "dashboard":
                    showSearch = true;
                    placeholderText = "Tìm kiếm thông tin...";
                    break;
                case "orders":
                    showSearch = true;
                    placeholderText = "Tìm kiếm đơn hàng...";
                    break;
                case "products":
                    showSearch = true;
                    placeholderText = "Tìm kiếm sản phẩm...";
                    break;
                case "suppliers":
                    showSearch = true;
                    placeholderText = "Tìm kiếm nhà cung cấp...";
                    break;
                case "customers":
                    showSearch = true;
                    placeholderText = "Tìm kiếm khách hàng...";
                    break;
                case "inventory":
                    showSearch = true;
                    placeholderText = "Tìm kiếm tồn kho...";
                    break;
                case "stockin":
                    showSearch = true;
                    placeholderText = "Tìm kiếm phiếu nhập...";
                    break;
                case "stockout":
                    showSearch = true;
                    placeholderText = "Tìm kiếm phiếu xuất...";
                    break;
                case "stations":
                    showSearch = true;
                    placeholderText = "Tìm kiếm trạm/bồn...";
                    break;
                case "payment":
                    showSearch = true;
                    placeholderText = "Tìm kiếm thanh toán...";
                    break;
                case "reports":
                    showSearch = true;
                    placeholderText = "Tìm kiếm báo cáo...";
                    break;
                case "systemsettings":
                    showSearch = false; // Trang cài đặt có thể không cần search
                    placeholderText = "Tìm kiếm cài đặt...";
                    break;
                default:
                    showSearch = false;
                    break;
            }

            headerControl.SearchTextBox.Visible = showSearch;
            headerControl.SearchPlaceHoder = placeholderText;
        }

        private void ShowDefaultContent()
        {
            ShowContent("dashboard");
        }

    }
}
