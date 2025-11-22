using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Common.Services;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Forms
{
    public partial class BatchDeliveryAssignmentForm : Form
    {
        private List<OrderDTO> _orders;
        private List<User> _drivers;
        private Dictionary<int, int> _orderDriverMap = new Dictionary<int, int>(); // OrderId -> DriverId

        public BatchDeliveryAssignmentForm(List<OrderDTO> orders, List<User> drivers)
        {
            _orders = orders ?? new List<OrderDTO>();
            _drivers = drivers ?? new List<User>();
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Phân công hàng loạt";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(900, 600);
            this.Padding = new Padding(20);

            LoadOrders();
            LoadDrivers();
        }

        private void LoadOrders()
        {
            dgvOrders.Columns.Clear();

            // Cột checkbox
            var checkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = "Chọn",
                Width = 50
            };
            dgvOrders.Columns.Add(checkColumn);

            // Cột mã đơn
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderCode",
                HeaderText = "Mã đơn",
                Width = 120,
                ReadOnly = true
            });

            // Cột khách hàng
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "Khách hàng",
                Width = 200,
                ReadOnly = true
            });

            // Cột tổng tiền
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalAmount",
                HeaderText = "Tổng tiền",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // Cột tài xế được phân công (ComboBox)
            var driverColumn = new DataGridViewComboBoxColumn
            {
                Name = "Driver",
                HeaderText = "Tài xế",
                Width = 200,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                DisplayStyleForCurrentCellOnly = false
            };
            dgvOrders.Columns.Add(driverColumn);

            // Cột trạng thái
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Trạng thái",
                Width = 100,
                ReadOnly = true
            });

            // Load dữ liệu
            foreach (var order in _orders)
            {
                var rowIndex = dgvOrders.Rows.Add(
                    false, // Selected
                    order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                    order.CustomerName ?? "Khách lẻ",
                    order.TotalAmount,
                    "", // Driver - sẽ set sau
                    "Chưa phân công"
                );

                dgvOrders.Rows[rowIndex].Tag = order;
            }

            // Load danh sách tài xế vào ComboBox column
            LoadDriversToComboBoxColumn();

            // Event handlers
            dgvOrders.CellValueChanged += DgvOrders_CellValueChanged;
            dgvOrders.CurrentCellDirtyStateChanged += DgvOrders_CurrentCellDirtyStateChanged;
        }

        private void LoadDriversToComboBoxColumn()
        {
            var driverColumn = dgvOrders.Columns["Driver"] as DataGridViewComboBoxColumn;
            if (driverColumn != null)
            {
                driverColumn.Items.Clear();
                driverColumn.Items.Add("-- Chưa chọn --");
                foreach (var driver in _drivers)
                {
                    driverColumn.Items.Add($"{driver.Fullname ?? driver.Username}");
                }
            }
        }

        private void LoadDrivers()
        {
            cmbDriver.Items.Clear();
            foreach (var driver in _drivers)
            {
                cmbDriver.Items.Add($"{driver.Fullname ?? driver.Username}");
            }

            if (cmbDriver.Items.Count > 0)
                cmbDriver.SelectedIndex = 0;
        }

        private void DgvOrders_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);
                UpdateStats();
            }
        }

        private void DgvOrders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvOrders.Rows[e.RowIndex];
            var order = row.Tag as OrderDTO;

            if (e.ColumnIndex == dgvOrders.Columns["Selected"].Index)
            {
                UpdateStats();
            }
            else if (e.ColumnIndex == dgvOrders.Columns["Driver"].Index)
            {
                var driverName = row.Cells["Driver"].Value?.ToString();
                if (!string.IsNullOrEmpty(driverName) && driverName != "-- Chưa chọn --")
                {
                    var driver = _drivers.FirstOrDefault(d =>
                        (d.Fullname ?? d.Username) == driverName);
                    if (driver != null)
                    {
                        _orderDriverMap[order.OrderId] = driver.UserId;
                        row.Cells["Status"].Value = "Đã phân công";
                    }
                }
                else
                {
                    _orderDriverMap.Remove(order.OrderId);
                    row.Cells["Status"].Value = "Chưa phân công";
                }
            }
        }

        private void UpdateStats()
        {
            int selectedCount = 0;
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                if (cell != null && cell.Value is bool isSelected && isSelected)
                    selectedCount++;
            }

            lblStats.Text = $"Đã chọn: {selectedCount} / Tổng: {_orders.Count}";
        }

        private async void BtnAssignSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDriver.SelectedIndex < 0 || cmbDriver.SelectedIndex >= _drivers.Count)
                {
                    AppServices.Dialog.ShowWarning("Vui lòng chọn tài xế.", "Thông báo");
                    return;
                }

                var selectedDriver = _drivers[cmbDriver.SelectedIndex];
                var selectedOrders = new List<OrderDTO>();

                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                    if (cell != null && cell.Value is bool isSelected && isSelected)
                    {
                        var order = row.Tag as OrderDTO;
                        selectedOrders.Add(order);
                    }
                }

                if (selectedOrders.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Vui lòng chọn ít nhất một đơn hàng.", "Thông báo");
                    return;
                }

                var confirm = AppServices.Dialog.ShowConfirm(
                    $"Bạn có chắc chắn muốn phân công {selectedOrders.Count} đơn hàng cho tài xế {selectedDriver.Fullname ?? selectedDriver.Username}?",
                    "Xác nhận phân công");

                if (!confirm) return;

                // Hiển thị progress
                var progressForm = new Form
                {
                    Text = "Đang phân công...",
                    Size = new Size(400, 100),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false
                };

                var progressBar = new ProgressBar
                {
                    Location = new Point(20, 20),
                    Size = new Size(360, 30),
                    Style = ProgressBarStyle.Continuous,
                    Maximum = selectedOrders.Count
                };

                var lblProgress = new Label
                {
                    Text = "Đang xử lý...",
                    Location = new Point(20, 60),
                    AutoSize = true
                };

                progressForm.Controls.Add(progressBar);
                progressForm.Controls.Add(lblProgress);
                progressForm.Show();
                progressForm.Refresh();

                int successCount = 0;
                int failCount = 0;

                foreach (var order in selectedOrders)
                {
                    try
                    {
                        // Kiểm tra xem đơn hàng đã được phân công chưa
                        var existingAssignments = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.PENDING);
                        var existing = existingAssignments?.Data?.FirstOrDefault(a => a.OrderId == order.OrderId);

                        if (existing != null)
                        {
                            // Cập nhật phân công hiện có
                            existing.DriverId = selectedDriver.UserId;
                            existing.AssignedDate = DateTime.Now;
                            var updateResult = await AppServices.DeliveryService.UpdateAsync(existing);
                            if (updateResult?.Success == true)
                                successCount++;
                            else
                                failCount++;
                        }
                        else
                        {
                            // Tạo phân công mới
                            var assignment = new DeliveryAssignment
                            {
                                OrderId = order.OrderId,
                                DriverId = selectedDriver.UserId,
                                AssignedDate = DateTime.Now,
                                Status = DeliveryStatus.PENDING,
                                CodAmount = 0,
                                PaymentStatus = DeliveryPaymentStatus.UNPAID,
                                Notes = ""
                            };

                            var result = await AppServices.DeliveryService.CreateAsync(assignment);
                            if (result?.Success == true)
                                successCount++;
                            else
                                failCount++;
                        }

                        progressBar.Value = successCount + failCount;
                        lblProgress.Text = $"Đã xử lý: {successCount + failCount} / {selectedOrders.Count}";
                        progressForm.Refresh();
                    }
                    catch
                    {
                        failCount++;
                    }
                }

                progressForm.Close();

                if (failCount == 0)
                {
                    AppServices.Dialog.ShowSuccess($"Đã phân công thành công {successCount} đơn hàng!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    AppServices.Dialog.ShowWarning(
                        $"Đã phân công thành công {successCount} đơn hàng.\nCó {failCount} đơn hàng phân công thất bại.",
                        "Kết quả phân công");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "phân công hàng loạt");
            }
        }

        private async void BtnAutoAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (_drivers.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có tài xế nào trong hệ thống.", "Thông báo");
                    return;
                }

                var selectedOrders = new List<OrderDTO>();
                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                    if (cell != null && cell.Value is bool isSelected && isSelected)
                    {
                        var order = row.Tag as OrderDTO;
                        selectedOrders.Add(order);
                    }
                }

                if (selectedOrders.Count == 0)
                {
                    // Nếu không chọn đơn nào, tự động chọn tất cả
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                        if (cell != null)
                            cell.Value = true;
                        var order = row.Tag as OrderDTO;
                        selectedOrders.Add(order);
                    }
                }

                if (selectedOrders.Count == 0)
                {
                    AppServices.Dialog.ShowWarning("Không có đơn hàng nào để phân công.", "Thông báo");
                    return;
                }

                var confirm = AppServices.Dialog.ShowConfirm(
                    $"Bạn có muốn phân công tự động {selectedOrders.Count} đơn hàng cho {_drivers.Count} tài xế (chia đều)?",
                    "Xác nhận phân công tự động");

                if (!confirm) return;

                // Phân công tự động: chia đều đơn hàng cho các tài xế
                int driverIndex = 0;
                int successCount = 0;
                int failCount = 0;

                foreach (var order in selectedOrders)
                {
                    var driver = _drivers[driverIndex % _drivers.Count];

                    try
                    {
                        var existingAssignments = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.PENDING);
                        var existing = existingAssignments?.Data?.FirstOrDefault(a => a.OrderId == order.OrderId);

                        if (existing != null)
                        {
                            existing.DriverId = driver.UserId;
                            existing.AssignedDate = DateTime.Now;
                            var updateResult = await AppServices.DeliveryService.UpdateAsync(existing);
                            if (updateResult?.Success == true)
                                successCount++;
                            else
                                failCount++;
                        }
                        else
                        {
                            var assignment = new DeliveryAssignment
                            {
                                OrderId = order.OrderId,
                                DriverId = driver.UserId,
                                AssignedDate = DateTime.Now,
                                Status = DeliveryStatus.PENDING,
                                CodAmount = 0,
                                PaymentStatus = DeliveryPaymentStatus.UNPAID,
                                Notes = "Phân công tự động"
                            };

                            var result = await AppServices.DeliveryService.CreateAsync(assignment);
                            if (result?.Success == true)
                                successCount++;
                            else
                                failCount++;
                        }

                        driverIndex++;
                    }
                    catch
                    {
                        failCount++;
                        driverIndex++;
                    }
                }

                if (failCount == 0)
                {
                    AppServices.Dialog.ShowSuccess($"Đã phân công tự động thành công {successCount} đơn hàng!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    AppServices.Dialog.ShowWarning(
                        $"Đã phân công thành công {successCount} đơn hàng.\nCó {failCount} đơn hàng phân công thất bại.",
                        "Kết quả phân công");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                AppServices.Dialog.ShowException(ex, "phân công tự động");
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                if (cell != null)
                    cell.Value = true;
            }
            UpdateStats();
        }

        private void BtnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                var cell = row.Cells["Selected"] as DataGridViewCheckBoxCell;
                if (cell != null)
                    cell.Value = false;
            }
            UpdateStats();
        }
    }
}