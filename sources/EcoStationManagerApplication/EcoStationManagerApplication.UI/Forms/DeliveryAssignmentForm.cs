using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
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
                    MessageBox.Show("Vui lòng chọn đơn hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbDriver.SelectedIndex < 0 || cmbDriver.SelectedIndex >= _drivers.Count)
                {
                    MessageBox.Show("Vui lòng chọn tài xế.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Đảm bảo index hợp lệ trước khi truy cập
                int orderIndex = cmbOrder.SelectedIndex;
                int driverIndex = cmbDriver.SelectedIndex;
                
                if (orderIndex < 0 || orderIndex >= _orders.Count || driverIndex < 0 || driverIndex >= _drivers.Count)
                {
                    MessageBox.Show("Lỗi: Dữ liệu không hợp lệ. Vui lòng thử lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var order = _orders[orderIndex];
                var driver = _drivers[driverIndex];

                // Kiểm tra xem đơn hàng đã được phân công chưa
                var existingAssignments = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.PENDING);
                var existing = existingAssignments?.Data?.FirstOrDefault(a => a.OrderId == order.OrderId);
                if (existing != null)
                {
                    var confirm = MessageBox.Show(
                        $"Đơn hàng {order.OrderCode} đã được phân công cho tài xế khác. Bạn có muốn cập nhật không?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        existing.DriverId = driver.UserId;
                        existing.AssignedDate = DateTime.Now;
                        var updateResult = await AppServices.DeliveryService.UpdateAsync(existing);
                        if (updateResult?.Success == true)
                        {
                            MessageBox.Show("Đã cập nhật phân công thành công.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi: {updateResult?.Message ?? "Không xác định"}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Đã phân công giao hàng thành công.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Lỗi: {result?.Message ?? "Không xác định"}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

