using System;
using System.Drawing;
using System.Windows.Forms;
// using EcoStationManagerApplication.UI.Forms; 

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class PackagingControl : UserControl
    {

        public PackagingControl()
        {
            InitializeComponent();

            SetupDataGridStyle(dgvPackaging);
            SetupDataGridStyle(dgvReuseStats);

            InitializeDataGridColumns();
            InitializeEvents();

            AddSamplePackagingData();
            AddSampleReuseData();
            CalculateRecoveryRate();
        }

        private void InitializeEvents()
        {
            if (btnAddPackaging != null)
                btnAddPackaging.Click += btnAddPackaging_Click;

            // Bạn có thể thêm các sự kiện dgv.CellFormatting ở đây nếu muốn
        }

        // Thêm cột cho các DataGridView (Designer không thể xử lý vòng lặp)
        private void InitializeDataGridColumns()
        {
            // --- Cột cho Bảng Tình trạng Bao bì ---
            var columnsPackaging = new[]
            {
                new { Name = "PackageCode", Header = "Mã bao bì", FillWeight = 15 },
                new { Name = "Type", Header = "Loại", FillWeight = 15 },
                new { Name = "Status", Header = "Tình trạng", FillWeight = 20 },
                new { Name = "UsageCount", Header = "Số lần sử dụng", FillWeight = 15 },
                new { Name = "StartDate", Header = "Ngày đưa vào sử dụng", FillWeight = 20 },
                new { Name = "Actions", Header = "Hành động", FillWeight = 15 }
            };

            foreach (var col in columnsPackaging)
            {
                dgvPackaging.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    DataPropertyName = col.Name,
                    ReadOnly = true
                });
            }

            // --- Cột cho Bảng Thống kê Tái sử dụng ---
            var columnsReuse = new[]
            {
                new { Name = "PackageType", Header = "Loại bao bì", FillWeight = 20 },
                new { Name = "AvgReuse", Header = "Số lần tái sử dụng TB", FillWeight = 25 },
                new { Name = "MostUsed", Header = "Bao bì sử dụng nhiều nhất", FillWeight = 35 },
                new { Name = "ReuseRate", Header = "Tỷ lệ tái sử dụng", FillWeight = 20 }
            };

            foreach (var col in columnsReuse)
            {
                dgvReuseStats.Columns.Add(new DataGridViewTextBoxColumn
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

        private void btnAddPackaging_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form thêm bao bì mới", "Thêm bao bì");
            // using (var addForm = new AddPackagingForm()) // Tên form giả định
            // {
            //     addForm.ShowDialog();
            // }
        }

        // --- HÀM LOGIC & DỮ LIỆU ---

        private void CalculateRecoveryRate()
        {
            try
            {
                // Dữ liệu mẫu
                int distributed = 156;
                int recovered = 132;

                if (distributed > 0)
                {
                    double rate = (double)recovered / distributed * 100;
                    txtDistributed.Text = distributed.ToString();
                    txtRecovered.Text = recovered.ToString();
                    txtRecoveryRate.Text = $"{rate:F1}%";
                }
                else
                {
                    txtDistributed.Text = "0";
                    txtRecovered.Text = "0";
                    txtRecoveryRate.Text = "N/A";
                }
            }
            catch (Exception)
            {
                txtRecoveryRate.Text = "Lỗi";
            }
        }

        private void AddSamplePackagingData()
        {
            dgvPackaging.Rows.Add("BB-00125", "Chai 500ml", "Mới", "0", "15/03/2025", "Sửa");
            dgvPackaging.Rows.Add("BB-00124", "Chai 1L", "Đang sử dụng", "3", "10/03/2025", "Sửa");
        }

        private void AddSampleReuseData()
        {
            dgvReuseStats.Rows.Add("Chai 500ml", "4.2 lần", "BB-00120 (12 lần)", "78%");
            dgvReuseStats.Rows.Add("Chai 1L", "3.8 lần", "BB-00115 (10 lần)", "72%");
        }

        // --- HÀM HELPER (Hàm phụ trợ) ---

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
    }
}