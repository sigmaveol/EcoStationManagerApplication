using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface ICleaningScheduleRepository : IRepository<CleaningSchedule>
    {
        /// <summary>
        /// Lấy lịch vệ sinh theo loại (tank hoặc package)
        /// </summary>
        Task<IEnumerable<CleaningSchedule>> GetByCleaningTypeAsync(CleaningType cleaningType);

        /// <summary>
        /// Lấy lịch vệ sinh theo trạng thái
        /// </summary>
        Task<IEnumerable<CleaningSchedule>> GetByStatusAsync(CleaningStatus status);

        /// <summary>
        /// Lấy lịch vệ sinh sắp đến hạn
        /// </summary>
        Task<IEnumerable<CleaningSchedule>> GetUpcomingSchedulesAsync(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Lấy lịch vệ sinh quá hạn
        /// </summary>
        Task<IEnumerable<CleaningSchedule>> GetOverdueSchedulesAsync();

        /// <summary>
        /// Lấy lịch vệ sinh theo người thực hiện
        /// </summary>
        Task<IEnumerable<CleaningSchedule>> GetByCleaningByAsync(int userId);

        /// <summary>
        /// Lấy lịch vệ sinh theo ngày và loại vệ sinh (dùng để kiểm tra trùng lịch)
        /// </summary>
        Task<CleaningSchedule> GetByDateAndTypeAsync(DateTime cleaningDate, CleaningType cleaningType);
    }
}

