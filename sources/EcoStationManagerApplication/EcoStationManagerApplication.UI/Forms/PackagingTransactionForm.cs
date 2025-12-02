using EcoStationManagerApplication.Common.Helpers;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Common.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class PackagingTransactionForm : Form
    {
        private int _customerId;
        private PackagingTransactionType _transactionType;
        private Customer _customer;

        public PackagingTransactionForm(int customerId, PackagingTransactionType transactionType)
        {
            _customerId = customerId;
            _transactionType = transactionType;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            // Load thông tin khách hàng
            await LoadCustomerInfo();

            // Load danh sách bao bì
            await LoadPackagings();

            LoadOwnershipTypes();
            await LoadProducts();

            toggleIsProductPackaging.Checked = false;
            lblProduct.Visible = false;
            cmbProduct.Visible = false;

            // Thiết lập UI dựa trên loại giao dịch
            if (_transactionType == PackagingTransactionType.ISSUE)
            {
                this.Text = lblTitle.Text = "Phát bao bì cho khách hàng";
                lblAmount.Text = "Giá ký quỹ:";
                lblAmountDescription.Text = "Số tiền khách hàng cần trả khi nhận bao bì";
            }
            else
            {
                this.Text = lblTitle.Text = "Thu hồi bao bì từ khách hàng";
                lblAmount.Text = "Số tiền hoàn trả:";
                lblAmountDescription.Text = "Số tiền hoàn trả cho khách hàng";

                // Load số lượng bao bì khách hàng đang giữ
                await LoadCustomerHoldings();
            }

            // Thiết lập giá trị mặc định
            numQuantity.Value = 1;
            txtAmount.Text = "0";
            txtNotes.Text = "";
        }

        private async Task LoadCustomerInfo()
        {
            try
            {
                var result = await AppServices.CustomerService.GetCustomerByIdAsync(_customerId);
                if (result.Success && result.Data != null)
                {
                    _customer = result.Data;
                    lblCustomerInfo.Text = $"Khách hàng: {_customer.Name} ({_customer.CustomerCode ?? ""})";
                }
                else
                {
                    AppServices.Dialog.ShowError("Không thể tải thông tin khách hàng.");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "tải thông tin khách hàng");
            }
        }

        private async Task LoadPackagings()
        {
            try
            {
                cmbPackaging.Items.Clear();
                var result = await AppServices.PackagingService.GetAllPackagingsAsync();

                if (result.Success && result.Data != null)
                {
                    foreach (var packaging in result.Data.OrderBy(p => p.Name))
                    {
                        cmbPackaging.Items.Add(new ComboItem<Packaging>
                        {
                            Text = $"{packaging.Name} - {packaging.DepositPrice:N0} đ",
                            Value = packaging
                        });
                    }

                    cmbPackaging.DisplayMember = "Text";
                    cmbPackaging.ValueMember = "Value";

                    if (cmbPackaging.Items.Count > 0)
                    {
                        cmbPackaging.SelectedIndex = 0;
                        UpdateAmountFromSelectedPackaging();
                    }
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "tải danh sách bao bì");
            }
        }

        private async Task LoadCustomerHoldings()
        {
            try
            {
                var result = await AppServices.PackagingTransactionService.GetCustomerHoldingsAsync(_customerId);
                if (result.Success && result.Data != null && result.Data.Any())
                {
                    var holdings = result.Data.ToList();
                    var holdingsText = string.Join("\n", holdings.Select(h =>
                        $"- {h.PackagingName}: {h.HoldingQuantity} cái"));

                    lblHoldingsInfo.Text = $"Bao bì đang giữ:\n{holdingsText}";
                    lblHoldingsInfo.Visible = true;
                }
                else
                {
                    lblHoldingsInfo.Text = "Khách hàng không đang giữ bao bì nào.";
                    lblHoldingsInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "tải thông tin bao bì khách hàng đang giữ");
            }
        }

        private void LoadOwnershipTypes()
        {
            cmbOwnershipType.Items.Clear();
            cmbOwnershipType.Items.Add(new ComboItem<PackagingOwnershipType> { Text = "Ký quỹ", Value = PackagingOwnershipType.DEPOSIT });
            cmbOwnershipType.Items.Add(new ComboItem<PackagingOwnershipType> { Text = "Bán đứt", Value = PackagingOwnershipType.SOLD });
            cmbOwnershipType.DisplayMember = "Text";
            cmbOwnershipType.ValueMember = "Value";
            cmbOwnershipType.SelectedIndex = 0;
        }

        private async Task LoadProducts()
        {
            try
            {
                cmbProduct.Items.Clear();
                var result = await AppServices.ProductService.GetAllActiveProductsAsync();
                if (result.Success && result.Data != null)
                {
                    foreach (var product in result.Data.OrderBy(p => p.Name))
                    {
                        cmbProduct.Items.Add(new ComboItem<Product>
                        {
                            Text = product.Name,
                            Value = product
                        });
                    }
                    cmbProduct.DisplayMember = "Text";
                    cmbProduct.ValueMember = "Value";
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "tải danh sách sản phẩm");
            }
        }

        private void UpdateAmountFromSelectedPackaging()
        {
            if (cmbPackaging.SelectedItem is ComboItem<Packaging> selectedItem && selectedItem.Value != null)
            {
                var packaging = selectedItem.Value;
                if (_transactionType == PackagingTransactionType.ISSUE)
                {
                    txtAmount.Text = packaging.DepositPrice.ToString("N0");
                }
            }
        }

        private void cmbPackaging_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAmountFromSelectedPackaging();
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (cmbPackaging.SelectedItem == null)
            {
                ShowError("Vui lòng chọn bao bì.");
                cmbPackaging.Focus();
                return false;
            }

            if (numQuantity.Value <= 0)
            {
                ShowError("Số lượng phải lớn hơn 0.");
                numQuantity.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text.Replace(",", "").Replace(".", ""), out decimal amount) || amount < 0)
            {
                ShowError("Số tiền không hợp lệ.");
                txtAmount.Focus();
                return false;
            }

            if (cmbOwnershipType.SelectedItem == null)
            {
                ShowError("Vui lòng chọn hình thức sở hữu.");
                cmbOwnershipType.Focus();
                return false;
            }

            if (toggleIsProductPackaging.Checked)
            {
                if (cmbProduct.SelectedItem == null)
                {
                    ShowError("Vui lòng chọn sản phẩm liên kết.");
                    cmbProduct.Focus();
                    return false;
                }
            }

            // Kiểm tra số lượng bao bì khách hàng đang giữ (cho thu hồi)
            if (_transactionType == PackagingTransactionType.RETURN)
            {
                if (cmbPackaging.SelectedItem is ComboItem<Packaging> selectedItem && selectedItem.Value != null)
                {
                    var packaging = selectedItem.Value;
                    // Kiểm tra sẽ được thực hiện ở service layer
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
                if (!(cmbPackaging.SelectedItem is ComboItem<Packaging> selectedItem) || selectedItem.Value == null)
                {
                    ShowError("Vui lòng chọn bao bì.");
                    return;
                }

                var packaging = selectedItem.Value;
                var quantity = (int)numQuantity.Value;
                var amountText = txtAmount.Text.Replace(",", "").Replace(".", "");
                if (!decimal.TryParse(amountText, out decimal amount))
                {
                    ShowError("Số tiền không hợp lệ.");
                    return;
                }

                var notes = txtNotes.Text.Trim();
                var ownershipType = (cmbOwnershipType.SelectedItem as ComboItem<PackagingOwnershipType>)?.Value ?? PackagingOwnershipType.DEPOSIT;
                int? refProductId = null;
                var productItem = cmbProduct.SelectedItem as ComboItem<Product>;
                if (toggleIsProductPackaging.Checked && productItem != null && productItem.Value != null)
                {
                    refProductId = productItem.Value.ProductId;
                }

                if (_transactionType == PackagingTransactionType.ISSUE)
                {
                    // Phát bao bì
                    var result = await AppServices.PackagingTransactionService.IssuePackagingAsync(
                        packaging.PackagingId,
                        _customerId,
                        quantity,
                        amount,
                        AppServices.State.CurrentUser.UserId,
                        ownershipType,
                        refProductId,
                        notes
                    );

                    AppServices.Dialog.HandleServiceResult(result, (transaction) =>
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    });
                }
                else
                {
                    // Thu hồi bao bì
                    var result = await AppServices.PackagingTransactionService.ReturnPackagingAsync(
                        packaging.PackagingId,
                        _customerId,
                        quantity,
                        amount,
                        AppServices.State.CurrentUser.UserId,
                        ownershipType,
                        refProductId,
                        notes
                    );

                    AppServices.Dialog.HandleServiceResult(result, (transaction) =>
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    });
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "xử lý giao dịch bao bì");
            }
        }

        private void toggleIsProductPackaging_CheckedChanged(object sender, EventArgs e)
        {
            var isOn = toggleIsProductPackaging.Checked;
            lblProduct.Visible = isOn;
            cmbProduct.Visible = isOn;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

