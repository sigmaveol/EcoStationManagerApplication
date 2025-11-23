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
        private class ProductRow
        {
            public int? ProductId { get; set; }
            public string ProductName { get; set; }
            public string BatchNo { get; set; }
            public string Unit { get; set; }
            public decimal Quantity { get; set; }
            public decimal CurrentStock { get; set; }
            public bool IsValid { get; set; } = true;
            public string ErrorMessage { get; set; }
        }

        private List<ProductRow> _productRows = new List<ProductRow>();
        private List<ProductDTO> _allProducts = new List<ProductDTO>();
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
            AddNewRow();
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

            // Cột ẩn ProductId
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductId",
                HeaderText = "ProductId",
                Visible = false
            });

            // Cột Sản phẩm (TextBox với autocomplete)
            var productColumn = new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "Sản phẩm",
                Width = 250
            };
            dgvProducts.Columns.Add(productColumn);

            // Cột Lô hàng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNo",
                HeaderText = "Lô hàng",
                Width = 120
            });

            // Cột Đơn vị
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "Đơn vị",
                Width = 80,
                ReadOnly = true
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

        private void DgvProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Xử lý autocomplete cho cột sản phẩm
            if (dgvProducts.CurrentCell.ColumnIndex == dgvProducts.Columns["ProductName"].Index)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    var autoComplete = new AutoCompleteStringCollection();
                    autoComplete.AddRange(_allProducts.Select(p => $"{p.Code} - {p.Name}").ToArray());
                    textBox.AutoCompleteCustomSource = autoComplete;
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
                var product = _allProducts.FirstOrDefault(p => 
                    $"{p.Code} - {p.Name}".Equals(productText, StringComparison.OrdinalIgnoreCase));
                if (product != null)
                {
                    row.Cells["ProductId"].Value = product.ProductId;
                    row.Cells["Unit"].Value = product.UnitMeasure ?? "-";
                    
                    // Hiển thị tồn kho
                    var currentStock = _productStocks.ContainsKey(product.ProductId) 
                        ? _productStocks[product.ProductId] 
                        : 0;
                    row.Cells["CurrentStock"].Value = currentStock.ToString("N2");
                    
                    // Kiểm tra lại tồn kho
                    ValidateStock(row);
                }
                else
                {
                    row.Cells["ProductId"].Value = DBNull.Value;
                    row.Cells["Unit"].Value = "-";
                    row.Cells["CurrentStock"].Value = "0";
                    row.Cells["Warning"].Value = "";
                    UpdateSummary();
                }
            }
        }

        private void ValidateStock(DataGridViewRow row)
        {
            var productIdCell = row.Cells["ProductId"];
            var quantityCell = row.Cells["Quantity"];
            var currentStockCell = row.Cells["CurrentStock"];
            var warningCell = row.Cells["Warning"];

            if (productIdCell.Value != null && productIdCell.Value != DBNull.Value &&
                int.TryParse(productIdCell.Value.ToString(), out int productId))
            {
                var currentStock = _productStocks.ContainsKey(productId) 
                    ? _productStocks[productId] 
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
            else
            {
                warningCell.Value = "";
            }
            
            UpdateSummary();
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

        private void AddNewRow()
        {
            var rowIndex = dgvProducts.Rows.Add();
            var row = dgvProducts.Rows[rowIndex];
            row.Cells["ProductName"].Value = "";
            row.Cells["Quantity"].Value = "0";
            row.Cells["Unit"].Value = "-";
            row.Cells["CurrentStock"].Value = "0";
            row.Cells["Warning"].Value = "";
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            AddNewRow();
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
                var productIdCell = row.Cells["ProductId"];
                
                if (productCell.Value == null || string.IsNullOrWhiteSpace(productCell.Value.ToString()) ||
                    productIdCell.Value == null || productIdCell.Value == DBNull.Value)
                {
                    ShowError($"Dòng {i + 1}: Vui lòng chọn sản phẩm.");
                    dgvProducts.CurrentCell = productCell;
                    return false;
                }

                if (!decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal quantity) || quantity <= 0)
                {
                    ShowError($"Dòng {i + 1}: Số lượng phải lớn hơn 0.");
                    dgvProducts.CurrentCell = row.Cells["Quantity"];
                    return false;
                }

                // Kiểm tra tồn kho
                if (int.TryParse(productIdCell.Value.ToString(), out int productId))
                {
                    var currentStock = _productStocks.ContainsKey(productId) 
                        ? _productStocks[productId] 
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
                    var productIdCell = row.Cells["ProductId"];
                    if (productIdCell.Value != null && productIdCell.Value != DBNull.Value &&
                        int.TryParse(productIdCell.Value.ToString(), out int productId))
                    {
                        var stockOut = new StockOut
                        {
                            RefType = RefType.PRODUCT,
                            RefId = productId,
                            Quantity = decimal.Parse(row.Cells["Quantity"].Value?.ToString() ?? "0"),
                            BatchNo = row.Cells["BatchNo"].Value?.ToString(),
                            Purpose = purpose,
                            Notes = notes,
                            CreatedDate = stockOutDate,
                            CreatedBy = UserContextHelper.GetCurrentUserId() // Người tạo là người hiện tại đăng nhập
                        };

                        stockOuts.Add(stockOut);
                    }
                }

                if (stockOuts.Any())
                {
                    var result = await AppServices.StockOutService.CreateMultipleStockOutsAsync(stockOuts);
                    if (result.Success)
                    {
                        UIHelper.ShowSuccessMessage($"Xuất kho thành công {stockOuts.Count} sản phẩm!");
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

