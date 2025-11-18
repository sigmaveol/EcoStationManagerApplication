using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EcoStationManagerApplication.UI.Controls
{
    public partial class StaffControl
    {
        #region Dashboard KPI Methods

        /// <summary>
        /// Load dashboard KPI statistics
        /// </summary>
        private async Task LoadDashboardKPIAsync()
        {
            try
            {
                var today = DateTime.Today;

                // Lấy số ca làm hôm nay
                var allShiftsResult = await AppServices.WorkShiftService.GetAllAsync();
                var todayShifts = allShiftsResult?.Data?.Where(s => s.ShiftDate.Date == today).Count() ?? 0;

                // Lấy số đơn đã giao thành công
                var deliveredResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.DELIVERED);
                var deliveredCount = deliveredResult?.Data?.Where(d => d.AssignedDate.Date == today).Count() ?? 0;

                // Lấy số đơn trễ (pending quá lâu)
                var pendingResult = await AppServices.DeliveryService.GetPendingDeliveriesAsync();
                var overdueCount = pendingResult?.Data?.Where(d => d.AssignedDate.Date < today).Count() ?? 0;

                // Tính tổng COD thu được hôm nay
                var allDeliveriesResult = await AppServices.DeliveryService.GetByDateRangeAsync(today, today);
                decimal totalCOD = 0;
                if (allDeliveriesResult?.Success == true && allDeliveriesResult.Data != null)
                {
                    totalCOD = allDeliveriesResult.Data
                        .Where(d => d.PaymentStatus == DeliveryPaymentStatus.PAID)
                        .Sum(d => d.CodAmount);
                }

                // Update UI labels/cards with these statistics
                if (lblTodayShifts != null)
                    lblTodayShifts.Text = $"Số ca làm hôm nay: {todayShifts}";
                if (lblDeliveredOrders != null)
                    lblDeliveredOrders.Text = $"Số đơn đã giao: {deliveredCount}";
                if (lblOverdueOrders != null)
                    lblOverdueOrders.Text = $"Số đơn trễ: {overdueCount}";
                if (lblTotalCOD != null)
                    lblTotalCOD.Text = $"COD: {totalCOD:N0} VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thống kê KPI: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load chart data for delivery status distribution
        /// </summary>
        private async Task LoadDeliveryStatusChartAsync()
        {
            try
            {
                var pendingResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.PENDING);
                var inTransitResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.INTRANSIT);
                var deliveredResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.DELIVERED);
                var failedResult = await AppServices.DeliveryService.GetByStatusAsync(DeliveryStatus.FAILED);

                var chartData = new Dictionary<string, int>
                {
                    { "Chờ giao", pendingResult?.Data?.Count() ?? 0 },
                    { "Đang giao", inTransitResult?.Data?.Count() ?? 0 },
                    { "Đã giao", deliveredResult?.Data?.Count() ?? 0 },
                    { "Thất bại", failedResult?.Data?.Count() ?? 0 }
                };

                // TODO: Update chart with chartData
                // chartDeliveryStatus.DataSource = chartData;
                // chartDeliveryStatus.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load KPI chart by staff/driver
        /// </summary>
        private async Task LoadKPIChartAsync()
        {
            try
            {
                var staffResult = await AppServices.UserService.GetActiveStaffAsync();
                var staffList = staffResult?.Data?.ToList() ?? new List<User>();

                var kpiData = new List<KeyValuePair<string, decimal>>();

                foreach (var staff in staffList)
                {
                    var shiftResult = await AppServices.WorkShiftService.GetCurrentShiftByUserIdAsync(staff.UserId);
                    if (shiftResult?.Success == true && shiftResult.Data != null && shiftResult.Data.KpiScore.HasValue)
                    {
                        kpiData.Add(new KeyValuePair<string, decimal>(
                            staff.Fullname ?? staff.Username,
                            shiftResult.Data.KpiScore.Value));
                    }
                }

                // TODO: Update chart with kpiData
                // chartKPI.DataSource = kpiData;
                // chartKPI.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải biểu đồ KPI: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}

