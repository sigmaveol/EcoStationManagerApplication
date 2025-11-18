using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IWorkShiftService
    {
        /// <summary>
        /// Lấy tất cả ca làm việc
        /// </summary>
        Task<Result<IEnumerable<WorkShift>>> GetAllAsync();

        /// <summary>
        /// Lấy ca làm việc theo ID
        /// </summary>
        Task<Result<WorkShift>> GetByIdAsync(int shiftId);

        /// <summary>
        /// Lấy ca làm việc theo user và ngày
        /// </summary>
        Task<Result<WorkShift>> GetByUserIdAndDateAsync(int userId, DateTime shiftDate);

        /// <summary>
        /// Lấy ca làm việc hiện tại của user (hôm nay)
        /// </summary>
        Task<Result<WorkShift>> GetCurrentShiftByUserIdAsync(int userId);

        /// <summary>
        /// Lấy ca làm việc theo user trong khoảng thời gian
        /// </summary>
        Task<Result<IEnumerable<WorkShift>>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Tạo ca làm việc mới
        /// </summary>
        Task<Result<int>> CreateAsync(WorkShift workShift);

        /// <summary>
        /// Cập nhật ca làm việc
        /// </summary>
        Task<Result> UpdateAsync(WorkShift workShift);

        /// <summary>
        /// Cập nhật thời gian bắt đầu ca
        /// </summary>
        Task<Result> UpdateStartTimeAsync(int shiftId, TimeSpan startTime);

        /// <summary>
        /// Cập nhật thời gian kết thúc ca
        /// </summary>
        Task<Result> UpdateEndTimeAsync(int shiftId, TimeSpan endTime);

        /// <summary>
        /// Tính KPI cho ca làm việc
        /// </summary>
        Task<Result<decimal>> CalculateKPIAsync(int shiftId, int ordersHandled, int targetOrders = 20);

        /// <summary>
        /// Xóa ca làm việc
        /// </summary>
        Task<Result> DeleteAsync(int shiftId);
    }
}

