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
    public partial class StockInForm : Form
    {
        private int? _productId = null;
        private int? _packagingId = null;
        private RefType _refType = RefType.PRODUCT;

        public StockInForm() : this(null, RefType.PRODUCT) { }

        public StockInForm(int? productId, RefType refType = RefType.PRODUCT)
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
            dtpStockInDate.Value = DateTime.Now;

            await LoadDataAsync();

            if (_productId.HasValue && _refType == RefType.PRODUCT)
            {
                var product = cmbItem.Items.OfType<ComboItem<ProductDTO>>()
                    .FirstOrDefault(x => x.Value.ProductId == _productId.Value);
                if (product != null)
                {
                    cmbItem.SelectedItem = product;
                    cmbItem.Enabled = false;
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

                // Load suppliers
                var suppliersResult = await AppServices.SupplierService.GetAllSuppliersAsync();
                if (suppliersResult.Success && suppliersResult.Data != null)
                {
                    cmbSupplier.Items.Clear();
                    cmbSupplier.Items.Add(new ComboItem<int?> { Text = "-- Chọn nhà cung cấp --", Value = null });
                    foreach (var supplier in suppliersResult.Data)
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

        private void cmbItem_SelectedIndexChanged(object sender, EventArgs e)
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
                var stockIn = new StockIn
                {
                    RefType = _refType,
                    Quantity = decimal.Parse(txtQuantity.Text),
                    Notes = txtNotes.Text,
                    CreatedDate = dtpStockInDate.Value,
                    BatchNo = txtBatchNo.Text
                };

                if (_refType == RefType.PRODUCT)
                {
                    var item = cmbItem.SelectedItem as ComboItem<ProductDTO>;
                    if (item != null)
                    {
                        stockIn.RefId = item.Value.ProductId;
                    }
                }
                else
                {
                    var item = cmbItem.SelectedItem as ComboItem<Packaging>;
                    if (item != null)
                    {
                        stockIn.RefId = item.Value.PackagingId;
                    }
                }

                if (cmbSupplier.SelectedItem != null)
                {
                    var supplier = cmbSupplier.SelectedItem as ComboItem<int?>;
                    if (supplier != null && supplier.Value.HasValue)
                    {
                        stockIn.SupplierId = supplier.Value;
                    }
                }

                if (dtpExpiryDate.Checked)
                {
                    stockIn.ExpiryDate = dtpExpiryDate.Value;
                }

                var result = await AppServices.StockInService.CreateStockInAsync(stockIn);
                if (result.Success)
                {
                    UIHelper.ShowSuccessMessage("Nhập kho thành công!");
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

