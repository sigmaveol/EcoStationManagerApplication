using System;
using System.Drawing;
using System.Windows.Forms;
using EcoStationManagerApplication.UI.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class InventoryControl : UserControl
    {

        public InventoryControl()
        {
            InitializeComponent();

            SetupDataGridStyle(dgvProducts);
            SetupDataGridStyle(dgvHistory);

            InitializeDataGridColumns();

            InitializeEvents();

            AddSampleProductData();
            AddSampleHistoryData();
            UpdateAlertCount();
        }

        private void InitializeEvents()
        {
            if (btnAddInventory != null)
                btnAddInventory.Click += btnAddInventory_Click;

            if (dgvProducts != null)
                dgvProducts.CellContentClick += dgvProducts_CellContentClick;

            if (dgvProducts != null)
                dgvProducts.CellFormatting += dgvProducts_CellFormatting;

            if (dgvHistory != null)
                dgvHistory.CellFormatting += dgvHistory_CellFormatting;
        }

        // Hàm này chứa các vòng lặp (foreach) mà Designer không hiểu
        private void InitializeDataGridColumns()
        {
            // --- Thêm cột cho Bảng Sản phẩm ---
            var columnsProducts = new (string Name, string Header, int FillWeight)[]
            {
                ("Product", "Sản phẩm", 25),
                ("SKU", "Mã SKU", 10),
                ("Stock", "Tồn kho", 10),
                ("AlertLevel", "Mức cảnh báo", 10),
                ("Price", "Giá bán", 10),
                ("Status", "Trạng thái", 15)
            };

            foreach (var col in columnsProducts)
            {
                this.dgvProducts.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    ReadOnly = true,
                    DataPropertyName = col.Name
                });
            }

            var btnEditCol = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            var btnStockInCol = new DataGridViewButtonColumn
            {
                Name = "colStockIn",
                HeaderText = "",
                Text = "Nhập kho",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            var btnStockOutCol = new DataGridViewButtonColumn
            {
                Name = "colStockOut",
                HeaderText = "",
                Text = "Xuất kho",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            this.dgvProducts.Columns.Add(btnEditCol);
            this.dgvProducts.Columns.Add(btnStockInCol);
            this.dgvProducts.Columns.Add(btnStockOutCol);

            // --- Thêm cột cho Bảng Lịch sử ---
            var columnsHistory = new (string Name, string Header, int FillWeight)[]
            {
                ("Date", "Ngày", 15),
                ("Type", "Loại", 10),
                ("Product", "Sản phẩm", 25),
                ("Quantity", "Số lượng", 10),
                ("Person", "Người thực hiện", 20),
                ("Note", "Ghi chú", 20)
            };

            foreach (var col in columnsHistory)
            {
                this.dgvHistory.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    ReadOnly = true,
                    DataPropertyName = col.Name
                });
            }
        }


        private void btnAddInventory_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddProductForm())
            {
                addForm.ShowDialog();
            }
        }

        private void UpdateAlertCount()
        {
            if (lblAlertCount != null)
            {
                lblAlertCount.Text = "3"; 
            }
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string sku = dgvProducts.Rows[e.RowIndex].Cells["SKU"].Value?.ToString();
            if (string.IsNullOrEmpty(sku)) return;
            string colName = dgvProducts.Columns[e.ColumnIndex].Name;

            if (colName == "colEdit")
            {
                using (var editForm = new AddProductForm())
                {
                    editForm.ShowDialog();
                }
            }
            else if (colName == "colStockIn")
            {
                using (var stockForm = new StockMovementForm(sku, StockMovementForm.MovementType.StockIn))
                {
                    stockForm.ShowDialog();
                }
            }
            else if (colName == "colStockOut")
            {
                using (var stockForm = new StockMovementForm(sku, StockMovementForm.MovementType.StockOut))
                {
                    stockForm.ShowDialog();
                }
            }
        }

        // --- Các hàm đổ dữ liệu mẫu ---

        private void AddSampleProductData()
        {
            if (dgvProducts == null) return;
            dgvProducts.Rows.Add(
                "Dầu gội thiên nhiên 500ml", "DG-001", "15", "10", "120.000đ", "Còn hàng"
            );
            dgvProducts.Rows.Add(
                "Sữa tắm thảo dược 500ml", "ST-002", "5", "10", "150.000đ", "Sắp hết"
            );
        }

        private void AddSampleHistoryData()
        {
            if (dgvHistory == null) return;
            dgvHistory.Rows.Add(
                "15/03/2025", "Nhập kho", "Dầu gội thiên nhiên 500ml", "20", "Nguyễn Văn A", "Nhập hàng từ NCC"
            );
            dgvHistory.Rows.Add(
                "14/03/2025", "Xuất kho", "Sữa tắm thảo dược 500ml", "5", "Trần Thị B", "Xuất cho đơn hàng ORD-00123"
            );
        }

        // --- CÁC HÀM HELPER (Hàm phụ trợ) ---
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

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Sắp hết")
                    {
                        e.CellStyle.BackColor = Color.FromArgb(255, 248, 225); // Vàng nhạt
                        e.CellStyle.ForeColor = Color.FromArgb(186, 104, 0); // Vàng đậm
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                    else if (status == "Còn hàng")
                    {
                        e.CellStyle.BackColor = Color.FromArgb(232, 245, 233); // Xanh lá nhạt
                        e.CellStyle.ForeColor = Color.FromArgb(27, 94, 32); // Xanh lá đậm
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvHistory.Columns[e.ColumnIndex].Name == "Type")
            {
                if (e.Value != null)
                {
                    string type = e.Value.ToString();
                    if (type == "Nhập kho")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if (type == "Xuất kho")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }
    }
}