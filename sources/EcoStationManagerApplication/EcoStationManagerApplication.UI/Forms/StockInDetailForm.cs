using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class StockInDetailForm : Form
    {
        private StockInDetail _detail;
        private string _batchNo;
        private List<StockInDetail> _batchItems;

        // Constructor cho single detail (backward compatibility)
        public StockInDetailForm(StockInDetail detail)
        {
            _detail = detail;
            InitializeComponent();
            InitializeForm();
        }

        // Constructor mới cho batch với nhiều sản phẩm
        public StockInDetailForm(string batchNo, List<StockInDetail> batchItems)
        {
            _batchNo = batchNo;
            _batchItems = batchItems ?? new List<StockInDetail>();
            _detail = _batchItems.FirstOrDefault();
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Chi tiết lô hàng nhập kho";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(900, 600);

            LoadDetailData();
        }

        private void LoadDetailData()
        {
            try
            {
                // Nếu có nhiều sản phẩm/bao bì trong lô, hiển thị DataGridView
                if (_batchItems != null && _batchItems.Any())
                {
                    LoadBatchData();
                }
                // Nếu chỉ có 1 sản phẩm hoặc dùng constructor cũ
                else if (_detail != null)
                {
                    LoadSingleDetail();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBatchData()
        {
            if (_batchItems == null || !_batchItems.Any()) return;

            var firstItem = _batchItems.First();
            
            // Hiển thị thông tin chung của lô
            lblReferenceNumber.Text = $"Lô: {_batchNo}";
            lblBatchNo.Text = _batchNo ?? "Không có";
            lblSupplierName.Text = firstItem.SupplierName ?? "Không có";
            lblCreatedDate.Text = _batchItems.Max(x => x.CreatedDate).ToString("dd/MM/yyyy HH:mm");
            lblCreatedBy.Text = firstItem.CreatedBy ?? "-";
            
            // Tính tổng
            var totalQuantity = _batchItems.Sum(x => x.Quantity);
            var totalValue = _batchItems.Sum(x => x.TotalValue);
            lblQuantity.Text = totalQuantity.ToString("N2");
            lblTotalValue.Text = totalValue.ToString("N0") + " VNĐ";
            lblProductName.Text = $"{_batchItems.Count} sản phẩm";
            lblUnitPrice.Text = "-";
            
            // Ghi chú - lấy từ item đầu tiên hoặc tổng hợp
            var notes = string.Join("; ", _batchItems.Where(x => !string.IsNullOrWhiteSpace(x.Notes))
                .Select(x => x.Notes).Distinct());
            txtNotes.Text = notes;

            // Load DataGridView với danh sách sản phẩm và bao bì
            if (dgvProducts != null)
            {
                dgvProducts.Visible = true;
                dgvProducts.Rows.Clear();
                
                // Khởi tạo DataGridView nếu chưa có cột
                if (dgvProducts.Columns.Count == 0)
                {
                    dgvProducts.Columns.Add("ProductName", "Sản phẩm/Bao bì");
                    dgvProducts.Columns.Add("Quantity", "Số lượng");
                    dgvProducts.Columns.Add("UnitPrice", "Đơn giá");
                    dgvProducts.Columns.Add("TotalPrice", "Thành tiền");
                    dgvProducts.Columns.Add("ExpiryDate", "Hạn sử dụng");
                    dgvProducts.Columns.Add("Notes", "Ghi chú");
                    
                    dgvProducts.Columns["Quantity"].DefaultCellStyle.Format = "N2";
                    dgvProducts.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
                    dgvProducts.Columns["TotalPrice"].DefaultCellStyle.Format = "N0";
                }
                
                foreach (var item in _batchItems)
                {
                    // Xác định loại: sản phẩm hay bao bì
                    var itemType = !string.IsNullOrWhiteSpace(item.ProductName) ? "[SP] " : "[BB] ";
                    var productName = itemType + (item.ProductName ?? item.PackagingName ?? "-");
                    var expiryDate = item.ExpiryDate.HasValue 
                        ? item.ExpiryDate.Value.ToString("dd/MM/yyyy") 
                        : "Không có";
                    
                    dgvProducts.Rows.Add(
                        productName,
                        item.Quantity.ToString("N2"),
                        item.UnitPrice.ToString("N0"),
                        (item.Quantity * item.UnitPrice).ToString("N0"),
                        expiryDate,
                        item.Notes ?? ""
                    );
                }
            }
        }

        private void LoadSingleDetail()
        {
            if (_detail == null) return;

            lblReferenceNumber.Text = $"NK{_detail.StockInId:D6}";
            lblProductName.Text = _detail.ProductName ?? _detail.PackagingName ?? "-";
            
            if (string.IsNullOrWhiteSpace(_detail.BatchNo))
            {
                lblBatchNo.Text = "Không có";
            }
            else
            {
                lblBatchNo.Text = _detail.BatchNo;
            }
            
            if (string.IsNullOrWhiteSpace(_detail.SupplierName))
            {
                lblSupplierName.Text = "Không có";
            }
            else
            {
                lblSupplierName.Text = _detail.SupplierName;
            }
            
            lblQuantity.Text = _detail.Quantity.ToString("N2");
            lblUnitPrice.Text = _detail.UnitPrice.ToString("N0") + " VNĐ";
            lblTotalValue.Text = _detail.TotalValue.ToString("N0") + " VNĐ";
            lblCreatedDate.Text = _detail.CreatedDate.ToString("dd/MM/yyyy HH:mm");
            lblCreatedBy.Text = _detail.CreatedBy ?? "-";
            txtNotes.Text = _detail.Notes ?? "";
            
            if (_detail.ExpiryDate.HasValue)
            {
                lblExpiryDate.Text = _detail.ExpiryDate.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                lblExpiryDate.Text = "Không có";
            }

            // Ẩn DataGridView nếu có
            if (dgvProducts != null)
            {
                dgvProducts.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

