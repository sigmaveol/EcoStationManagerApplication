using System;
using System.Drawing;
using System.Windows.Forms;
namespace EcoStationManagerApplication.UI.Controls
{
    public partial class OrdersControl : UserControl
    {
        public OrdersControl()
        {
            InitializeComponent();

            SetupDataGridStyle(dgvOrders);

            PopulateTabPanel();
            InitializeDataGridColumns();
            AddSampleOrderData();

            InitializeEvents();
        }

        // Gán tất cả sự kiện ở đây
        private void InitializeEvents()
        {
            if (btnExportPDF != null)
                btnExportPDF.Click += btnExportPDF_Click;

            if (btnExportExcel != null)
                btnExportExcel.Click += btnExportExcel_Click;

            if (btnAddOrder != null)
                btnAddOrder.Click += btnAddOrder_Click;

            if (dgvOrders != null)
            {
                dgvOrders.CellContentClick += dgvOrders_CellContentClick;
                dgvOrders.CellFormatting += dgvOrders_CellFormatting;
            }
        }

        // Đổ các nút Tab vào FlowLayoutPanel
        private void PopulateTabPanel()
        {
            if (tabPanel == null) return;

            string[] tabs = { "Tất cả", "Đơn Online", "Đơn Offline", "Mới", "Chuẩn bị", "Đang giao", "Hoàn thành", "Thu hồi bao bì" };

            foreach (string tab in tabs)
            {
                var tabButton = new Button();
                tabButton.Text = tab;
                tabButton.Size = new Size(100, 34);
                tabButton.Margin = new Padding(3); 
                tabButton.FlatStyle = FlatStyle.Flat;
                tabButton.FlatAppearance.BorderSize = 0;
                tabButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                // Nút đầu tiên (Tất cả) được chọn
                if (tab == "Tất cả")
                {
                    tabButton.BackColor = Color.FromArgb(46, 125, 50);
                    tabButton.ForeColor = Color.White;
                }
                else
                {
                    tabButton.BackColor = Color.White;
                    tabButton.ForeColor = Color.Black;
                }

                tabButton.Click += contentTab_Click;
                tabPanel.Controls.Add(tabButton);
            }
        }

        // Thêm cột vào DataGridView
        private void InitializeDataGridColumns()
        {
            if (dgvOrders == null) return;

            var columns = new[]
            {
                new { Name = "OrderCode", Header = "Mã đơn" },
                new { Name = "Customer", Header = "Khách hàng" },
                new { Name = "Product", Header = "Sản phẩm" },
                new { Name = "Type", Header = "Loại" },
                new { Name = "Quantity", Header = "Số lượng" },
                new { Name = "Status", Header = "Trạng thái" },
                new { Name = "CreatedDate", Header = "Ngày tạo" },
            };

            foreach (var col in columns)
            {
                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    ReadOnly = true,
                    DataPropertyName = col.Name
                });
            }

            dgvOrders.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colDetail",
                HeaderText = "Chi tiết",
                Text = "Chi tiết",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvOrders.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colUpdate",
                HeaderText = "Cập nhật",
                Text = "Cập nhật",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // Tự co giãn cột
            dgvOrders.Columns["Product"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvOrders.Columns["Customer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Thêm dữ liệu mẫu
        private void AddSampleOrderData()
        {
            dgvOrders.Rows.Add(
                "ORD-00125", "Nguyễn Văn A", "Dầu gội thiên nhiên 500ml",
                "Online", "2", "Đang giao", "15/03/2025"
            );
            dgvOrders.Rows.Add(
                "ORD-00124", "Trần Thị B", "Nước rửa chén 1L",
                "Offline", "1", "Chuẩn bị", "15/03/2025"
            );
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xuất PDF đơn hàng", "Xuất PDF");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xuất Excel đơn hàng", "Xuất Excel");
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form Thêm Đơn Hàng");
        }

        // Xử lý khi nhấn vào Tab
        private void contentTab_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                // Reset tất cả các nút
                foreach (Control control in tabPanel.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;
                    }
                }
                // Highlight nút được chọn
                button.BackColor = Color.FromArgb(46, 125, 50);
                button.ForeColor = Color.White;
                MessageBox.Show($"Lọc theo: {button.Text}", "Chuyển tab");
            }
        }

        // Xử lý khi nhấn nút trên DataGridView
        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;
            string orderId = dgvOrders.Rows[e.RowIndex].Cells["OrderCode"].Value.ToString();

            if (colName == "colDetail")
            {
                MessageBox.Show($"Mở chi tiết cho đơn hàng: {orderId}", "Xem Chi tiết");
            }
            else if (colName == "colUpdate")
            {
                MessageBox.Show($"Cập nhật trạng thái cho đơn hàng: {orderId}", "Cập nhật");
            }
        }

        // --- HÀM HELPER (Hàm phụ trợ) ---

        // Hàm tô màu cho các ô
        private void dgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvOrders.Columns[e.ColumnIndex].Name;
            if (colName == "Status" || colName == "Type")
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

        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Online": return Color.FromArgb(187, 222, 251);
                case "Offline": return Color.FromArgb(224, 224, 224);
                case "Đang giao": return Color.FromArgb(200, 230, 201);
                case "Chuẩn bị": return Color.FromArgb(255, 249, 196);
                case "Mới": return Color.FromArgb(209, 196, 233); // Thêm màu
                case "Hoàn thành": return Color.FromArgb(232, 234, 237); // Thêm màu
                default: return Color.LightGray;
            }
        }

        // Hàm áp dụng style chung cho DataGridView
        private void SetupDataGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(240, 240, 240);
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;
        }

        private void dgvOrders_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}