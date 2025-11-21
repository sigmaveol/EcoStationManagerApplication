using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class CleaningScheduleRepository : BaseRepository<CleaningSchedule>, ICleaningScheduleRepository
    {
        public CleaningScheduleRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "CleaningSchedules", "cs_id")
        {
        }

        public async Task<IEnumerable<CleaningSchedule>> GetByCleaningTypeAsync(CleaningType cleaningType)
        {
            // Với TINYINT, cần pass số nguyên thay vì string
            return await _databaseHelper.QueryAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetByCleaningType,
                new { CleaningType = (int)cleaningType }
            );
        }

        public async Task<IEnumerable<CleaningSchedule>> GetByStatusAsync(CleaningStatus status)
        {
            // Với TINYINT, cần pass số nguyên thay vì string
            return await _databaseHelper.QueryAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetByStatus,
                new { Status = (int)status }
            );
        }

        public async Task<IEnumerable<CleaningSchedule>> GetUpcomingSchedulesAsync(DateTime fromDate, DateTime toDate)
        {
            return await _databaseHelper.QueryAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetUpcomingInRange,
                new { FromDate = fromDate, ToDate = toDate }
            );
        }

        public async Task<IEnumerable<CleaningSchedule>> GetOverdueSchedulesAsync()
        {
            return await _databaseHelper.QueryAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetOverdue,
                new { }
            );
        }

        public async Task<IEnumerable<CleaningSchedule>> GetByCleaningByAsync(int userId)
        {
            return await _databaseHelper.QueryAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetByCleaningBy,
                new { CleaningBy = userId }
            );
        }

        public async Task<CleaningSchedule> GetByDateAndTypeAsync(DateTime cleaningDate, CleaningType cleaningType)
        {
            // Với TINYINT, cần pass số nguyên thay vì string
            return await _databaseHelper.QueryFirstOrDefaultAsync<CleaningSchedule>(
                CleaningScheduleQueries.GetByDateAndType,
                new
                {
                    CleaningDate = cleaningDate,
                    CleaningType = (int)cleaningType
                }
            );
        }
    }
}


