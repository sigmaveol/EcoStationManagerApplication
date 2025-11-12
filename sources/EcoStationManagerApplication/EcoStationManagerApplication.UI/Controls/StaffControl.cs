using System;
using System.Drawing;
using System.Windows.Forms;
// using EcoStationManagerApplication.UI.Forms; s

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl : UserControl
    {

        public StaffControl()
        {
            InitializeComponent();

            SetupDataGridStyle(dgvAssignments);
            SetupDataGridStyle(dgvKPI);

            InitializeDataGridColumns();
            InitializeEvents();

            AddSampleAssignmentData();
            AddSampleKpiData();
        }

        // Gán tất cả sự kiện ở đây
        private void InitializeEvents()
        {
            if (btnAddStaff != null)
                btnAddStaff.Click += btnAddStaff_Click;

            if (dgvAssignments != null)
                dgvAssignments.CellFormatting += dgvAssignments_CellFormatting;

            if (dgvKPI != null)
                dgvKPI.CellFormatting += dgvKPI_CellFormatting;
        }

        // Thêm cột cho các DataGridView (Designer không thể xử lý vòng lặp)
        private void InitializeDataGridColumns()
        {
            // --- Cột cho Bảng Phân công ---
            var columnsAssignments = new[]
            {
                new { Name = "StaffName", Header = "Nhân viên", FillWeight = 15 },
                new { Name = "Position", Header = "Chức vụ", FillWeight = 15 },
                new { Name = "Shift", Header = "Ca làm", FillWeight = 15 },
                new { Name = "Assignment", Header = "Công việc được phân công", FillWeight = 25 },
                new { Name = "Status", Header = "Trạng thái", FillWeight = 15 },
                new { Name = "Actions", Header = "Hành động", FillWeight = 15 }
            };

            foreach (var col in columnsAssignments)
            {
                dgvAssignments.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    DataPropertyName = col.Name,
                    ReadOnly = true
                });
            }

            // --- Cột cho Bảng KPI ---
            var columnsKPI = new[]
            {
                new { Name = "StaffName", Header = "Nhân viên", FillWeight = 20 },
                new { Name = "Month", Header = "Tháng", FillWeight = 10 },
                new { Name = "CompletedOrders", Header = "Số đơn hoàn thành", FillWeight = 20 },
                new { Name = "Revenue", Header = "Doanh thu", FillWeight = 15 },
                new { Name = "Rating", Header = "Đánh giá", FillWeight = 15 },
                new { Name = "KPI", Header = "KPI", FillWeight = 10 }
            };

            foreach (var col in columnsKPI)
            {
                dgvKPI.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    DataPropertyName = col.Name,
                    ReadOnly = true
                });
            }
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form thêm nhân viên mới", "Thêm nhân viên");
            // using (var addStaffForm = new AddStaffForm()) // Tên form giả định
            // {
            //     addStaffForm.ShowDialog();
            // }
        }

        // --- HÀM LOGIC & DỮ LIỆU ---

        private void AddSampleAssignmentData()
        {
            dgvAssignments.Rows.Add(
                "Nguyễn Văn A",
                "Nhân viên giao hàng",
                "Sáng (8h-12h)",
                "Giao 5 đơn hàng khu vực Q1",
                "Đang giao",
                "Chi tiết | Phân công"
            );
            dgvAssignments.Rows.Add(
                "Trần Thị B",
                "Nhân viên refill",
                "Chiều (13h-17h)",
                "Chuẩn bị 10 đơn hàng",
                "Đang làm",
                "Chi tiết | Phân công"
            );
        }

        private void AddSampleKpiData()
        {
            dgvKPI.Rows.Add("Nguyễn Văn A", "03/2025", "45", "12.5M", "Tốt", "95%");
            dgvKPI.Rows.Add("Trần Thị B", "03/2025", "38", "10.2M", "Khá", "88%");
        }

        // --- CÁC HÀM HELPER (Hàm phụ trợ) ---

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

        // Hàm tô màu cho các ô
        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Đang giao": return Color.FromArgb(200, 230, 201); // Xanh lá
                case "Đang làm": return Color.FromArgb(187, 222, 251); // Xanh dương
                case "Tốt": return Color.Green;
                case "Khá": return Color.Orange;
                default: return Color.LightGray;
            }
        }

        private void dgvAssignments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAssignments.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.BackColor = GetBadgeColor(status);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void dgvKPI_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKPI.Columns[e.ColumnIndex].Name == "Rating")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.ForeColor = GetBadgeColor(status);
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }
    }
}