using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class MultiProductStockOutForm : Form
    {
        private List<ProductDTO> _allProducts = new List<ProductDTO>();
        private List<Packaging> _allPackagings = new List<Packaging>();
        private Dictionary<int, decimal> _productStocks = new Dictionary<int, decimal>();

        public MultiProductStockOutForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;
            dtpStockOutDate.Value = DateTime.Now;

            // Khởi tạo mục đích xuất
            cmbPurpose.Items.Clear();
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Bán hàng", Value = StockOutPurpose.SALE });
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Chuyển kho", Value = StockOutPurpose.TRANSFER });
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Hao hụt", Value = StockOutPurpose.DAMAGE });
            cmbPurpose.DisplayMember = "Text";
            cmbPurpose.ValueMember = "Value";
            cmbPurpose.SelectedIndex = 0;

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

                    // Load tồn kho cho tất cả sản phẩm
                    foreach (var product in _allProducts)
                    {
                        var inventoryResult = await AppServices.InventoryService.GetInventoryByProductAsync(product.ProductId);
                        if (inventoryResult.Success && inventoryResult.Data != null)
                        {
                            var totalQty = inventoryResult.Data.Sum(inv => inv.Quantity);
                            _productStocks[product.ProductId] = totalQty;
                        }
                        else
                        {
                            _productStocks[product.ProductId] = 0;
                        }
                    }
                }

                // Load bao bì
                var packagingsResult = await AppServices.PackagingService.GetAllPackagingsAsync();
                if (packagingsResult.Success && packagingsResult.Data != null)
                {
                    _allPackagings = packagingsResult.Data.ToList();
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
            dgvProducts.Columns.Add(productColumn);

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

            // Cột Đơn vị
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "Đơn vị",
                Width = 80,
                ReadOnly = true
            });

            // Cột Lô hàng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNo",
                HeaderText = "Lô hàng",
                Width = 120
            });

            // Cột Tồn kho
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CurrentStock",
                HeaderText = "Tồn kho",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Số lượng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "Số lượng",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột Cảnh báo
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Warning",
                HeaderText = "Cảnh báo",
                Width = 200,
                ReadOnly = true
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

        private void DgvProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvProducts.Rows[e.RowIndex];
            
            // Cập nhật thông tin sản phẩm khi chọn
            if (e.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                UpdateProductInfo(row);
            }
            // Kiểm tra tồn kho khi số lượng thay đổi
            else if (e.ColumnIndex == dgvProducts.Columns["Quantity"].Index)
            {
                ValidateStock(row);
            }
        }

        private void DgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvProducts.Rows[e.RowIndex];
            
            // Cập nhật thông tin sản phẩm sau khi edit
            if (e.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                UpdateProductInfo(row);
            }
            // Kiểm tra tồn kho sau khi edit số lượng
            else if (e.ColumnIndex == dgvProducts.Columns["Quantity"].Index)
            {
                ValidateStock(row);
            }
        }

        private void UpdateProductInfo(DataGridViewRow row)
        {
            var productCell = row.Cells["ProductName"];
            
            if (productCell.Value != null)
            {
                var productText = productCell.Value.ToString();
                if (productText.StartsWith("[SP]"))
                {
                    var productCodeName = productText.Substring(5).Trim();
                    var product = _allProducts.FirstOrDefault(p => 
                        $"{p.Code} - {p.Name}".Equals(productCodeName, StringComparison.OrdinalIgnoreCase));
                    if (product != null)
                    {
                        row.Cells["RefType"].Value = RefType.PRODUCT.ToString();
                        row.Cells["RefId"].Value = product.ProductId;
                        row.Cells["Unit"].Value = product.UnitMeasure ?? "-";
                        
                        // Hiển thị tồn kho
                        var currentStock = _productStocks.ContainsKey(product.ProductId) 
                            ? _productStocks[product.ProductId] 
                            : 0;
                        row.Cells["CurrentStock"].Value = currentStock.ToString("N2");
                        
                        // Kiểm tra lại tồn kho
                        ValidateStock(row);
                    }
                }
                else if (productText.StartsWith("[BB]"))
                {
                    var packagingCodeName = productText.Substring(5).Trim();
                    var packaging = _allPackagings.FirstOrDefault(p => 
                        $"{p.Barcode ?? ""} - {p.Name}".Equals(packagingCodeName, StringComparison.OrdinalIgnoreCase));
                    if (packaging != null)
                    {
                        row.Cells["RefType"].Value = RefType.PACKAGING.ToString();
                        row.Cells["RefId"].Value = packaging.PackagingId;
                        row.Cells["Unit"].Value = "cái";
                        row.Cells["CurrentStock"].Value = "0"; // Bao bì không có tồn kho
                        row.Cells["Warning"].Value = "";
                    }
                }
                else
                {
                    row.Cells["RefType"].Value = DBNull.Value;
                    row.Cells["RefId"].Value = DBNull.Value;
                    row.Cells["Unit"].Value = "-";
                    row.Cells["CurrentStock"].Value = "0";
                    row.Cells["Warning"].Value = "";
                    UpdateSummary();
                }
            }
        }

        private void ValidateStock(DataGridViewRow row)
        {
            var refTypeCell = row.Cells["RefType"];
            var refIdCell = row.Cells["RefId"];
            var quantityCell = row.Cells["Quantity"];
            var currentStockCell = row.Cells["CurrentStock"];
            var warningCell = row.Cells["Warning"];

            // Chỉ validate tồn kho cho sản phẩm (PRODUCT), không validate cho bao bì
            if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                refTypeCell.Value.ToString() == RefType.PRODUCT.ToString() &&
                refIdCell.Value != null && refIdCell.Value != DBNull.Value &&
                int.TryParse(refIdCell.Value.ToString(), out int refId))
            {
                var currentStock = _productStocks.ContainsKey(refId) 
                    ? _productStocks[refId] 
                    : 0;

                if (decimal.TryParse(quantityCell.Value?.ToString(), out decimal quantity))
                {
                    if (quantity > currentStock)
                    {
                        warningCell.Value = $"⚠ Không đủ tồn! (Còn: {currentStock:N2})";
                        warningCell.Style.ForeColor = System.Drawing.Color.Red;
                        row.Cells["Quantity"].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (quantity <= 0)
                    {
                        warningCell.Value = "⚠ Số lượng phải > 0";
                        warningCell.Style.ForeColor = System.Drawing.Color.Red;
                        row.Cells["Quantity"].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        warningCell.Value = "✓ Hợp lệ";
                        warningCell.Style.ForeColor = System.Drawing.Color.Green;
                        row.Cells["Quantity"].Style.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    warningCell.Value = "⚠ Số lượng không hợp lệ";
                    warningCell.Style.ForeColor = System.Drawing.Color.Red;
                }
            }
            else if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                     refTypeCell.Value.ToString() == RefType.PACKAGING.ToString())
            {
                // Bao bì không cần kiểm tra tồn kho, chỉ kiểm tra số lượng > 0
                if (decimal.TryParse(quantityCell.Value?.ToString(), out decimal quantity))
                {
                    if (quantity <= 0)
                    {
                        warningCell.Value = "⚠ Số lượng phải > 0";
                        warningCell.Style.ForeColor = System.Drawing.Color.Red;
                        row.Cells["Quantity"].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        warningCell.Value = "✓ Hợp lệ";
                        warningCell.Style.ForeColor = System.Drawing.Color.Green;
                        row.Cells["Quantity"].Style.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    warningCell.Value = "⚠ Số lượng không hợp lệ";
                    warningCell.Style.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                warningCell.Value = "";
            }
            
            UpdateSummary();
        }

        private void UpdateStockInfoAndValidate(DataGridViewRow row)
        {
            // Cập nhật thông tin tồn kho và validate
            var refTypeCell = row.Cells["RefType"];
            var refIdCell = row.Cells["RefId"];
            
            if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                refTypeCell.Value.ToString() == RefType.PRODUCT.ToString() &&
                refIdCell.Value != null && refIdCell.Value != DBNull.Value &&
                int.TryParse(refIdCell.Value.ToString(), out int refId))
            {
                // Cập nhật tồn kho cho sản phẩm
                var currentStock = _productStocks.ContainsKey(refId) 
                    ? _productStocks[refId] 
                    : 0;
                row.Cells["CurrentStock"].Value = currentStock.ToString("N2");
            }
            else if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                     refTypeCell.Value.ToString() == RefType.PACKAGING.ToString())
            {
                // Bao bì không có tồn kho
                row.Cells["CurrentStock"].Value = "0";
            }
            
            // Validate sau khi cập nhật
            ValidateStock(row);
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvProducts.Columns[e.ColumnIndex].Name == "Delete")
            {
                dgvProducts.Rows.RemoveAt(e.RowIndex);
                UpdateSummary();
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            // Mở form chọn sản phẩm/bao bì
            using (var selectionForm = new SingleProductStockOutForm(_allProducts, _allPackagings))
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

                    // Set BatchNo nếu có từ form chọn
                    if (!string.IsNullOrEmpty(selectionForm.BatchNo))
                    {
                        row.Cells["BatchNo"].Value = selectionForm.BatchNo;
                    }

                    // Cập nhật tồn kho và kiểm tra
                    UpdateStockInfoAndValidate(row);

                    // Cập nhật tổng kết
                    UpdateSummary();
                }
            }
        }

        private void UpdateSummary()
        {
            int totalRows = dgvProducts.Rows.Count;
            decimal totalQuantity = 0;
            int invalidRows = 0;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal qty))
                {
                    totalQuantity += qty;
                }
                
                var warning = row.Cells["Warning"].Value?.ToString() ?? "";
                if (warning.Contains("⚠"))
                {
                    invalidRows++;
                }
            }

            lblTotalRows.Text = $"Tổng số dòng: {totalRows}";
            lblTotalQuantity.Text = $"Tổng số lượng: {totalQuantity:N2}";
            
            if (invalidRows > 0)
            {
                lblSummaryWarning.Text = $"⚠ Có {invalidRows} dòng không hợp lệ";
                lblSummaryWarning.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblSummaryWarning.Text = "✓ Tất cả dòng hợp lệ";
                lblSummaryWarning.ForeColor = System.Drawing.Color.Green;
            }
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
                
                if (productCell.Value == null || string.IsNullOrWhiteSpace(productCell.Value.ToString()) ||
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

                // Kiểm tra tồn kho chỉ cho sản phẩm (PRODUCT)
                if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                    refTypeCell.Value.ToString() == RefType.PRODUCT.ToString() &&
                    int.TryParse(refIdCell.Value.ToString(), out int refId))
                {
                    var currentStock = _productStocks.ContainsKey(refId) 
                        ? _productStocks[refId] 
                        : 0;
                    
                    if (quantity > currentStock)
                    {
                        ShowError($"Dòng {i + 1}: Không đủ tồn kho. Tồn kho hiện có: {currentStock:N2}, yêu cầu: {quantity:N2}");
                        dgvProducts.CurrentCell = row.Cells["Quantity"];
                        return false;
                    }
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
                var stockOuts = new List<StockOut>();
                var stockOutDate = dtpStockOutDate.Value;
                var notes = txtNotes.Text;
                
                StockOutPurpose purpose = StockOutPurpose.SALE;
                if (cmbPurpose.SelectedItem is ComboItem<StockOutPurpose> purposeItem)
                {
                    purpose = purposeItem.Value;
                }

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    var refTypeCell = row.Cells["RefType"];
                    var refIdCell = row.Cells["RefId"];
                    
                    if (refTypeCell.Value != null && refTypeCell.Value != DBNull.Value &&
                        refIdCell.Value != null && refIdCell.Value != DBNull.Value &&
                        Enum.TryParse<RefType>(refTypeCell.Value.ToString(), out RefType refType) &&
                        int.TryParse(refIdCell.Value.ToString(), out int refId))
                    {
                        var stockOut = new StockOut
                        {
                            RefType = refType,
                            RefId = refId,
                            Quantity = decimal.Parse(row.Cells["Quantity"].Value?.ToString() ?? "0"),
                            BatchNo = row.Cells["BatchNo"].Value?.ToString(),
                            Purpose = purpose,
                            Notes = notes,
                            CreatedDate = stockOutDate,
                            CreatedBy = AppUserContext.CurrentUserId // Người tạo là người hiện tại đăng nhập
                        };

                        stockOuts.Add(stockOut);
                    }
                }

                if (stockOuts.Any())
                {
                    var result = await AppServices.StockOutService.CreateMultipleStockOutsAsync(stockOuts);
                    if (result.Success)
                    {
                        // Hiển thị thông báo thành công và đợi người dùng đóng
                        UIHelper.ShowSuccessMessage($"Xuất kho thành công {stockOuts.Count} sản phẩm!");
                        btnCancel_Click(sender, e);
                    }
                    else
                    {
                        UIHelper.HandleServiceResult(result);
                    }
                }
                else
                {
                    UIHelper.ShowWarningMessage("Không có sản phẩm nào để xuất kho!");
                }   
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "lưu xuất kho");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

