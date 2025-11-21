using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface ICleaningScheduleService
    {
        /// <summary>
        /// Lấy tất cả lịch vệ sinh
        /// </summary>
        Task<Result<IEnumerable<CleaningSchedule>>> GetAllAsync();

        /// <summary>
        /// Lấy lịch vệ sinh theo ID
        /// </summary>
        Task<Result<CleaningSchedule>> GetByIdAsync(int cleaningScheduleId);

        /// <summary>
        /// Lấy lịch vệ sinh sắp đến hạn
        /// </summary>
        Task<Result<IEnumerable<CleaningSchedule>>> GetUpcomingSchedulesAsync(DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Lấy lịch vệ sinh quá hạn
        /// </summary>
        Task<Result<IEnumerable<CleaningSchedule>>> GetOverdueSchedulesAsync();

        /// <summary>
        /// Tạo lịch vệ sinh mới
        /// </summary>
        Task<Result<int>> CreateAsync(CleaningSchedule cleaningSchedule);

        /// <summary>
        /// Cập nhật lịch vệ sinh
        /// </summary>
        Task<Result<bool>> UpdateAsync(CleaningSchedule cleaningSchedule);

        /// <summary>
        /// Đánh dấu hoàn thành vệ sinh
        /// </summary>
        Task<Result<bool>> MarkAsCompletedAsync(int cleaningScheduleId, int? cleanedBy = null, string notes = null);

        /// <summary>
        /// Hủy lịch vệ sinh
        /// </summary>
        Task<Result<bool>> CancelAsync(int cleaningScheduleId, string reason = null);

        /// <summary>
        /// Xóa lịch vệ sinh
        /// </summary>
        Task<Result<bool>> DeleteAsync(int cleaningScheduleId);

        /// <summary>
        /// Lấy lịch vệ sinh theo trạm (V0 không có station_id, method này giữ lại để tương thích)
        /// </summary>
        Task<Result<IEnumerable<CleaningSchedule>>> GetByStationIdAsync(int stationId);
    }
}

