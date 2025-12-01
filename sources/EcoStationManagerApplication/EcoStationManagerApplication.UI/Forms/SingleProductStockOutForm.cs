using EcoStationManagerApplication.Common.Helpers;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class SingleProductStockOutForm : Form
    {
        public bool IsOK { get; private set; } = false;
        public string SelectedProductName { get; private set; }
        public RefType? SelectedRefType { get; private set; }
        public int? SelectedRefId { get; private set; }
        public string SelectedUnit { get; private set; }
        public string BatchNo { get; private set; }
        public decimal Quantity { get; private set; }

        private List<ProductDTO> _allProducts;
        private List<Packaging> _allPackagings;
        private Dictionary<int, List<string>> _productBatchNos;
        private Dictionary<int, List<string>> _packagingBatchNos;
        private Dictionary<int, decimal> _productStocks = new Dictionary<int, decimal>();
        private Dictionary<string, decimal> _batchStocks = new Dictionary<string, decimal>(); // Key: "ProductId_BatchNo", Value: Quantity
        private decimal _currentStock = 0;

        public SingleProductStockOutForm(List<ProductDTO> products, List<Packaging> packagings)
        {
            InitializeComponent();
            _allProducts = products;
            _allPackagings = packagings;
            _productBatchNos = new Dictionary<int, List<string>>();
            _packagingBatchNos = new Dictionary<int, List<string>>();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            // Khởi tạo ComboBox với sản phẩm và bao bì
            cmbProduct.Items.Clear();
            cmbProduct.Items.Add("-- Chọn sản phẩm/bao bì --");

            // Thêm sản phẩm với prefix [SP]
            foreach (var product in _allProducts)
            {
                cmbProduct.Items.Add($"[SP] {product.Code} - {product.Name}");
            }

            // Thêm bao bì với prefix [BB]
            foreach (var packaging in _allPackagings)
            {
                cmbProduct.Items.Add($"[BB] {packaging.Barcode ?? ""} - {packaging.Name}");
            }

            cmbProduct.SelectedIndex = 0;
            txtQuantity.Text = "1,00";
            labelError.Text = "";
            labelError.Visible = false;

            // Load tồn kho cho sản phẩm
            await LoadProductStocksAsync();

            // Load batch nos từ database
            await LoadBatchNosAsync();

            // Set focus vào ComboBox
            cmbProduct.Focus();
        }

        private async Task LoadProductStocksAsync()
        {
            try
            {
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading product stocks: {ex.Message}");
            }
        }

        private async Task LoadBatchNosAsync()
        {
            try
            {
                // Load batch nos và tồn kho theo lô cho sản phẩm từ Inventory
                foreach (var product in _allProducts)
                {
                    var inventoryResult = await AppServices.InventoryService.GetInventoryByProductAsync(product.ProductId);
                    if (inventoryResult.Success && inventoryResult.Data != null)
                    {
                        var batchNos = inventoryResult.Data
                            .Where(inv => !string.IsNullOrWhiteSpace(inv.BatchNo))
                            .Select(inv => inv.BatchNo)
                            .Distinct()
                            .ToList();
                        
                        if (batchNos.Any())
                        {
                            _productBatchNos[product.ProductId] = batchNos;
                            
                            // Lưu tồn kho theo lô
                            foreach (var inventory in inventoryResult.Data)
                            {
                                if (!string.IsNullOrWhiteSpace(inventory.BatchNo))
                                {
                                    var key = $"{product.ProductId}_{inventory.BatchNo}";
                                    _batchStocks[key] = inventory.Quantity;
                                }
                            }
                        }
                    }
                }

                // Bao bì không có batch numbers trong PackagingInventory, bỏ qua
                // Nếu cần batch numbers cho bao bì, có thể lấy từ StockIn/StockOut records
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi để tránh làm gián đoạn form
                System.Diagnostics.Debug.WriteLine($"Error loading batch nos: {ex.Message}");
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProductInfo();
            UpdateBatchNoComboBox();
        }

        private void UpdateProductInfo()
        {
            if (cmbProduct.SelectedIndex <= 0)
            {
                lblStock.Text = "Tồn kho: -";
                _currentStock = 0;
                SelectedRefType = null;
                SelectedRefId = null;
                SelectedUnit = null;
                SelectedProductName = null;
                return;
            }

            var selectedText = cmbProduct.SelectedItem.ToString();
            SelectedProductName = selectedText;

            if (selectedText.StartsWith("[SP]"))
            {
                // Là sản phẩm
                var productText = selectedText.Substring(5).Trim(); // Bỏ "[SP] "
                var product = _allProducts.FirstOrDefault(p =>
                    $"{p.Code} - {p.Name}".Equals(productText, StringComparison.OrdinalIgnoreCase));
                if (product != null)
                {
                    SelectedRefType = RefType.PRODUCT;
                    SelectedRefId = product.ProductId;
                    SelectedUnit = product.UnitMeasure ?? "-";
                    
                    // Cập nhật tồn kho (sẽ được cập nhật lại trong UpdateStockDisplay khi chọn lô)
                    UpdateStockDisplay();
                    
                    // Validate lại số lượng
                    ValidateQuantity();
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
                    SelectedRefType = RefType.PACKAGING;
                    SelectedRefId = packaging.PackagingId;
                    SelectedUnit = "cái";
                    _currentStock = 0; // Bao bì không có tồn kho
                    lblStock.Text = "Tồn kho: -";
                }
            }
        }

        private void UpdateBatchNoComboBox()
        {
            cmbBatch.Items.Clear();
            cmbBatch.Text = "";

            if (SelectedRefType.HasValue && SelectedRefId.HasValue)
            {
                List<string> batchNos = null;

                if (SelectedRefType == RefType.PRODUCT)
                {
                    if (_productBatchNos.ContainsKey(SelectedRefId.Value))
                    {
                        batchNos = _productBatchNos[SelectedRefId.Value];
                    }
                }
                else if (SelectedRefType == RefType.PACKAGING)
                {
                    if (_packagingBatchNos.ContainsKey(SelectedRefId.Value))
                    {
                        batchNos = _packagingBatchNos[SelectedRefId.Value];
                    }
                }

                if (batchNos != null && batchNos.Any())
                {
                    cmbBatch.Items.Add("-- Chọn lô hàng --");
                    cmbBatch.Items.AddRange(batchNos.ToArray());
                    cmbBatch.SelectedIndex = 0;
                }
            }
            
            // Reset tồn kho về tổng khi chưa chọn lô
            UpdateStockDisplay();
        }

        private async void cmbBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStockDisplay();
            ValidateQuantity();
        }

        private void UpdateStockDisplay()
        {
            if (SelectedRefType == RefType.PRODUCT && SelectedRefId.HasValue)
            {
                var batchNo = cmbBatch.Text.Trim();
                
                // Nếu đã chọn lô hàng cụ thể, hiển thị tồn kho của lô đó
                if (!string.IsNullOrWhiteSpace(batchNo) && batchNo != "-- Chọn lô hàng --")
                {
                    var key = $"{SelectedRefId.Value}_{batchNo}";
                    if (_batchStocks.ContainsKey(key))
                    {
                        _currentStock = _batchStocks[key];
                        lblStock.Text = $"{batchNo}: {_currentStock:N2} {SelectedUnit}";
                    }
                    else
                    {
                        // Lô không có trong cache, load từ database
                        LoadBatchStockAsync(SelectedRefId.Value, batchNo);
                    }
                }
                else
                {
                    // Chưa chọn lô hoặc chọn "-- Chọn lô hàng --", hiển thị tổng tồn kho
                    _currentStock = _productStocks.ContainsKey(SelectedRefId.Value) 
                        ? _productStocks[SelectedRefId.Value] 
                        : 0;
                    lblStock.Text = $"Tồn kho tổng: {_currentStock:N2} {SelectedUnit}";
                }
            }
            else
            {
                _currentStock = 0;
                lblStock.Text = "Tồn kho: -";
            }
        }

        private async void LoadBatchStockAsync(int productId, string batchNo)
        {
            try
            {
                // Load tồn kho của lô cụ thể từ database
                var inventoryResult = await AppServices.InventoryService.GetInventoryByProductAsync(productId);
                if (inventoryResult.Success && inventoryResult.Data != null)
                {
                    var batchInventory = inventoryResult.Data
                        .FirstOrDefault(inv => inv.BatchNo == batchNo);
                    
                    if (batchInventory != null)
                    {
                        _currentStock = batchInventory.Quantity;
                        var key = $"{productId}_{batchNo}";
                        _batchStocks[key] = batchInventory.Quantity;
                        lblStock.Text = $"Tồn kho lô {batchNo}: {_currentStock:N2} {SelectedUnit}";
                    }
                    else
                    {
                        _currentStock = 0;
                        lblStock.Text = $"Tồn kho lô {batchNo}: 0 {SelectedUnit}";
                    }
                    
                    ValidateQuantity();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading batch stock: {ex.Message}");
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            ValidateQuantity();
        }

        private void ValidateQuantity()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity))
            {
                if (!string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    ShowError("Số lượng không hợp lệ!");
                }
                return;
            }

            if (quantity <= 0)
            {
                ShowError("Số lượng phải lớn hơn 0!");
                return;
            }

            // Kiểm tra tồn kho chỉ cho sản phẩm
            if (SelectedRefType == RefType.PRODUCT && SelectedRefId.HasValue)
            {
                // Kiểm tra tồn kho theo lô nếu đã chọn lô, hoặc tổng tồn kho nếu chưa chọn lô
                if (quantity > _currentStock)
                {
                    var batchNo = cmbBatch.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(batchNo) && batchNo != "-- Chọn lô hàng --")
                    {
                        ShowError($"Không đủ tồn kho lô {batchNo}! Tồn kho hiện có: {_currentStock:N2} {SelectedUnit}");
                    }
                    else
                    {
                        ShowError($"Không đủ tồn kho! Tồn kho hiện có: {_currentStock:N2} {SelectedUnit}");
                    }
                }
            }
        }

        private void ShowError(string message)
        {
            labelError.Text = message;
            labelError.Visible = true;
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và dấu chấm thập phân
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Chỉ cho phép một dấu chấm thập phân
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            // Kiểm tra đã chọn sản phẩm/bao bì chưa
            if (cmbProduct.SelectedIndex <= 0)
            {
                ShowError("Vui lòng chọn sản phẩm hoặc bao bì!");
                cmbProduct.Focus();
                return false;
            }

            if (!SelectedRefType.HasValue || !SelectedRefId.HasValue)
            {
                ShowError("Sản phẩm/bao bì đã chọn không hợp lệ!");
                cmbProduct.Focus();
                return false;
            }

            if (SelectedRefType == RefType.PRODUCT)
            {
                if (string.IsNullOrWhiteSpace(cmbBatch.Text) || cmbBatch.Text == "-- Chọn lô hàng --")
                {
                    ShowError("Vui lòng chọn lô hàng!");
                    cmbBatch.Focus();
                    return false;
                }
            }

            // Kiểm tra số lượng
            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                ShowError("Số lượng phải lớn hơn 0!");
                txtQuantity.Focus();
                return false;
            }

            // Kiểm tra tồn kho chỉ cho sản phẩm
            if (SelectedRefType == RefType.PRODUCT && SelectedRefId.HasValue)
            {
                // Kiểm tra tồn kho theo lô nếu đã chọn lô, hoặc tổng tồn kho nếu chưa chọn lô
                if (quantity > _currentStock)
                {
                    var batchNo = cmbBatch.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(batchNo) && batchNo != "-- Chọn lô hàng --")
                    {
                        ShowError($"Không đủ tồn kho lô {batchNo}! Tồn kho hiện có: {_currentStock:N2} {SelectedUnit}");
                    }
                    else
                    {
                        ShowError($"Không đủ tồn kho! Tồn kho hiện có: {_currentStock:N2} {SelectedUnit}");
                    }
                    txtQuantity.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                // Lấy thông tin từ form và lưu vào properties
                Quantity = decimal.Parse(txtQuantity.Text);
                BatchNo = cmbBatch.Text.Trim();

                // Nếu chọn "-- Chọn lô hàng --" thì để trống
                if (BatchNo == "-- Chọn lô hàng --")
                {
                    BatchNo = "";
                }

                // Chỉ lưu thông tin, không xuất kho ngay
                // Form cha (MultiProductStockOutForm) sẽ thực sự xuất kho khi nhấn "Lưu phiếu"
                IsOK = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "lưu thông tin sản phẩm");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SingleProductStockOutForm_Load(object sender, EventArgs e)
        {
            // Auto-select text trong quantity khi form load
            txtQuantity.SelectAll();
        }

        private void SingleProductStockOutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnExport_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }

        private void cmbBatch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép nhập text tự do vào ComboBox
            // Không cần xử lý gì thêm
        }
    }
}