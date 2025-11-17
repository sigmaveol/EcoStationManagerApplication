using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class WorkShiftRepository : BaseRepository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "WorkShifts", "shift_id")
        {
        }

        public async Task<WorkShift> GetByUserIdAndDateAsync(int userId, DateTime shiftDate)
        {
            try
            {
                if (userId <= 0)
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<WorkShift>(
                    WorkShiftQueries.GetByUserIdAndDate,
                    new { UserId = userId, ShiftDate = shiftDate.Date }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByUserIdAndDateAsync error - UserId: {userId}, Date: {shiftDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<WorkShift> GetCurrentShiftByUserIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<WorkShift>(
                    WorkShiftQueries.GetCurrentShiftByUserId,
                    new { UserId = userId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetCurrentShiftByUserIdAsync error - UserId: {userId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<WorkShift>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (userId <= 0)
                    return new List<WorkShift>();

                return await _databaseHelper.QueryAsync<WorkShift>(
                    WorkShiftQueries.GetByUserIdAndDateRange,
                    new 
                    { 
                        UserId = userId, 
                        StartDate = startDate.Date, 
                        EndDate = endDate.Date 
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByUserIdAndDateRangeAsync error - UserId: {userId}, Start: {startDate}, End: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ExistsByUserIdAndDateAsync(int userId, DateTime shiftDate)
        {
            try
            {
                if (userId <= 0)
                    return false;

                var count = await _databaseHelper.ExecuteScalarAsync<int>(
                    WorkShiftQueries.ExistsByUserIdAndDate,
                    new { UserId = userId, ShiftDate = shiftDate.Date }
                );

                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ExistsByUserIdAndDateAsync error - UserId: {userId}, Date: {shiftDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateStartTimeAsync(int shiftId, TimeSpan startTime)
        {
            try
            {
                if (shiftId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    WorkShiftQueries.UpdateStartTime,
                    new { ShiftId = shiftId, StartTime = startTime }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật start_time cho shift - ShiftId: {shiftId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateStartTimeAsync error - ShiftId: {shiftId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateEndTimeAsync(int shiftId, TimeSpan endTime)
        {
            try
            {
                if (shiftId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    WorkShiftQueries.UpdateEndTime,
                    new { ShiftId = shiftId, EndTime = endTime }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật end_time cho shift - ShiftId: {shiftId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateEndTimeAsync error - ShiftId: {shiftId} - {ex.Message}");
                throw;
            }
        }
    }
}

