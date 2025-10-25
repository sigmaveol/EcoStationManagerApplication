using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByStationAsync(int stationId);
        Task<bool> UpdateUserStatusAsync(int userId, bool isActive);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<bool> UpdateUserPasswordAsync(int userId, string passwordHash);
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> ResetUserPasswordAsync(int userId, string newPassword);
        Task<bool> ValidateUserPermissionsAsync(int userId, string permission); // THIẾU
    }

    public interface IWorkShiftService : IService<WorkShift>
    {
        Task<IEnumerable<WorkShift>> GetShiftsByUserAsync(int userId);
        Task<IEnumerable<WorkShift>> GetShiftsByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<WorkShift>> GetShiftsByStationAsync(int stationId, DateTime date);
        Task<bool> ClockInAsync(int userId, int stationId, DateTime shiftDate, TimeSpan startTime);
        Task<bool> ClockOutAsync(int shiftId, TimeSpan endTime, decimal? kpiScore = null);
        Task<WorkShift> GetCurrentShiftAsync(int userId);
        Task<WorkShift> CreateShiftAsync(WorkShift shift);
        Task<decimal> CalculateUserKPIScoreAsync(int userId, DateTime startDate, DateTime endDate);
        Task<bool> ValidateShiftOverlapAsync(int userId, DateTime shiftDate, TimeSpan startTime, TimeSpan endTime); // THIẾU
    }
}