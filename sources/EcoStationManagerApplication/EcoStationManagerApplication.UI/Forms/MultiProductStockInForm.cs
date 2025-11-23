using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class MultiProductStockInForm : Form
    {
        private class ProductRow
        {
            public RefType? RefType { get; set; } // PRODUCT hoặc PACKAGING
            public int? RefId { get; set; } // ProductId hoặc PackagingId
            public string ProductName { get; set; }
            public string BatchNo { get; set; }
            public string Unit { get; set; }
            public decimal Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice => Quantity * UnitPrice;
            public DateTime? ExpiryDate { get; set; }
        }

        private List<ProductDTO> _allProducts = new List<ProductDTO>();
        private List<Packaging> _allPackagings = new List<Packaging>();
        private List<Supplier> _allSuppliers = new List<Supplier>();

        public MultiProductStockInForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;
            dtpStockInDate.Value = DateTime.Now;

            // Khởi tạo nguồn nhập
            cmbSource.Items.Clear();
            cmbSource.Items.Add("Nhà cung cấp");
            cmbSource.Items.Add("Chuyển kho");
            cmbSource.Items.Add("Trả hàng");
            cmbSource.SelectedIndex = 0;

            await LoadDataAsync();
            InitializeDataGridView();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Load sản phẩm
                var productsResult = await AppServices.ProductService.GetAllProductsAsync();
                if (productsResult.Success && productsResult.Data != null)
                {
                    _allProducts = productsResult.Data
                        .Where(p => p.IsActive == ActiveStatus.ACTIVE)
                        .Select(p => new ProductDTO
                        {
                            ProductId = p.ProductId,
                            Code = p.Sku ?? "",
                            Name = p.Name,
                            UnitMeasure = p.Unit
                        })
                        .ToList();
                }

                // Load bao bì
                var packagingsResult = await AppServices.PackagingService.GetAllPackagingsAsync();
                if (packagingsResult.Success && packagingsResult.Data != null)
                {
                    _allPackagings = packagingsResult.Data.ToList();
                }

                // Load nhà cung cấp
                var suppliersResult = await AppServices.SupplierService.GetAllSuppliersAsync();
                if (suppliersResult.Success && suppliersResult.Data != null)
                {
                    _allSuppliers = suppliersResult.Data.ToList();
                    cmbSupplier.Items.Clear();
                    cmbSupplier.Items.Add(new ComboItem<int?> { Text = "-- Chọn nhà cung cấp --", Value = null });
                    foreach (var supplier in _allSuppliers)
                    {
                        cmbSupplier.Items.Add(new ComboItem<int?> { Text = supplier.Name, Value = supplier.SupplierId });
                    }
                    cmbSupplier.DisplayMember = "Text";
                    cmbSupplier.ValueMember = "Value";
                    cmbSupplier.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu");
            }
        }

        private void InitializeDataGridView()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;

            // Cột ẩn RefType (PRODUCT hoặc PACKAGING)
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RefType",
                HeaderText = "RefType",
                Visible = false
            });

            // Cột ẩn RefId (ProductId hoặc PackagingId)
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RefId",
                HeaderText = "RefId",
                Visible = false
            });

            // Cột Sản phẩm/Bao bì (ComboBox) - Chọn từ danh sách database
            var productColumn = new DataGridViewComboBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm/Bao bì",
                Width = 300,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                DisplayStyleForCurrentCellOnly = false,
                FlatStyle = FlatStyle.Flat
            };
            
            // Thêm danh sách sản phẩm và bao bì vào ComboBox
            productColumn.Items.Add("-- Chọn sản phẩm/bao bì --");
            
            // Thêm sản phẩm với prefix [SP]
            foreach (var product in _allProducts)
            {
                productColumn.Items.Add($"[SP] {product.Code} - {product.Name}");
            }
            
            // Thêm bao bì với prefix [BB]
            foreach (var packaging in _allPackagings)
            {
                productColumn.Items.Add($"[BB] {packaging.Barcode ?? ""} - {packaging.Name}");
            }
            
            dgvProducts.Columns.Add(productColumn);

            // Cột Đơn vị
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "Đơn vị",
                Width = 80,
                ReadOnly = true
            });

            // Cột Số lượng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Hạn sử dụng (DateTimePicker)
            var expiryDateColumn = new DataGridViewTextBoxColumn
            {
                Name = "ExpiryDate",
                HeaderText = "Hạn sử dụng",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            };
            dgvProducts.Columns.Add(expiryDateColumn);

            // Cột Đơn giá
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "Đơn giá",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Thành tiền
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalPrice",
                HeaderText = "Thành tiền",
                Width = 130,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Thao tác
            var deleteColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Thao tác",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dgvProducts.Columns.Add(deleteColumn);

            dgvProducts.CellValueChanged += DgvProducts_CellValueChanged;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.CellEndEdit += DgvProducts_CellEndEdit;
            dgvProducts.EditingControlShowing += DgvProducts_EditingControlShowing;
        }

        private void DgvProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Xử lý khi chọn sản phẩm từ ComboBox
            if (dgvProducts.CurrentCell.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                if (e.Control is ComboBox comboBox)
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                    comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                }
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentCell == null) return;
            
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                var selectedText = comboBox.SelectedItem.ToString();
                if (selectedText != "-- Chọn sản phẩm/bao bì --")
                {
                    // Kiểm tra xem là sản phẩm [SP] hay bao bì [BB]
                    if (selectedText.StartsWith("[SP]"))
                    {
                        // Là sản phẩm
                        var productText = selectedText.Substring(5).Trim(); // Bỏ "[SP] "
                        var product = _allProducts.FirstOrDefault(p => 
                            $"{p.Code} - {p.Name}".Equals(productText, StringComparison.OrdinalIgnoreCase));
                        if (product != null)
                        {
                            dgvProducts.CurrentRow.Cells["RefType"].Value = RefType.PRODUCT.ToString();
                            dgvProducts.CurrentRow.Cells["RefId"].Value = product.ProductId;
                            dgvProducts.CurrentRow.Cells["Unit"].Value = product.UnitMeasure ?? "-";
                        }
                    }
                    else if (selectedText.StartsWith("[BB]"))
                    {
                        // Là bao bì
                        var packagingText = selectedText.Substring(5).Trim(); // Bỏ "[BB] "
                        var packaging = _allPackagings.FirstOrDefault(p => 
                            $"{p.Barcode ?? ""} - {p.Name}".Equals(packagingText, StringComparison.OrdinalIgnoreCase));
                        if (packaging != null)
                        {
                            dgvProducts.CurrentRow.Cells["RefType"].Value = RefType.PACKAGING.ToString();
                            dgvProducts.CurrentRow.Cells["RefId"].Value = packaging.PackagingId;
                            dgvProducts.CurrentRow.Cells["Unit"].Value = "cái";
                        }
                    }
                }
                else
                {
                    dgvProducts.CurrentRow.Cells["RefType"].Value = DBNull.Value;
                    dgvProducts.CurrentRow.Cells["RefId"].Value = DBNull.Value;
                    dgvProducts.CurrentRow.Cells["Unit"].Value = "-";
                }
            }
        }

        private void DgvProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvProducts.Rows[e.RowIndex];
            
            // Cập nhật thông tin sản phẩm khi chọn
            if (e.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                UpdateProductInfo();
            }
            // Tính thành tiền khi số lượng hoặc đơn giá thay đổi
            else if (e.ColumnIndex == dgvProducts.Columns["Quantity"].Index || 
                     e.ColumnIndex == dgvProducts.Columns["UnitPrice"].Index)
            {
                CalculateTotalPrice(row);
            }
        }

        private void DgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvProducts.Rows[e.RowIndex];
            
            // Cập nhật thông tin sản phẩm sau khi edit
            if (e.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                UpdateProductInfo();
            }
            // Tính lại thành tiền sau khi edit
            else if (e.ColumnIndex == dgvProducts.Columns["Quantity"].Index || 
                     e.ColumnIndex == dgvProducts.Columns["UnitPrice"].Index)
            {
                CalculateTotalPrice(row);
            }
            // Validate và format hạn sử dụng
            else if (e.ColumnIndex == dgvProducts.Columns["ExpiryDate"].Index)
            {
                ValidateAndFormatExpiryDate(row);
            }
        }

        private void ValidateAndFormatExpiryDate(DataGridViewRow row)
        {
            var expiryDateCell = row.Cells["ExpiryDate"];
            if (expiryDateCell.Value != null && !string.IsNullOrWhiteSpace(expiryDateCell.Value.ToString()))
            {
                var value = expiryDateCell.Value.ToString();
                // Thử parse với nhiều format
                DateTime parsedDate;
                if (DateTime.TryParse(value, out parsedDate))
                {
                    expiryDateCell.Value = parsedDate.ToString("dd/MM/yyyy");
                    expiryDateCell.Style.ForeColor = System.Drawing.Color.Black;
                }
                else if (DateTime.TryParseExact(value, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    expiryDateCell.Value = parsedDate.ToString("dd/MM/yyyy");
                    expiryDateCell.Style.ForeColor = System.Drawing.Color.Black;
                }
                else if (DateTime.TryParseExact(value, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    expiryDateCell.Value = parsedDate.ToString("dd/MM/yyyy");
                    expiryDateCell.Style.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    // Không parse được, hiển thị cảnh báo
                    expiryDateCell.Style.ForeColor = System.Drawing.Color.Red;
                    expiryDateCell.ToolTipText = "Định dạng ngày không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy";
                }
            }
            else
            {
                // Cho phép để trống
                expiryDateCell.Value = null;
                expiryDateCell.Style.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void UpdateProductInfo()
        {
            if (dgvProducts.CurrentCell == null || dgvProducts.CurrentRow == null) return;
            var row = dgvProducts.CurrentRow;
            var productCell = row.Cells["ProductName"];
            
            if (productCell.Value != null)
            {
                var productText = productCell.Value.ToString();
                if (productText != "-- Chọn sản phẩm/bao bì --")
                {
                    // Kiểm tra xem là sản phẩm [SP] hay bao bì [BB]
                    if (productText.StartsWith("[SP]"))
                    {
                        // Là sản phẩm
                        var productName = productText.Substring(5).Trim(); // Bỏ "[SP] "
                        var product = _allProducts.FirstOrDefault(p => 
                            $"{p.Code} - {p.Name}".Equals(productName, StringComparison.OrdinalIgnoreCase));
                        if (product != null)
                        {
                            row.Cells["RefType"].Value = RefType.PRODUCT.ToString();
                            row.Cells["RefId"].Value = product.ProductId;
                            row.Cells["Unit"].Value = product.UnitMeasure ?? "-";
                        }
                    }
                    else if (productText.StartsWith("[BB]"))
                    {
                        // Là bao bì
                        var packagingName = productText.Substring(5).Trim(); // Bỏ "[BB] "
                        var packaging = _allPackagings.FirstOrDefault(p => 
                            $"{p.Barcode ?? ""} - {p.Name}".Equals(packagingName, StringComparison.OrdinalIgnoreCase));
                        if (packaging != null)
                        {
                            row.Cells["RefType"].Value = RefType.PACKAGING.ToString();
                            row.Cells["RefId"].Value = packaging.PackagingId;
                            row.Cells["Unit"].Value = "cái";
                        }
                    }
                }
                else
                {
                    row.Cells["RefType"].Value = DBNull.Value;
                    row.Cells["RefId"].Value = DBNull.Value;
                    row.Cells["Unit"].Value = "-";
                }
            }
        }

        private void CalculateTotalPrice(DataGridViewRow row)
        {
            if (decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal quantity) &&
                decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal unitPrice))
            {
                row.Cells["TotalPrice"].Value = (quantity * unitPrice).ToString("N0");
            }
            else
            {
                row.Cells["TotalPrice"].Value = "0";
            }
            UpdateSummary();
        }

        //private void DgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0) return;
        //    var row = dgvProducts.Rows[e.RowIndex];
            
        //    // Cập nhật thông tin sản phẩm sau khi edit
        //    if (e.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
        //    {
        //        UpdateProductInfo();
        //    }
        //    // Tính lại thành tiền sau khi edit
        //    else if (e.ColumnIndex == dgvProducts.Columns["Quantity"].Index || 
        //             e.ColumnIndex == dgvProducts.Columns["UnitPrice"].Index)
        //    {
        //        CalculateTotalPrice(row);
        //    }
        //}

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvProducts.Columns[e.ColumnIndex].Name == "Delete")
            {
                dgvProducts.Rows.RemoveAt(e.RowIndex);
                UpdateSummary();
            }
        }

        private void AddNewRow()
        {
            var rowIndex = dgvProducts.Rows.Add();
            var row = dgvProducts.Rows[rowIndex];
            row.Cells["ProductName"].Value = "-- Chọn sản phẩm/bao bì --";
            row.Cells["Quantity"].Value = "0";
            row.Cells["UnitPrice"].Value = "0";
            row.Cells["TotalPrice"].Value = "0";
            row.Cells["Unit"].Value = "-";
            row.Cells["RefType"].Value = DBNull.Value;
            row.Cells["RefId"].Value = DBNull.Value;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            // Mở form chọn sản phẩm/bao bì
            using (var selectionForm = new ProductSelectionForm(_allProducts, _allPackagings))
            {
                if (selectionForm.ShowDialog() == DialogResult.OK && selectionForm.IsOK)
                {
                    // Thêm dòng mới vào DataGridView với thông tin đã chọn
                    var rowIndex = dgvProducts.Rows.Add();
                    var row = dgvProducts.Rows[rowIndex];
                    
                    // Set các giá trị từ form chọn
                    row.Cells["ProductName"].Value = selectionForm.SelectedProductName;
                    row.Cells["RefType"].Value = selectionForm.SelectedRefType.HasValue ? selectionForm.SelectedRefType.Value.ToString() : (object)DBNull.Value;
                    row.Cells["RefId"].Value = selectionForm.SelectedRefId.HasValue ? (object)selectionForm.SelectedRefId.Value : DBNull.Value;
                    row.Cells["Unit"].Value = selectionForm.SelectedUnit ?? "-";
                    row.Cells["Quantity"].Value = selectionForm.Quantity.ToString("N2");
                    row.Cells["UnitPrice"].Value = selectionForm.UnitPrice.ToString("N0");
                    
                    // Tính thành tiền tự động
                    CalculateTotalPrice(row);
                    
                    // Set hạn sử dụng nếu có
                    if (selectionForm.ExpiryDate.HasValue)
                    {
                        row.Cells["ExpiryDate"].Value = selectionForm.ExpiryDate.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        row.Cells["ExpiryDate"].Value = DBNull.Value;
                    }
                    
                    // Cập nhật tổng kết
                    UpdateSummary();
                }
            }
        }

        private void UpdateSummary()
        {
            int totalRows = dgvProducts.Rows.Count;
            decimal totalQuantity = 0;
            decimal totalValue = 0;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal qty))
                {
                    totalQuantity += qty;
                }
                if (decimal.TryParse(row.Cells["TotalPrice"].Value?.ToString(), out decimal value))
                {
                    totalValue += value;
                }
            }

            lblTotalRows.Text = $"Tổng số dòng: {totalRows}";
            lblTotalQuantity.Text = $"Tổng số lượng: {totalQuantity:N2}";
            lblTotalValue.Text = $"Tổng giá trị: {totalValue:N0} VNĐ";
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (dgvProducts.Rows.Count == 0)
            {
                ShowError("Vui lòng thêm ít nhất một sản phẩm.");
                return false;
            }

            // Kiểm tra từng dòng
            for (int i = 0; i < dgvProducts.Rows.Count; i++)
            {
                var row = dgvProducts.Rows[i];
                var productCell = row.Cells["ProductName"];
                var refTypeCell = row.Cells["RefType"];
                var refIdCell = row.Cells["RefId"];
                
                var productValue = productCell.Value?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(productValue) || 
                    productValue == "-- Chọn sản phẩm/bao bì --" ||
                    refTypeCell.Value == null || refTypeCell.Value == DBNull.Value ||
                    refIdCell.Value == null || refIdCell.Value == DBNull.Value)
                {
                    ShowError($"Dòng {i + 1}: Vui lòng chọn sản phẩm hoặc bao bì.");
                    dgvProducts.CurrentCell = productCell;
                    return false;
                }

                if (!decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal quantity) || quantity <= 0)
                {
                    ShowError($"Dòng {i + 1}: Số lượng phải lớn hơn 0.");
                    dgvProducts.CurrentCell = row.Cells["Quantity"];
                    return false;
                }
            }

            return true;
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var stockIns = new List<StockIn>();
                var stockInDate = dtpStockInDate.Value;
                var supplierId = cmbSupplier.SelectedItem is ComboItem<int?> supplierItem && supplierItem.Value.HasValue 
                    ? supplierItem.Value.Value 
                    : (int?)null;
                var notes = txtNotes.Text;
                var batchNo = txtBatchNo.Text; // Lấy mã lô từ phần thông tin chung

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    var refTypeCell = row.Cells["RefType"];
                    var refIdCell = row.Cells["RefId"];
                    var productNameCell = row.Cells["ProductName"];
                    var productValue = productNameCell.Value?.ToString() ?? "";
                    
                    // Bỏ qua dòng chưa chọn sản phẩm/bao bì
                    if (productValue == "-- Chọn sản phẩm/bao bì --" || string.IsNullOrWhiteSpace(productValue))
                        continue;
                    
                    if (refTypeCell.Value != null && refIdCell.Value != null && 
                        refIdCell.Value != DBNull.Value &&
                        Enum.TryParse<RefType>(refTypeCell.Value.ToString(), out RefType refType) &&
                        int.TryParse(refIdCell.Value.ToString(), out int refId))
                    {
                        var stockIn = new StockIn
                        {
                            RefType = refType,
                            RefId = refId,
                            Quantity = decimal.Parse(row.Cells["Quantity"].Value?.ToString() ?? "0"),
                            UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value?.ToString() ?? "0"),
                            BatchNo = string.IsNullOrWhiteSpace(batchNo) ? null : batchNo, // Cho phép null/trống
                            SupplierId = supplierId,
                            Notes = notes,
                            CreatedDate = stockInDate,
                            CreatedBy = AppUserContext.CurrentUserId // Người tạo là người hiện tại đăng nhập
                        };

                        // Xử lý hạn sử dụng nếu có (chỉ áp dụng cho sản phẩm)
                        if (refType == RefType.PRODUCT && row.Cells["ExpiryDate"] != null && row.Cells["ExpiryDate"].Value != null)
                        {
                            var expiryDateValue = row.Cells["ExpiryDate"].Value.ToString();
                            if (!string.IsNullOrWhiteSpace(expiryDateValue))
                            {
                                // Thử parse với nhiều format
                                if (DateTime.TryParse(expiryDateValue, out DateTime expiryDate))
                                {
                                    stockIn.ExpiryDate = expiryDate;
                                }
                                else if (DateTime.TryParseExact(expiryDateValue, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out expiryDate))
                                {
                                    stockIn.ExpiryDate = expiryDate;
                                }
                                else if (DateTime.TryParseExact(expiryDateValue, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out expiryDate))
                                {
                                    stockIn.ExpiryDate = expiryDate;
                                }
                                // Nếu không parse được, bỏ qua (không set ExpiryDate)
                            }
                        }

                        stockIns.Add(stockIn);
                    }
                }

                if (stockIns.Any())
                {
                    var result = await AppServices.StockInService.CreateMultipleStockInsAsync(stockIns);
                    if (result.Success)
                    {
                        UIHelper.ShowSuccessMessage($"Nhập kho thành công {stockIns.Count} sản phẩm!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        UIHelper.HandleServiceResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "lưu nhập kho");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

