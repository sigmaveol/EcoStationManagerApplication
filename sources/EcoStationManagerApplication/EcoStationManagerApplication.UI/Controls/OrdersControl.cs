using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Helpers;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class OrdersControl : UserControl
    {
        private List<Order> orders;
        private List<Customer> customers;
        private List<Station> stations;
        private string searchTerm = "";
        private string statusFilter = "all";

        public OrdersControl()
        {
            InitializeComponent();
            LoadData();
            InitializeControls();
        }

        private void LoadData()
        {
            // Load mock data
            orders = MockData.GetOrders();
            customers = MockData.GetCustomers();
            stations = MockData.GetStations();
        }

        private void InitializeControls()
        {
            // Initialize DataGridView columns
            InitializeDataGridView();

            // Initialize status filter
            comboBoxStatus.Items.AddRange(new object[] {
                "Tất cả", "Nháp", "Đã xác nhận", "Đang xử lý", "Đã giao", "Hoàn thành", "Đã hủy"
            });
            comboBoxStatus.SelectedIndex = 0;

            var statCards = new List<Control> { guna2Panel2, guna2Panel3, guna2Panel4, guna2Panel5 };
            ResponsiveLayoutHelper.SetupResponsiveFlowPanel(
                flowLayoutPanelStats,
                statCards,
                new Size(270, 70),  // Kích thước mặc định
                new Size(200, 70),  // Kích thước tối thiểu
                10, 10              // Khoảng cách ngang/dọc
            );

            // Bind data
            BindData();
            UpdateStatistics();
        }

        private void InitializeDataGridView()
        {
            // Clear existing columns
            dataGridViewOrders.Columns.Clear();

            // Add columns
            dataGridViewOrders.Columns.Add("OrderId", "Mã đơn");
            dataGridViewOrders.Columns.Add("Source", "Nguồn");
            dataGridViewOrders.Columns.Add("Customer", "Khách hàng");
            dataGridViewOrders.Columns.Add("Station", "Trạm xử lý");
            dataGridViewOrders.Columns.Add("TotalAmount", "Tổng tiền");
            dataGridViewOrders.Columns.Add("Discount", "Giảm giá");
            dataGridViewOrders.Columns.Add("FinalAmount", "Thành tiền");
            dataGridViewOrders.Columns.Add("Status", "Trạng thái");
            dataGridViewOrders.Columns.Add("CreatedDate", "Ngày tạo");
            dataGridViewOrders.Columns.Add("Action", "Thao tác");

            // Set column properties
            dataGridViewOrders.Columns["OrderId"].Width = 100;
            dataGridViewOrders.Columns["Source"].Width = 120;
            dataGridViewOrders.Columns["Customer"].Width = 150;
            dataGridViewOrders.Columns["Station"].Width = 120;
            dataGridViewOrders.Columns["TotalAmount"].Width = 120;
            dataGridViewOrders.Columns["Discount"].Width = 100;
            dataGridViewOrders.Columns["FinalAmount"].Width = 120;
            dataGridViewOrders.Columns["Status"].Width = 120;
            dataGridViewOrders.Columns["CreatedDate"].Width = 100;
            dataGridViewOrders.Columns["Action"].Width = 80;

            // Set alignment
            dataGridViewOrders.Columns["TotalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewOrders.Columns["Discount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewOrders.Columns["FinalAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewOrders.Columns["Action"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set number format
            dataGridViewOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            dataGridViewOrders.Columns["Discount"].DefaultCellStyle.Format = "N0";
            dataGridViewOrders.Columns["FinalAmount"].DefaultCellStyle.Format = "N0";
        }

        private void BindData()
        {
            try
            {
                var filteredOrders = orders.Where(order =>
                {
                    var matchStatus = statusFilter == "all" || GetStatusText(order.Status) == statusFilter;
                    var matchSearch = order.OrderId.ToString().Contains(searchTerm);
                    return matchStatus && matchSearch;
                }).ToList();

                dataGridViewOrders.Rows.Clear();

                foreach (var order in filteredOrders)
                {
                    int rowIndex = dataGridViewOrders.Rows.Add(
                        $"ORD{order.OrderId.ToString().PadLeft(5, '0')}",
                        GetSourceText(order.Source),
                        GetCustomerName(order.CustomerId),
                        GetStationName(order.StationId),
                        order.TotalAmount,
                        order.DiscountedAmount,
                        order.TotalAmount - order.DiscountedAmount,
                        GetStatusText(order.Status),
                        order.CreatedDate.ToString("dd/MM/yyyy"),
                        "👁️" // Eye icon for view
                    );

                    // Set status color
                    var statusColor = GetStatusColor(order.Status);
                    dataGridViewOrders.Rows[rowIndex].Cells["Status"].Style.ForeColor = statusColor;

                    // Set discount color to red
                    dataGridViewOrders.Rows[rowIndex].Cells["Discount"].Style.ForeColor = Color.Red;
                    dataGridViewOrders.Rows[rowIndex].Cells["Discount"].Value = "-" + order.DiscountedAmount.ToString("N0") + "₫";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            lblTotalOrders.Text = orders.Count.ToString();
            lblPendingOrders.Text = orders.Count(o => o.Status == "draft" || o.Status == "confirmed").ToString();
            lblProcessingOrders.Text = orders.Count(o => o.Status == "processing" || o.Status == "ready").ToString();
            lblCompletedOrders.Text = orders.Count(o => o.Status == "completed").ToString();
        }

        private string GetCustomerName(int customerId)
        {
            return customers.FirstOrDefault(c => c.CustomerId == customerId)?.Name ?? "N/A";
        }

        private string GetStationName(int? stationId)
        {
            if (!stationId.HasValue) return "N/A";
            return stations.FirstOrDefault(s => s.StationId == stationId.Value)?.Name ?? "N/A";
        }

        private string GetStatusText(string status)
        {
            var statusMap = new Dictionary<string, string>
            {
                ["draft"] = "Nháp",
                ["confirmed"] = "Đã xác nhận",
                ["processing"] = "Đang xử lý",
                ["ready"] = "Sẵn sàng",
                ["shipped"] = "Đã giao",
                ["completed"] = "Hoàn thành",
                ["cancelled"] = "Đã hủy",
                ["returned"] = "Trả hàng"
            };
            return statusMap.ContainsKey(status) ? statusMap[status] : status;
        }

        private string GetSourceText(string source)
        {
            var sourceMap = new Dictionary<string, string>
            {
                ["googleform"] = "Google Form",
                ["excel"] = "Excel",
                ["email"] = "Email",
                ["manual"] = "Thủ công",
                ["other"] = "Khác"
            };
            return sourceMap.ContainsKey(source) ? sourceMap[source] : source;
        }

        private Color GetStatusColor(string status)
        {
            var colorMap = new Dictionary<string, Color>
            {
                ["draft"] = Color.Gray,
                ["confirmed"] = Color.Orange,
                ["processing"] = Color.Blue,
                ["ready"] = Color.Teal,
                ["shipped"] = Color.Purple,
                ["completed"] = Color.Green,
                ["cancelled"] = Color.Red,
                ["returned"] = Color.Red
            };
            return colorMap.ContainsKey(status) ? colorMap[status] : Color.Gray;
        }

        #region Event Handlers
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTerm = txtSearch.Text;
            BindData();
        }

        private void comboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var statusMap = new Dictionary<string, string>
            {
                ["Tất cả"] = "all",
                ["Nháp"] = "Nháp",
                ["Đã xác nhận"] = "Đã xác nhận",
                ["Đang xử lý"] = "Đang xử lý",
                ["Đã giao"] = "Đã giao",
                ["Hoàn thành"] = "Hoàn thành",
                ["Đã hủy"] = "Đã hủy"
            };

            statusFilter = statusMap[comboBoxStatus.SelectedItem.ToString()];
            BindData();
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            // Import Excel functionality
            MessageBox.Show("Chức năng Import Excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            // Create order functionality
            MessageBox.Show("Tạo đơn hàng mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridViewOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridViewOrders.Columns["Action"].Index)
            {
                // View order details
                var orderId = dataGridViewOrders.Rows[e.RowIndex].Cells["OrderId"].Value.ToString();
                MessageBox.Show($"Xem chi tiết đơn hàng: {orderId}", "Chi tiết đơn hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImportExcel_MouseEnter(object sender, EventArgs e)
        {
            btnImportExcel.FillColor = Color.FromArgb(240, 240, 240);
        }

        private void btnImportExcel_MouseLeave(object sender, EventArgs e)
        {
            btnImportExcel.FillColor = Color.White;
        }

        private void btnCreateOrder_MouseEnter(object sender, EventArgs e)
        {
            btnCreateOrder.FillColor = Color.FromArgb(33, 140, 73);
        }

        private void btnCreateOrder_MouseLeave(object sender, EventArgs e)
        {
            btnCreateOrder.FillColor = Color.FromArgb(31, 107, 59);
        }
        #endregion

        private void OrdersControl_Load(object sender, EventArgs e)
        {

        }
    }

    #region Data Models
    public class Order
    {
        public int OrderId { get; set; }
        public string Source { get; set; }
        public int CustomerId { get; set; }
        public int? StationId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
    }

    public class Station
    {
        public int StationId { get; set; }
        public string Name { get; set; }
    }

    public static class MockData
    {
        public static List<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order {
                    OrderId = 1,
                    Source = "googleform",
                    CustomerId = 1,
                    StationId = 1,
                    TotalAmount = 1000000,
                    DiscountedAmount = 100000,
                    Status = "completed",
                    CreatedDate = DateTime.Now
                },
                new Order {
                    OrderId = 2,
                    Source = "excel",
                    CustomerId = 2,
                    StationId = 2,
                    TotalAmount = 1500000,
                    DiscountedAmount = 150000,
                    Status = "processing",
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new Order {
                    OrderId = 3,
                    Source = "manual",
                    CustomerId = 3,
                    StationId = 1,
                    TotalAmount = 800000,
                    DiscountedAmount = 80000,
                    Status = "confirmed",
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new Order {
                    OrderId = 4,
                    Source = "email",
                    CustomerId = 1,
                    StationId = null,
                    TotalAmount = 1200000,
                    DiscountedAmount = 120000,
                    Status = "draft",
                    CreatedDate = DateTime.Now.AddDays(-3)
                }
            };
        }

        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer { CustomerId = 1, Name = "Nguyễn Văn A" },
                new Customer { CustomerId = 2, Name = "Trần Thị B" },
                new Customer { CustomerId = 3, Name = "Lê Văn C" },
                new Customer { CustomerId = 4, Name = "Phạm Thị D" }
            };
        }

        public static List<Station> GetStations()
        {
            return new List<Station>
            {
                new Station { StationId = 1, Name = "Trạm Hà Nội" },
                new Station { StationId = 2, Name = "Trạm Hồ Chí Minh" },
                new Station { StationId = 3, Name = "Trạm Đà Nẵng" }
            };
        }
    }
    #endregion
}