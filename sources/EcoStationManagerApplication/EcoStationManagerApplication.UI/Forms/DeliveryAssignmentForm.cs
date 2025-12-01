using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Common.Services;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class DeliveryAssignmentForm : Form
    {
        private List<OrderDTO> _orders;
        private List<User> _drivers;
        private int? _orderId;

        public DeliveryAssignmentForm(List<OrderDTO> orders, List<User> drivers, int? orderId = null)
        {
            _orders = orders ?? new List<OrderDTO>();
            _drivers = drivers ?? new List<User>();
            _orderId = orderId;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Phân công giao hàng";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new System.Drawing.Size(600, 400);

            LoadOrders();
            LoadDrivers();
        }

        private void LoadOrders()
        {
            cmbOrder.Items.Clear();
            foreach (var order in _orders)
            {
                cmbOrder.Items.Add($"{order.OrderCode} - {order.CustomerName ?? "Khách lẻ"}");
            }

            if (_orderId.HasValue)
            {
                var order = _orders.FirstOrDefault(o => o.OrderId == _orderId.Value);
                if (order != null)
                {
                    int index = _orders.IndexOf(order);
                    if (index >= 0 && index < cmbOrder.Items.Count)
                        cmbOrder.SelectedIndex = index;
                }
            }
            else if (cmbOrder.Items.Count > 0)
            {
                cmbOrder.SelectedIndex = 0;
            }
        }

        private void LoadDrivers()
        {
            cmbDriver.Items.Clear();
            foreach (var driver in _drivers)
            {
                cmbDriver.Items.Add($"{driver.Fullname ?? driver.Username} (ID: {driver.UserId})");
            }

            if (cmbDriver.Items.Count > 0)
                cmbDriver.SelectedIndex = 0;
        }

        private void cmbOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOrder.SelectedIndex >= 0 && cmbOrder.SelectedIndex < _orders.Count)
            {
                var order = _orders[cmbOrder.SelectedIndex];
                lblOrderInfo.Text = $"Mã đơn: {order.OrderCode}\nKhách hàng: {order.CustomerName ?? "Khách lẻ"}\nTổng tiền: {order.TotalAmount:N0} VNĐ";
            }
        }

        private async void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra SelectedIndex hợp lệ
                if (cmbOrder.SelectedIndex < 0 || cmbOrder.SelectedIndex >= _orders.Count)
                {
                    AppServices.Dialog.ShowWarning("Vui lòng chọn đơn hàng.", "Thông báo");
                    return;
                }

                if (cmbDriver.SelectedIndex < 0 || cmbDriver.SelectedIndex >= _drivers.Count)
                {
                    AppServices.Dialog.ShowWarning("Vui lòng chọn tài xế.", "Thông báo");
                    return;
                }

                var order = _orders[cmbOrder.SelectedIndex];
                var driver = _drivers[cmbDriver.SelectedIndex];

                // Kiểm tra xem đơn hàng đã được phân công chưa
                var existingAssignments = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.PENDING);
                var existing = existingAssignments?.Data?.FirstOrDefault(a => a.OrderId == order.OrderId);
                if (existing != null)
                {
                    var confirm = AppServices.Dialog.ShowConfirm(
                        $"Đơn hàng {order.OrderCode} đã được phân công cho tài xế khác. Bạn có muốn cập nhật không?",
                        "Xác nhận");

                    if (confirm)
                    {
                        existing.DriverId = driver.UserId;
                        existing.AssignedDate = DateTime.Now;
                        var updateResult = await AppServices.DeliveryService.UpdateAsync(existing);
                        if (updateResult?.Success == true)
                        {
                            AppServices.Dialog.ShowSuccess("Đã cập nhật phân công thành công.");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            AppServices.Dialog.ShowError($"Lỗi: {updateResult?.Message ?? "Không xác định"}");
                        }
                    }
                    return;
                }

                // Tạo phân công mới
                var assignment = new DeliveryAssignment
                {
                    OrderId = order.OrderId,
                    DriverId = driver.UserId,
                    AssignedDate = DateTime.Now,
                    Status = DeliveryStatus.PENDING,
                    CodAmount = 0, // Có thể lấy từ order nếu có
                    PaymentStatus = DeliveryPaymentStatus.UNPAID,
                    Notes = txtNotes.Text?.Trim() ?? ""
                };

                var result = await AppServices.DeliveryService.CreateAsync(assignment);
                if (result?.Success == true)
                {
                    AppServices.Dialog.ShowSuccess("Đã phân công giao hàng thành công.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    AppServices.Dialog.ShowError($"Lỗi: {result?.Message ?? "Không xác định"}");
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "phân công giao hàng");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region Designer Components
        private Label lblOrder;
        private Guna2ComboBox cmbOrder;
        private Label lblDriver;
        private Guna2ComboBox cmbDriver;
        private Label lblOrderInfo;
        private Label lblNotes;
        private Guna2TextBox txtNotes;
        private Guna2Button btnAssign;
        private Guna2Button btnCancel;
        #endregion
    }
}

