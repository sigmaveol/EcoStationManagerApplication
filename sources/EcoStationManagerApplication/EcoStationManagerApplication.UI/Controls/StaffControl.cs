using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using EcoStationManagerApplication.UI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
// using EcoStationManagerApplication.UI.Forms; s

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl : UserControl
    {
        private readonly BindingList<DeliveryAssignmentRow> _deliverySource = new BindingList<DeliveryAssignmentRow>();
        private readonly BindingList<WorkShiftRow> _workShiftSource = new BindingList<WorkShiftRow>();
        private bool _isLoadingDashboard;

        public StaffControl()
        {
            InitializeComponent();

            SetupDataGridStyle(dgvAssignments);
            SetupDataGridStyle(dgvKPI);

            InitializeDataGridColumns();
            InitializeEvents();
            InitializeFilters();
            dgvAssignments.DataSource = _deliverySource;
            dgvKPI.DataSource = _workShiftSource;

            if (!IsInDesignMode())
            {
                _ = LoadDashboardAsync();
            }
        }

        // Gán tất cả sự kiện ở đây
        private void InitializeEvents()
        {
            // Delivery Assignment Events
            if (btnAssignDelivery != null)
                btnAssignDelivery.Click += btnAssignDelivery_Click;
            if (btnUpdateDeliveryStatus != null)
                btnUpdateDeliveryStatus.Click += btnUpdateDeliveryStatus_Click;
            if (btnExportDeliveryExcel != null)
                btnExportDeliveryExcel.Click += btnExportDeliveryExcel_Click;
            if (btnExportDeliveryPdf != null)
                btnExportDeliveryPdf.Click += btnExportDeliveryPdf_Click;
            if (txtDeliverySearch != null)
                txtDeliverySearch.TextChanged += (s, e) => { _deliverySearchText = txtDeliverySearch.Text ?? ""; FilterDeliveryData(); };
            if (cmbDeliveryStatusFilter != null)
                cmbDeliveryStatusFilter.SelectedIndexChanged += (s, e) => 
                {
                    if (cmbDeliveryStatusFilter.SelectedIndex > 0)
                    {
                        var statusText = cmbDeliveryStatusFilter.SelectedItem.ToString();
                        _deliveryFilterStatus = statusText == "Chờ giao" ? DeliveryStatus.PENDING :
                            statusText == "Đang giao" ? DeliveryStatus.INTRANSIT :
                            statusText == "Đã giao" ? DeliveryStatus.DELIVERED :
                            statusText == "Thất bại" ? DeliveryStatus.FAILED : (DeliveryStatus?)null;
                    }
                    else
                        _deliveryFilterStatus = null;
                    FilterDeliveryData();
                };
            if (dtpDeliveryDateFilter != null)
                dtpDeliveryDateFilter.ValueChanged += (s, e) => { _deliveryFilterDate = dtpDeliveryDateFilter.Value.Date; FilterDeliveryData(); };

            // WorkShift Events
            if (btnAddWorkShift != null)
                btnAddWorkShift.Click += btnAddWorkShift_Click;
            if (btnEditWorkShift != null)
                btnEditWorkShift.Click += btnEditWorkShift_Click;
            if (btnDeleteWorkShift != null)
                btnDeleteWorkShift.Click += btnDeleteWorkShift_Click;
            if (btnExportWorkShiftExcel != null)
                btnExportWorkShiftExcel.Click += btnExportWorkShiftExcel_Click;
            if (btnExportWorkShiftPdf != null)
                btnExportWorkShiftPdf.Click += btnExportWorkShiftPdf_Click;
            if (txtWorkShiftSearch != null)
                txtWorkShiftSearch.TextChanged += (s, e) => { _workShiftSearchText = txtWorkShiftSearch.Text ?? ""; FilterWorkShiftData(); };
            if (cmbWorkShiftRoleFilter != null)
                cmbWorkShiftRoleFilter.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbWorkShiftRoleFilter.SelectedIndex > 0)
                    {
                        var roleText = cmbWorkShiftRoleFilter.SelectedItem.ToString();
                        _workShiftFilterRole = roleText == "Quản trị viên" ? UserRole.ADMIN :
                            roleText == "Quản lý trạm" ? UserRole.MANAGER :
                            roleText == "Nhân viên" ? UserRole.STAFF :
                            roleText == "Tài xế" ? UserRole.DRIVER : (UserRole?)null;
                    }
                    else
                        _workShiftFilterRole = null;
                    FilterWorkShiftData();
                };
            if (dtpWorkShiftDateFilter != null)
                dtpWorkShiftDateFilter.ValueChanged += (s, e) => { _workShiftFilterDate = dtpWorkShiftDateFilter.Value.Date; FilterWorkShiftData(); };

            if (dgvAssignments != null)
            {
                dgvAssignments.CellFormatting += dgvAssignments_CellFormatting;
                dgvAssignments.CellDoubleClick += dgvAssignments_CellDoubleClick;
                dgvAssignments.ColumnHeaderMouseClick += dgvAssignments_ColumnHeaderMouseClick;
            }

            if (dgvKPI != null)
            {
                dgvKPI.CellFormatting += dgvKPI_CellFormatting;
                dgvKPI.ColumnHeaderMouseClick += dgvKPI_ColumnHeaderMouseClick;
            }

            this.Load += async (s, e) =>
            {
                if (!IsInDesignMode() && !_isLoadingDashboard)
                {
                    await LoadDashboardAsync();
                }
            };
        }

        // Khởi tạo các bộ lọc
        private void InitializeFilters()
        {
            if (cmbDeliveryStatusFilter != null && cmbDeliveryStatusFilter.Items.Count > 0)
                cmbDeliveryStatusFilter.SelectedIndex = 0;
            if (cmbWorkShiftRoleFilter != null && cmbWorkShiftRoleFilter.Items.Count > 0)
                cmbWorkShiftRoleFilter.SelectedIndex = 0;
        }

        // Thêm cột cho các DataGridView (Designer không thể xử lý vòng lặp)
        private void InitializeDataGridColumns()
        {
            dgvAssignments.AutoGenerateColumns = false;
            dgvKPI.AutoGenerateColumns = false;
            dgvAssignments.Columns.Clear();
            dgvKPI.Columns.Clear();

            // --- Cột cho Bảng Phân công ---
            var columnsAssignments = new[]
            {
                new { Name = "OrderCode", Header = "Mã đơn", FillWeight = 12 },
                new { Name = "CustomerName", Header = "Tên khách", FillWeight = 15 },
                new { Name = "Address", Header = "Địa chỉ giao", FillWeight = 20 },
                new { Name = "DriverName", Header = "Tài xế", FillWeight = 12 },
                new { Name = "Status", Header = "Trạng thái", FillWeight = 10 },
                new { Name = "CodAmount", Header = "COD", FillWeight = 10 },
                new { Name = "PaymentStatus", Header = "Thanh toán", FillWeight = 10 },
                new { Name = "AssignedDate", Header = "Ngày phân công", FillWeight = 11 }
            };

            foreach (var col in columnsAssignments)
            {
                dgvAssignments.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    DataPropertyName = col.Name,
                    ReadOnly = true
                });
            }

            // --- Cột cho Bảng KPI ---
            var columnsKPI = new[]
            {
                new { Name = "StaffName", Header = "Tên nhân viên", FillWeight = 15 },
                new { Name = "Role", Header = "Vai trò", FillWeight = 10 },
                new { Name = "StationName", Header = "Trạm làm việc", FillWeight = 15 },
                new { Name = "ShiftDate", Header = "Ngày ca", FillWeight = 12 },
                new { Name = "StartTime", Header = "Giờ bắt đầu", FillWeight = 10 },
                new { Name = "EndTime", Header = "Giờ kết thúc", FillWeight = 10 },
                new { Name = "KpiScore", Header = "KPI (%)", FillWeight = 10 },
                new { Name = "Notes", Header = "Ghi chú", FillWeight = 18 }
            };

            foreach (var col in columnsKPI)
            {
                dgvKPI.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    FillWeight = col.FillWeight,
                    DataPropertyName = col.Name,
                    ReadOnly = true
                });
            }
        }

        // --- HÀM XỬ LÝ SỰ KIỆN ---

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form thêm nhân viên mới", "Thêm nhân viên");
            // using (var addStaffForm = new AddStaffForm()) // Tên form giả định
            // {
            //     addStaffForm.ShowDialog();
            // }
        }

        // --- HÀM LOGIC & DỮ LIỆU ---
        private async Task LoadDashboardAsync()
        {
            if (_isLoadingDashboard)
                return;

            try
            {
                _isLoadingDashboard = true;
                SetLoadingState(true);

                var deliveryTask = LoadDeliveryAssignmentsAsync();
                var shiftTask = LoadWorkShiftDataAsync();
                var dashboardTask = LoadDashboardKPIAsync();

                await Task.WhenAll(deliveryTask, shiftTask, dashboardTask);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu nhân sự: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                _isLoadingDashboard = false;
            }
        }

        private async Task LoadDeliveryAssignmentsAsync()
        {
            _deliverySource.Clear();

            try
            {
                // Lấy các phân công giao hàng đang chờ và đang giao
                var pendingResult = await AppServices.DeliveryService.GetPendingDeliveriesAsync();
                var inTransitResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.INTRANSIT);

                var allAssignments = new List<DeliveryAssignment>();
                if (pendingResult?.Success == true && pendingResult.Data != null)
                    allAssignments.AddRange(pendingResult.Data);
                if (inTransitResult?.Success == true && inTransitResult.Data != null)
                    allAssignments.AddRange(inTransitResult.Data);

                if (!allAssignments.Any())
                {
                    // Nếu không có phân công, lấy các đơn hàng đang xử lý để có thể phân công
                    var ordersResult = await AppServices.OrderService.GetProcessingOrdersAsync();
                    if (ordersResult?.Success == true && ordersResult.Data != null)
                    {
                        var orders = ordersResult.Data.Take(30).ToList();
                        foreach (var order in orders)
                        {
                            _deliverySource.Add(new DeliveryAssignmentRow
                            {
                                OrderId = order.OrderId,
                                AssignmentId = 0,
                                OrderCode = order.OrderCode ?? "N/A",
                                CustomerName = order.CustomerName ?? "Khách lẻ",
                                Address = "-",
                                DriverName = "Chưa phân công",
                                Status = GetDeliveryStatusDisplay(DeliveryStatus.PENDING),
                                CodAmount = 0,
                                PaymentStatus = "Chưa xác định",
                                AssignedDate = "-"
                            });
                        }
                    }
                    return;
                }

                // Lấy thông tin đơn hàng và tài xế
                var driversResult = await AppServices.UserService.GetActiveDriversAsync();
                var driverDict = driversResult?.Data?.ToDictionary(d => d.UserId, d => d) ?? new Dictionary<int, User>();

                foreach (var assignment in allAssignments.Take(50))
                {
                    var orderResult = await AppServices.OrderService.GetOrderByIdAsync(assignment.OrderId);
                    var order = orderResult?.Data;
                    var driver = driverDict.ContainsKey(assignment.DriverId) ? driverDict[assignment.DriverId] : null;

                    var staffName = driver?.Fullname ?? driver?.Username ?? "Chưa phân công";
                    var shiftStart = assignment.AssignedDate;
                    var shiftEnd = shiftStart.AddHours(4); // Mặc định ca 4 giờ

                    // Lấy thông tin ca làm việc của tài xế
                    var shiftResult = await AppServices.WorkShiftService.GetByUserIdAndDateAsync(assignment.DriverId, assignment.AssignedDate.Date);
                    if (shiftResult?.Success == true && shiftResult.Data != null)
                    {
                        var shift = shiftResult.Data;
                        if (shift.StartTime.HasValue)
                            shiftStart = assignment.AssignedDate.Date.Add(shift.StartTime.Value);
                        if (shift.EndTime.HasValue)
                            shiftEnd = assignment.AssignedDate.Date.Add(shift.EndTime.Value);
                    }

                    // Lấy tên khách hàng
                    string customerName = "Khách lẻ";
                    if (order != null && order.CustomerId.HasValue)
                    {
                        var customerResult = await AppServices.CustomerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult?.Success == true && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name ?? "Khách lẻ";
                        }
                    }

                    _deliverySource.Add(new DeliveryAssignmentRow
                    {
                        OrderId = assignment.OrderId,
                        AssignmentId = assignment.AssignmentId,
                        OrderCode = order?.OrderCode ?? "N/A",
                        CustomerName = customerName,
                        Address = order?.Address ?? "-",
                        DriverName = staffName,
                        Status = GetDeliveryStatusDisplay(assignment.Status),
                        CodAmount = assignment.CodAmount,
                        PaymentStatus = GetPaymentStatusDisplay(assignment.PaymentStatus),
                        AssignedDate = assignment.AssignedDate.ToString("dd/MM/yyyy HH:mm")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải phân công giao hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadWorkShiftDataAsync()
        {
            _workShiftSource.Clear();

            try
            {
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                var staffList = staffResult?.Data?.ToList() ?? new List<User>();
                if (!staffList.Any())
                    return;

                var today = DateTime.Today;
                var todayOrdersResult = await AppServices.OrderService.GetTodayOrdersAsync();
                var todayOrders = todayOrdersResult?.Data?.ToList() ?? new List<Order>();

                // Lấy danh sách stations để map với manager
                var stationsResult = await AppServices.StationService.GetAllStationsAsync();
                var stations = stationsResult?.Data?.ToList() ?? new List<Station>();
                var stationDict = stations.Where(s => s.Manager.HasValue)
                    .ToDictionary(s => s.Manager.Value, s => s.Name);

                foreach (var staff in staffList)
                {
                    // Lấy ca làm việc hôm nay của nhân viên
                    var shiftResult = await AppServices.WorkShiftService.GetCurrentShiftByUserIdAsync(staff.UserId);
                    WorkShift shift = null;
                    DateTime shiftStart = today.AddHours(8); // Mặc định 8h
                    DateTime shiftEnd = today.AddHours(17); // Mặc định 17h

                    if (shiftResult?.Success == true && shiftResult.Data != null)
                    {
                        shift = shiftResult.Data;
                        if (shift.StartTime.HasValue)
                            shiftStart = today.Add(shift.StartTime.Value);
                        if (shift.EndTime.HasValue)
                            shiftEnd = today.Add(shift.EndTime.Value);
                    }

                    // Đếm số đơn hàng nhân viên đã xử lý hôm nay
                    var ordersHandled = todayOrders.Where(o => o.UserId == staff.UserId).Count();

                    var status = DetermineShiftStatus(shiftStart, shiftEnd);
                    string kpiPercent = "-";

                    if (shift != null && shift.KpiScore.HasValue)
                    {
                        kpiPercent = $"{shift.KpiScore.Value:F1}%";
                    }
                    else if (ordersHandled > 0 && shift != null && shift.ShiftId > 0)
                    {
                        // Tính KPI nếu chưa có
                        var kpiResult = await AppServices.WorkShiftService.CalculateKPIAsync(shift.ShiftId, ordersHandled, 20);
                        if (kpiResult?.Success == true)
                        {
                            kpiPercent = $"{kpiResult.Data:F1}%";
                        }
                    }
                    else if (ordersHandled > 0)
                    {
                        // Tính KPI tạm thời (không lưu vào DB)
                        var kpi = Math.Min(100, (decimal)ordersHandled / 20 * 100);
                        kpiPercent = $"{kpi:F1}%";
                    }

                    // Lấy tên trạm (nếu user là manager của một trạm)
                    string stationName = "-";
                    if (stationDict.ContainsKey(staff.UserId))
                    {
                        stationName = stationDict[staff.UserId];
                    }

                    // Lấy role display name
                    string roleDisplay = GetRoleDisplayName(staff.Role);

                    _workShiftSource.Add(new WorkShiftRow
                    {
                        ShiftId = shift?.ShiftId ?? 0,
                        UserId = staff.UserId,
                        StaffName = staff.Fullname ?? staff.Username,
                        Role = roleDisplay,
                        StationName = stationName,
                        ShiftDate = shift != null ? shift.ShiftDate.ToString("dd/MM/yyyy") : today.ToString("dd/MM/yyyy"),
                        StartTime = shift?.StartTime.HasValue == true ? shift.StartTime.Value.ToString(@"hh\:mm") : "-",
                        EndTime = shift?.EndTime.HasValue == true ? shift.EndTime.Value.ToString(@"hh\:mm") : "-",
                        KpiScore = kpiPercent,
                        Notes = shift?.Notes ?? "-"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu ca làm việc: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CÁC HÀM HELPER (Hàm phụ trợ) ---
        private void SetLoadingState(bool isLoading)
        {
            if (IsInDesignMode())
                return;

            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private bool IsInDesignMode()
        {
            return DesignMode ||
                   LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   (Site?.DesignMode ?? false);
        }

        private string GetOrderStatusDisplay(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Đã xác nhận";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Sẵn sàng";
                case OrderStatus.SHIPPED:
                    return "Đang giao";
                case OrderStatus.COMPLETED:
                    return "Đã hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return status.ToString();
            }
        }

        private string GetDeliveryStatusDisplay(DeliveryStatus status)
        {
            switch (status)
            {
                case DeliveryStatus.PENDING:
                    return "Chờ giao";
                case DeliveryStatus.INTRANSIT:
                    return "Đang giao";
                case DeliveryStatus.DELIVERED:
                    return "Đã giao";
                case DeliveryStatus.FAILED:
                    return "Thất bại";
                default:
                    return status.ToString();
            }
        }

        private string GetPaymentStatusDisplay(DeliveryPaymentStatus status)
        {
            switch (status)
            {
                case DeliveryPaymentStatus.UNPAID:
                    return "Chưa thanh toán";
                case DeliveryPaymentStatus.PAID:
                    return "Đã thanh toán";
                default:
                    return status.ToString();
            }
        }

        private string DetermineShiftStatus(DateTime shiftStart, DateTime shiftEnd)
        {
            var now = DateTime.Now;
            if (now < shiftStart)
                return "Chưa bắt đầu";

            if (now >= shiftStart && now <= shiftEnd)
                return "Đang làm";

            return "Đã kết thúc";
        }

        private string CalculateKpiPercent(int ordersHandled)
        {
            var kpi = Math.Min(100, 60 + ordersHandled * 5);
            return $"{kpi}%";
        }

        private string GetRoleDisplayName(UserRole role)
        {
            return RolePermissionHelper.GetRoleDisplayName(role);
        }

        // Hàm áp dụng style chung cho DataGridView
        private void SetupDataGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(240, 240, 240);
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 245, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;
        }

        // Hàm tô màu cho các ô
        private Color GetBadgeColor(string status)
        {
            switch (status)
            {
                case "Đang giao": return Color.FromArgb(200, 230, 201); // Xanh lá
                case "Đang làm": return Color.FromArgb(187, 222, 251); // Xanh dương
                case "Tốt": return Color.Green;
                case "Khá": return Color.Orange;
                default: return Color.LightGray;
            }
        }

        private void dgvAssignments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAssignments.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.BackColor = GetBadgeColor(status);
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void dgvKPI_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKPI.Columns[e.ColumnIndex].Name == "Rating")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    e.CellStyle.ForeColor = GetBadgeColor(status);
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }

        private class DeliveryAssignmentRow
        {
            public int OrderId { get; set; }
            public int AssignmentId { get; set; }
            public string OrderCode { get; set; }
            public string CustomerName { get; set; }
            public string Address { get; set; }
            public string DriverName { get; set; }
            public string Status { get; set; }
            public decimal CodAmount { get; set; }
            public string PaymentStatus { get; set; }
            public string AssignedDate { get; set; }
        }

        private class WorkShiftRow
        {
            public int ShiftId { get; set; }
            public int UserId { get; set; }
            public string StaffName { get; set; }
            public string Role { get; set; }
            public string StationName { get; set; }
            public string ShiftDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string KpiScore { get; set; }
            public string Notes { get; set; }
        }
    }
}