using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class StockOutForm : Form
    {
        private int? _productId = null;
        private int? _packagingId = null;
        private RefType _refType = RefType.PRODUCT;

        public StockOutForm() : this(null, RefType.PRODUCT) { }

        public StockOutForm(int? productId, RefType refType = RefType.PRODUCT)
        {
            _productId = productId;
            _refType = refType;
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
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Giao hàng", Value = StockOutPurpose.SALE });
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Điều chuyển trạm", Value = StockOutPurpose.TRANSFER });
            cmbPurpose.Items.Add(new ComboItem<StockOutPurpose> { Text = "Hủy hàng", Value = StockOutPurpose.DAMAGE });
            cmbPurpose.DisplayMember = "Text";
            cmbPurpose.ValueMember = "Value";
            cmbPurpose.SelectedIndex = 0;

            await LoadDataAsync();

            if (_productId.HasValue && _refType == RefType.PRODUCT)
            {
                var product = cmbItem.Items.OfType<ComboItem<ProductDTO>>()
                    .FirstOrDefault(x => x.Value.ProductId == _productId.Value);
                if (product != null)
                {
                    cmbItem.SelectedItem = product;
                    cmbItem.Enabled = false;
                    await LoadCurrentStock();
                }
            }
            else if (_packagingId.HasValue && _refType == RefType.PACKAGING)
            {
                var packaging = cmbItem.Items.OfType<ComboItem<Packaging>>()
                    .FirstOrDefault(x => x.Value.PackagingId == _packagingId.Value);
                if (packaging != null)
                {
                    cmbItem.SelectedItem = packaging;
                    cmbItem.Enabled = false;
                    await LoadCurrentStock();
                }
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                if (_refType == RefType.PRODUCT)
                {
                    lblItem.Text = "Sản phẩm:";
                    cmbType.Items.Clear();
                    cmbType.Items.Add("Sản phẩm");
                    cmbType.SelectedIndex = 0;
                    cmbType.Enabled = false;

                    var productsResult = await AppServices.ProductService.GetAllProductsAsync();
                    if (productsResult.Success && productsResult.Data != null)
                    {
                        cmbItem.Items.Clear();
                        foreach (var product in productsResult.Data.Where(p => p.IsActive == ActiveStatus.ACTIVE))
                        {
                            var dto = new ProductDTO
                            {
                                ProductId = product.ProductId,
                                Code = product.Sku ?? "",
                                Name = product.Name,
                                UnitMeasure = product.Unit
                            };
                            cmbItem.Items.Add(new ComboItem<ProductDTO> { Text = $"{dto.Code} - {dto.Name}", Value = dto });
                        }
                        cmbItem.DisplayMember = "Text";
                        cmbItem.ValueMember = "Value";
                    }
                }
                else
                {
                    lblItem.Text = "Bao bì:";
                    cmbType.Items.Clear();
                    cmbType.Items.Add("Bao bì");
                    cmbType.SelectedIndex = 0;
                    cmbType.Enabled = false;

                    var packagingsResult = await AppServices.PackagingService.GetAllPackagingsAsync();
                    if (packagingsResult.Success && packagingsResult.Data != null)
                    {
                        cmbItem.Items.Clear();
                        foreach (var packaging in packagingsResult.Data)
                        {
                            cmbItem.Items.Add(new ComboItem<Packaging> { Text = $"{packaging.Barcode} - {packaging.Name}", Value = packaging });
                        }
                        cmbItem.DisplayMember = "Text";
                        cmbItem.ValueMember = "Value";
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải dữ liệu");
            }
        }

        private async Task LoadCurrentStock()
        {
            try
            {
                if (cmbItem.SelectedItem == null)
                {
                    lblCurrentStock.Text = "Tồn kho hiện tại: -";
                    return;
                }

                if (_refType == RefType.PRODUCT)
                {
                    var item = cmbItem.SelectedItem as ComboItem<ProductDTO>;
                    if (item != null)
                    {
                        var inventoryResult = await AppServices.InventoryService.GetInventoryByProductAsync(item.Value.ProductId);
                        if (inventoryResult.Success && inventoryResult.Data != null)
                        {
                            var totalQty = inventoryResult.Data.Sum(inv => inv.Quantity);
                            lblCurrentStock.Text = $"Tồn kho hiện tại: {totalQty} {item.Value.UnitMeasure ?? ""}";
                        }
                        else
                        {
                            lblCurrentStock.Text = "Tồn kho hiện tại: 0";
                        }
                    }
                }
                else
                {
                    var item = cmbItem.SelectedItem as ComboItem<Packaging>;
                    if (item != null)
                    {
                        var inventoryResult = await AppServices.PackagingInventoryService.GetPackagingInventoryAsync(item.Value.PackagingId);
                        if (inventoryResult.Success && inventoryResult.Data != null)
                        {
                            var inv = inventoryResult.Data;
                            var totalQty = inv.QtyNew + inv.QtyInUse + inv.QtyReturned + inv.QtyCleaned;
                            lblCurrentStock.Text = $"Tồn kho hiện tại: {totalQty} Cái";
                        }
                        else
                        {
                            lblCurrentStock.Text = "Tồn kho hiện tại: 0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải tồn kho");
                lblCurrentStock.Text = "Tồn kho hiện tại: -";
            }
        }

        private async void cmbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItem.SelectedItem != null)
            {
                if (_refType == RefType.PRODUCT)
                {
                    var item = cmbItem.SelectedItem as ComboItem<ProductDTO>;
                    if (item != null)
                    {
                        lblUnit.Text = $"Đơn vị: {item.Value.UnitMeasure ?? "-"}";
                    }
                }
                else
                {
                    lblUnit.Text = "Đơn vị: Cái";
                }
                await LoadCurrentStock();
            }
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (cmbItem.SelectedItem == null)
            {
                ShowError("Vui lòng chọn sản phẩm/bao bì.");
                cmbItem.Focus();
                return false;
            }

            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                ShowError("Số lượng phải lớn hơn 0.");
                txtQuantity.Focus();
                return false;
            }

            // TODO: Kiểm tra tồn kho có đủ không
            // if (quantity > currentStock) { ... }

            if (cmbPurpose.SelectedIndex < 0)
            {
                ShowError("Vui lòng chọn mục đích xuất.");
                cmbPurpose.Focus();
                return false;
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
                var purpose = StockOutPurpose.SALE;
                if (cmbPurpose.SelectedItem != null)
                {
                    var purposeItem = cmbPurpose.SelectedItem as ComboItem<StockOutPurpose>;
                    if (purposeItem != null)
                    {
                        purpose = purposeItem.Value;
                    }
                }

                var stockOut = new StockOut
                {
                    RefType = _refType,
                    Quantity = decimal.Parse(txtQuantity.Text),
                    Notes = txtNotes.Text,
                    CreatedDate = dtpStockOutDate.Value,
                    Purpose = purpose,
                    BatchNo = txtBatchNo.Text
                };

                if (_refType == RefType.PRODUCT)
                {
                    var item = cmbItem.SelectedItem as ComboItem<ProductDTO>;
                    if (item != null)
                    {
                        stockOut.RefId = item.Value.ProductId;
                    }
                }
                else
                {
                    var item = cmbItem.SelectedItem as ComboItem<Packaging>;
                    if (item != null)
                    {
                        stockOut.RefId = item.Value.PackagingId;
                    }
                }

                var result = await AppServices.StockOutService.CreateStockOutAsync(stockOut);
                if (result.Success)
                {
                    UIHelper.ShowSuccessMessage("Xuất kho thành công!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    UIHelper.HandleServiceResult(result);
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

