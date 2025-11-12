using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class DashboardControl : UserControl
    {
        public event EventHandler ViewAllOrdersClicked;

        public DashboardControl()
        {
            InitializeComponent();

            PopulateStatsPanel();
            InitializeDataGrid(); 
            if (btnViewAllOrders != null)
            {
                btnViewAllOrders.Click += btnViewAllOrders_Click;
            }
        }

        private void btnViewAllOrders_Click(object sender, EventArgs e)
        {
            ViewAllOrdersClicked?.Invoke(this, EventArgs.Empty);
        }

        // Đổ dữ liệu cho các thẻ thống kê
        private void PopulateStatsPanel()
        {
            if (statsPanel == null) return;

            var statsData = new[]
            {
                new { Label = "Đơn hàng hôm nay", Value = "12", Desc = "+2 so với hôm qua", IsEco = false },
                new { Label = "Doanh thu tháng", Value = "8.5M", Desc = "+15% so với tháng trước", IsEco = false },
                new { Label = "Tồn kho thấp", Value = "3", Desc = "Sản phẩm cần nhập", IsEco = false },
                new { Label = "Bao bì cần thu hồi", Value = "24", Desc = "Chai/lọ đang lưu hành", IsEco = false },
            };

            foreach (var stat in statsData)
            {
                var statCard = CreateStatCard(stat.Label, stat.Value, stat.Desc, stat.IsEco);
                statCard.Margin = new Padding(10);
                statCard.Size = new Size(170, 120);
                statsPanel.Controls.Add(statCard);
            }
        }

        private async void LoadDashboardData()
        {
            try
            {
                ShowLoading(true);

                // Load dữ liệu song song
                await Task.WhenAll(
                    LoadStatistics(),
                    LoadRecentOrders()
                );
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu dashboard");
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task LoadStatistics()
        {
            try
            {
                // Tổng đơn hàng hôm nay
                var todayOrdersResult = await AppServices.OrderService.GetTodayOrdersAsync();
                var todayOrderCount = todayOrdersResult.Success ? todayOrdersResult.Data.Count : 0;

                // Doanh thu tháng
                var monthlyRevenue = await CalculateMonthlyRevenue();

                // Sản phẩm tồn kho thấp
                var lowStockResult = await AppServices.InventoryService.GetLowStockItemsAsync();
                var lowStockCount = lowStockResult.Success ? lowStockResult.Data.Count() : 0;

                // Bao bì cần thu hồi
                var packagingInUse = await CalculatePackagingInUse();

                // Tác động môi trường (tính toán giả định)
                var environmentalImpact = await CalculateEnvironmentalImpact();

                var statsData = new[]
                {
                    new { Label = "Đơn hàng hôm nay", Value = todayOrderCount.ToString(), Desc = "Đơn hàng mới", IsEco = false },
                    new { Label = "Doanh thu tháng", Value = $"{monthlyRevenue:N0}", Desc = "VND", IsEco = false },
                    new { Label = "Tồn kho thấp", Value = lowStockCount.ToString(), Desc = "Sản phẩm cần nhập", IsEco = false },
                    new { Label = "Bao bì đang sử dụng", Value = packagingInUse.ToString(), Desc = "Chai/lọ lưu hành", IsEco = false },
                    new { Label = "CO2 tiết kiệm", Value = $"{environmentalImpact} kg", Desc = "Giảm phát thải", IsEco = true }
                };

                UpdateStatsPanel(statsData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tải thống kê: {ex.Message}");
            }
        }

        // Khởi tạo và đổ dữ liệu cho DataGridView
        private void InitializeDataGrid()
        {
            if (dgvRecentOrders == null) return;

            dgvRecentOrders.BackgroundColor = Color.White;
            dgvRecentOrders.BorderStyle = BorderStyle.None;
            dgvRecentOrders.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecentOrders.GridColor = Color.FromArgb(240, 240, 240);
            dgvRecentOrders.AllowUserToAddRows = false;
            dgvRecentOrders.AllowUserToDeleteRows = false;
            dgvRecentOrders.AllowUserToResizeRows = false;
            dgvRecentOrders.RowHeadersVisible = false;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRecentOrders.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRecentOrders.EnableHeadersVisualStyles = false;
            dgvRecentOrders.ColumnHeadersHeight = 40;
            dgvRecentOrders.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvRecentOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgvRecentOrders.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRecentOrders.RowTemplate.Height = 35;

            dgvRecentOrders.Columns.Add("MaDon", "Mã đơn");
            dgvRecentOrders.Columns.Add("KhachHang", "Khách hàng");
            dgvRecentOrders.Columns.Add("SanPham", "Sản phẩm");
            dgvRecentOrders.Columns.Add("Loai", "Loại");
            dgvRecentOrders.Columns.Add("TrangThai", "Trạng thái");
            dgvRecentOrders.Columns.Add("NgayTao", "Ngày tạo");
            dgvRecentOrders.Columns["SanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRecentOrders.Columns["KhachHang"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            string[,] data = {
                { "ORD-00125", "Nguyễn Văn A", "Dầu gội thiên nhiên 500ml", "Online", "Đang giao", "15/03/2025" },
                { "ORD-00124", "Trần Thị B", "Nước rửa chén 1L", "Offline", "Chuẩn bị", "15/03/2025" }
            };

            for (int row = 0; row < data.GetLength(0); row++)
            {
                dgvRecentOrders.Rows.Add(
                    data[row, 0], data[row, 1], data[row, 2],
                    data[row, 3], data[row, 4], data[row, 5]
                );
            }

            dgvRecentOrders.CellFormatting += dgvRecentOrders_CellFormatting;
        }

        // Sự kiện này sẽ tô màu cho các ô "Loại" và "Trạng thái"
        private void dgvRecentOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvRecentOrders.Columns[e.ColumnIndex].Name;

            if (colName == "Loai" || colName == "TrangThai")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.BackColor = GetBadgeColor(status);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private Panel CreateStatCard(string label, string value, string desc, bool isEco = false)
        {
            var card = new Panel();
            card.Size = new Size(170, 120);
            card.BackColor = isEco ? Color.FromArgb(232, 245, 233) : Color.White;
            card.BorderStyle = BorderStyle.None;
            card.Padding = new Padding(15);
            card.Margin = new Padding(5);

            var lblLabel = new Label();
            lblLabel.Text = label;
            lblLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblLabel.ForeColor = Color.FromArgb(100, 100, 100);
            lblLabel.AutoSize = true;
            lblLabel.Location = new Point(15, 15);

            var lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = isEco ? Color.FromArgb(46, 125, 50) : Color.FromArgb(25, 118, 210);
            lblValue.AutoSize = true;
            lblValue.Location = new Point(15, 40);

            var lblDesc = new Label();
            lblDesc.Text = desc;
            lblDesc.Font = new Font("Segoe UI", 9);
            lblDesc.ForeColor = Color.Gray;
            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(15, 85);

            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblDesc);

            return card;
        }

        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Online": return Color.FromArgb(187, 222, 251);
                case "Offline": return Color.FromArgb(224, 224, 224);
                case "Đang giao": return Color.FromArgb(200, 230, 201);
                case "Chuẩn bị": return Color.FromArgb(255, 249, 196);
                default: return Color.LightGray;
            }
        }
    }
}