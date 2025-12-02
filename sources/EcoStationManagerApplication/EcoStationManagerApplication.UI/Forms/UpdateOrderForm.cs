using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using AppServices = EcoStationManagerApplication.UI.Common.AppServices;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class UpdateOrderForm : Form
    {
        private readonly int _orderId;
        private Order _order;

        public UpdateOrderForm(int orderId)
        {
            _orderId = orderId;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            InitializeComboBoxes();
            _ = LoadOrderAsync();
        }

        private void InitializeComboBoxes()
        {
            cmbStatus.DisplayMember = "Display";
            cmbStatus.ValueMember = "Value";
            cmbStatus.DataSource = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(status => new EnumOption<OrderStatus>(EnumHelper.GetDisplayName(status), status))
                .ToList();

            cmbPaymentStatus.DisplayMember = "Display";
            cmbPaymentStatus.ValueMember = "Value";
            cmbPaymentStatus.DataSource = Enum.GetValues(typeof(PaymentStatus))
                .Cast<PaymentStatus>()
                .Select(status => new EnumOption<PaymentStatus>(EnumHelper.GetDisplayName(status), status))
                .ToList();
        }

        private async Task LoadOrderAsync()
        {
            try
            {
                btnSave.Enabled = false;
                var orderResult = await AppServices.OrderService.GetOrderByIdAsync(_orderId);
                if (!orderResult.Success || orderResult.Data == null)
                {
                    MessageBox.Show(orderResult.Message ?? "Không tìm thấy đơn hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                _order = orderResult.Data;
                await PopulateOrderDataAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "tải thông tin đơn hàng");
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private async Task PopulateOrderDataAsync()
        {
            if (_order == null) return;

            lblOrderCodeValue.Text = _order.OrderCode ?? $"ORD-{_orderId:D5}";
            lblCustomerValue.Text = "Khách lẻ";

            if (_order.CustomerId.HasValue)
            {
                var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(_order.CustomerId.Value);
                if (customerResult.Success && customerResult.Data != null)
                {
                    lblCustomerValue.Text = customerResult.Data.Name ?? "Khách lẻ";
                }
            }

            txtNote.Text = _order.Note ?? string.Empty;

            cmbStatus.SelectedValue = _order.Status;
            cmbPaymentStatus.SelectedValue = _order.PaymentStatus;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_order == null) return;

                var selectedStatus = _order.Status;
                if (cmbStatus.SelectedValue is OrderStatus os)
                    selectedStatus = os;
                var selectedPaymentStatus = _order.PaymentStatus;
                if (cmbPaymentStatus.SelectedValue is PaymentStatus ps)
                    selectedPaymentStatus = ps;

                bool hasChanges = false;

                if (selectedPaymentStatus != _order.PaymentStatus)
                {
                    var updatePaymentResult = await AppServices.OrderService.UpdatePaymentStatusAsync(_orderId, selectedPaymentStatus);
                    if (!updatePaymentResult.Success)
                    {
                        MessageBox.Show(updatePaymentResult.Message ?? "Cập nhật trạng thái thanh toán thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    hasChanges = true;
                    _order.PaymentStatus = selectedPaymentStatus;
                }

                if (selectedStatus != _order.Status)
                {
                    var updateStatusResult = await AppServices.OrderService.UpdateOrderStatusAsync(_orderId, selectedStatus);
                    if (!updateStatusResult.Success)
                    {
                        MessageBox.Show(updateStatusResult.Message ?? "Cập nhật trạng thái đơn hàng thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    hasChanges = true;
                    _order.Status = selectedStatus;
                }

                if (!hasChanges)
                {
                    MessageBox.Show("Không có thay đổi nào để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show("Cập nhật đơn hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UIHelper.ShowExceptionError(ex, "cập nhật đơn hàng");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private class EnumOption<T>
        {
            public EnumOption(string display, T value)
            {
                Display = display;
                Value = value;
            }

            public string Display { get; }
            public T Value { get; }
        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

