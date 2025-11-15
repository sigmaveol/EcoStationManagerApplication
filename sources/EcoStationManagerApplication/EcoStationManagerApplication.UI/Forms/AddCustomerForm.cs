using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class AddCustomerForm : Form
    {
        private int? _customerId = null;
        private bool _isEditMode => _customerId.HasValue;

        public AddCustomerForm() : this(null) { }

        public AddCustomerForm(int? customerId)
        {
            _customerId = customerId;
            InitializeComponent();
            InitializeForm();
        }

        private async void InitializeForm()
        {
            labelError.Text = "";
            labelError.Visible = false;
            LoadCustomerRanks();

            if (_isEditMode)
            {
                this.Text = lblTitle.Text = "Chỉnh sửa khách hàng";
                btnSave.Text = "Cập nhật";
                txtCustomerCode.ReadOnly = true;
                await LoadCustomerData(_customerId.Value);
            }
            else
            {
                this.Text = lblTitle.Text = "Thêm khách hàng mới";
                btnSave.Text = "Lưu";
                txtCustomerCode.ReadOnly = false;
            }
        }

        private async Task LoadCustomerData(int customerId)
        {
            try
            {
                var result = await AppServices.CustomerService.GetCustomerByIdAsync(customerId);
                if (result.Success && result.Data != null)
                {
                    var customer = result.Data;
                    txtCustomerCode.Text = customer.CustomerCode ?? "";
                    txtName.Text = customer.Name;
                    txtPhone.Text = customer.Phone ?? "";

                    var selectedRank = cmbRank.Items.OfType<ComboItem<CustomerRank>>()
                        .FirstOrDefault(x => x.Value == customer.Rank);
                    if (selectedRank != null)
                        cmbRank.SelectedItem = selectedRank;
                    else
                        cmbRank.SelectedIndex = 0;
                }
                else
                {
                    UIHelper.ShowErrorMessage("Không thể tải thông tin khách hàng.");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải thông tin khách hàng");
            }
        }

        private void LoadCustomerRanks()
        {
            cmbRank.Items.Clear();
            cmbRank.Items.Add(new ComboItem<CustomerRank> { Text = "Thành viên", Value = CustomerRank.MEMBER });
            cmbRank.Items.Add(new ComboItem<CustomerRank> { Text = "Bạc", Value = CustomerRank.SILVER });
            cmbRank.Items.Add(new ComboItem<CustomerRank> { Text = "Vàng", Value = CustomerRank.GOLD });
            cmbRank.Items.Add(new ComboItem<CustomerRank> { Text = "Kim cương", Value = CustomerRank.DIAMONDS });
            cmbRank.DisplayMember = "Text";
            cmbRank.ValueMember = "Value";
            cmbRank.SelectedIndex = 0;
        }

        private bool ValidateForm()
        {
            labelError.Text = "";
            labelError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtCustomerCode.Text))
            {
                ShowError("Vui lòng nhập mã khách hàng.");
                txtCustomerCode.Focus();
                return false;
            }

            if (txtCustomerCode.Text.Length > 50)
            {
                ShowError("Mã khách hàng không được vượt quá 50 ký tự.");
                txtCustomerCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowError("Vui lòng nhập tên khách hàng.");
                txtName.Focus();
                return false;
            }

            if (txtName.Text.Length > 255)
            {
                ShowError("Tên khách hàng không được vượt quá 255 ký tự.");
                txtName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && txtPhone.Text.Length > 50)
            {
                ShowError("Số điện thoại không được vượt quá 50 ký tự.");
                txtPhone.Focus();
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
            if (!ValidateForm()) return;

            // Kiểm tra mã khách hàng trùng
            var codeCheck = await AppServices.CustomerService.CheckCustomerCodeExistsAsync(
                txtCustomerCode.Text.Trim());
            if (codeCheck.Success && codeCheck.Data)
            {
                if (!_isEditMode || (_isEditMode && await IsCodeBelongsToOtherCustomer(txtCustomerCode.Text.Trim())))
                {
                    ShowError($"Mã khách hàng '{txtCustomerCode.Text.Trim()}' đã tồn tại.");
                    txtCustomerCode.Focus();
                    return;
                }
            }

            // Kiểm tra số điện thoại trùng (nếu có)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                var phoneCheck = await AppServices.CustomerService.CheckPhoneNumberExistsAsync(
                    txtPhone.Text.Trim());
                if (phoneCheck.Success && phoneCheck.Data)
                {
                    if (!_isEditMode || (_isEditMode && await IsPhoneBelongsToOtherCustomer(txtPhone.Text.Trim())))
                    {
                        ShowError($"Số điện thoại '{txtPhone.Text.Trim()}' đã tồn tại.");
                        txtPhone.Focus();
                        return;
                    }
                }
            }

            SetControlsEnabled(false);
            btnSave.Text = _isEditMode ? "Đang cập nhật..." : "Đang lưu...";

            try
            {
                var customerRank = (cmbRank.SelectedItem as ComboItem<CustomerRank>)?.Value ?? CustomerRank.MEMBER;

                var customer = new Customer
                {
                    CustomerCode = txtCustomerCode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    Rank = customerRank,
                    TotalPoint = 0, // Mặc định 0 điểm khi tạo mới
                    IsActive = ActiveStatus.ACTIVE
                };

                if (_isEditMode)
                {
                    customer.CustomerId = _customerId.Value;
                    // Giữ nguyên điểm tích lũy khi sửa
                    var existingResult = await AppServices.CustomerService.GetCustomerByIdAsync(_customerId.Value);
                    if (existingResult.Success && existingResult.Data != null)
                    {
                        customer.TotalPoint = existingResult.Data.TotalPoint;
                    }
                }

                bool success = false;
                string message = "";

                if (_isEditMode)
                {
                    var updateResult = await AppServices.CustomerService.UpdateCustomerAsync(customer);
                    success = updateResult.Success;
                    message = updateResult.Message ?? "Cập nhật khách hàng thành công!";
                }
                else
                {
                    var createResult = await AppServices.CustomerService.CreateCustomerAsync(customer);
                    success = createResult.Success;
                    message = createResult.Message ?? "Thêm khách hàng thành công!";
                    if (success) _customerId = createResult.Data;
                }

                if (success)
                {
                    UIHelper.ShowSuccessMessage(message);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError(message ?? (_isEditMode ? "Không thể cập nhật khách hàng." : "Không thể thêm khách hàng."));
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, _isEditMode ? "cập nhật khách hàng" : "thêm khách hàng");
                ShowError($"Đã xảy ra lỗi khi {(_isEditMode ? "cập nhật" : "thêm")} khách hàng. Vui lòng thử lại.");
            }
            finally
            {
                SetControlsEnabled(true);
                btnSave.Text = _isEditMode ? "Cập nhật" : "Lưu";
            }
        }

        private async Task<bool> IsCodeBelongsToOtherCustomer(string customerCode)
        {
            try
            {
                var result = await AppServices.CustomerService.GetCustomerByCodeAsync(customerCode);
                if (result.Success && result.Data != null)
                {
                    return result.Data.CustomerId != _customerId.Value;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IsPhoneBelongsToOtherCustomer(string phone)
        {
            try
            {
                // Tìm khách hàng theo số điện thoại
                var result = await AppServices.CustomerService.SearchCustomersAsync(phone);
                if (result.Success && result.Data != null)
                {
                    var customer = result.Data.FirstOrDefault(c => c.Phone == phone);
                    if (customer != null)
                    {
                        return customer.CustomerId != _customerId.Value;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtCustomerCode.Enabled = enabled;
            txtName.Enabled = enabled;
            txtPhone.Enabled = enabled;
            cmbRank.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }
    }

    public class ComboItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
        public override string ToString() => Text;
    }
}

