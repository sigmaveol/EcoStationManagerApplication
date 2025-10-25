using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByStationAsync(int stationId);
        Task<bool> UpdateUserStatusAsync(int userId, bool isActive);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> UpdateUserPasswordAsync(int userId, string passwordHash);
        // THIẾU: GetUserWithRolesAsync
        Task<User> GetUserWithRolesAsync(int userId); // ĐÃ THÊM
    }

    public interface IWorkShiftRepository : IRepository<WorkShift>
    {
        Task<IEnumerable<WorkShift>> GetShiftsByUserAsync(int userId);
        Task<IEnumerable<WorkShift>> GetShiftsByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<WorkShift>> GetShiftsByStationAsync(int stationId, DateTime date);
        Task<bool> ClockInAsync(int userId, int stationId, DateTime shiftDate, TimeSpan startTime);
        Task<bool> ClockOutAsync(int shiftId, TimeSpan endTime, decimal? kpiScore = null);
        Task<WorkShift> GetCurrentShiftAsync(int userId);
        // THIẾU: GetShiftStatisticsAsync
        Task<Dictionary<string, decimal>> GetShiftStatisticsAsync(int userId, DateTime startDate, DateTime endDate); // ĐÃ THÊM
    }
}