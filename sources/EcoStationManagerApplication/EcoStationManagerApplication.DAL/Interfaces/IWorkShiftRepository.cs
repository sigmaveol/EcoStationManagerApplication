using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IWorkShiftRepository : IRepository<WorkShift>
    {
        /// <summary>
        /// Lấy WorkShift theo user_id và ngày
        /// </summary>
        Task<WorkShift> GetByUserIdAndDateAsync(int userId, DateTime shiftDate);

        /// <summary>
        /// Lấy WorkShift hiện tại của user (hôm nay)
        /// </summary>
        Task<WorkShift> GetCurrentShiftByUserIdAsync(int userId);

        /// <summary>
        /// Lấy tất cả WorkShift của user trong khoảng thời gian
        /// </summary>
        Task<IEnumerable<WorkShift>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Kiểm tra xem user đã có ca làm việc trong ngày chưa
        /// </summary>
        Task<bool> ExistsByUserIdAndDateAsync(int userId, DateTime shiftDate);

        /// <summary>
        /// Cập nhật start_time khi bắt đầu ca
        /// </summary>
        Task<bool> UpdateStartTimeAsync(int shiftId, TimeSpan startTime);

        /// <summary>
        /// Cập nhật end_time khi kết thúc ca
        /// </summary>
        Task<bool> UpdateEndTimeAsync(int shiftId, TimeSpan endTime);
    }
}

