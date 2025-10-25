using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository() : base("Users", "user_id", true) { }

        public UserRepository(IDbHelper dbHelper) : base(dbHelper, "Users", "user_id", true) { }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE username = @Username AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE email = @Email AND is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<IEnumerable<User>> GetUsersByStationAsync(int stationId)
        {
            var sql = @"
                SELECT u.* 
                FROM Users u
                INNER JOIN UserStation us ON u.user_id = us.user_id
                WHERE us.station_id = @StationId AND u.is_active = 1
                ORDER BY u.fullname";
            return await _dbHelper.QueryAsync<User>(sql, new { StationId = stationId });
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var sql = "UPDATE Users SET is_active = @IsActive, updated_date = NOW() WHERE user_id = @UserId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, IsActive = isActive });
            return affectedRows > 0;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string passwordHash)
        {
            var sql = "UPDATE Users SET password_hash = @PasswordHash, updated_date = NOW() WHERE user_id = @UserId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, PasswordHash = passwordHash });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            var sql = "SELECT * FROM Users WHERE is_active = 1 ORDER BY fullname";
            return await _dbHelper.QueryAsync<User>(sql);
        }
    }

    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository() : base("Roles", "role_id", true) { }

        public RoleRepository(IDbHelper dbHelper) : base(dbHelper, "Roles", "role_id", true) { }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            var sql = "SELECT * FROM Roles WHERE name = @Name";
            return await _dbHelper.QueryFirstOrDefaultAsync<Role>(sql, new { Name = name });
        }

        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            var sql = "SELECT * FROM Roles ORDER BY name";
            return await _dbHelper.QueryAsync<Role>(sql);
        }
    }

    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository() : base("UserRoles", "user_id", false) { }

        public UserRoleRepository(IDbHelper dbHelper) : base(dbHelper, "UserRoles", "user_id", false) { }

        public async Task<IEnumerable<UserRole>> GetUserRolesByUserAsync(int userId)
        {
            var sql = @"
                SELECT ur.*, r.name as RoleName 
                FROM UserRoles ur
                LEFT JOIN Roles r ON ur.role_id = r.role_id
                WHERE ur.user_id = @UserId";
            return await _dbHelper.QueryAsync<UserRole>(sql, new { UserId = userId });
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId, int assignedBy)
        {
            var sql = @"
                INSERT INTO UserRoles (user_id, role_id, assigned_by, assigned_date)
                VALUES (@UserId, @RoleId, @AssignedBy, NOW())
                ON DUPLICATE KEY UPDATE assigned_by = @AssignedBy, assigned_date = NOW()";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId, AssignedBy = assignedBy });
            return affectedRows > 0;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var sql = "DELETE FROM UserRoles WHERE user_id = @UserId AND role_id = @RoleId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
            return affectedRows > 0;
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName)
        {
            var sql = @"
                SELECT COUNT(*) 
                FROM UserRoles ur
                LEFT JOIN Roles r ON ur.role_id = r.role_id
                WHERE ur.user_id = @UserId AND r.name = @RoleName";
            var count = await _dbHelper.ExecuteScalarAsync<int>(sql, new { UserId = userId, RoleName = roleName });
            return count > 0;
        }
    }

    public class UserStationRepository : BaseRepository<UserStation>, IUserStationRepository
    {
        public UserStationRepository() : base("UserStation", "user_id", false) { }

        public UserStationRepository(IDbHelper dbHelper) : base(dbHelper, "UserStation", "user_id", false) { }

        public async Task<IEnumerable<UserStation>> GetUserStationsByUserAsync(int userId)
        {
            var sql = @"
                SELECT us.*, s.name as StationName 
                FROM UserStation us
                LEFT JOIN Stations s ON us.station_id = s.station_id
                WHERE us.user_id = @UserId";
            return await _dbHelper.QueryAsync<UserStation>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<UserStation>> GetUserStationsByStationAsync(int stationId)
        {
            var sql = @"
                SELECT us.*, u.fullname as UserName 
                FROM UserStation us
                LEFT JOIN Users u ON us.user_id = u.user_id
                WHERE us.station_id = @StationId AND u.is_active = 1";
            return await _dbHelper.QueryAsync<UserStation>(sql, new { StationId = stationId });
        }

        public async Task<bool> AssignUserToStationAsync(int userId, int stationId)
        {
            var sql = @"
                INSERT INTO UserStation (user_id, station_id)
                VALUES (@UserId, @StationId)
                ON DUPLICATE KEY UPDATE user_id = @UserId, station_id = @StationId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, StationId = stationId });
            return affectedRows > 0;
        }

        public async Task<bool> RemoveUserFromStationAsync(int userId, int stationId)
        {
            var sql = "DELETE FROM UserStation WHERE user_id = @UserId AND station_id = @StationId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, StationId = stationId });
            return affectedRows > 0;
        }

        public async Task<bool> IsUserInStationAsync(int userId, int stationId)
        {
            var sql = "SELECT COUNT(*) FROM UserStation WHERE user_id = @UserId AND station_id = @StationId";
            var count = await _dbHelper.ExecuteScalarAsync<int>(sql, new { UserId = userId, StationId = stationId });
            return count > 0;
        }
    }

    public class WorkShiftRepository : BaseRepository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository() : base("WorkShifts", "shift_id", false) { }

        public WorkShiftRepository(IDbHelper dbHelper) : base(dbHelper, "WorkShifts", "shift_id", false) { }

        public async Task<IEnumerable<WorkShift>> GetShiftsByUserAsync(int userId)
        {
            var sql = @"
                SELECT ws.*, u.fullname as UserName 
                FROM WorkShifts ws
                LEFT JOIN Users u ON ws.user_id = u.user_id
                WHERE ws.user_id = @UserId
                ORDER BY ws.shift_date DESC, ws.start_time DESC";
            return await _dbHelper.QueryAsync<WorkShift>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<WorkShift>> GetShiftsByDateRangeAsync(DateTime start, DateTime end)
        {
            var sql = @"
                SELECT ws.*, u.fullname as UserName 
                FROM WorkShifts ws
                LEFT JOIN Users u ON ws.user_id = u.user_id
                WHERE ws.shift_date BETWEEN @Start AND @End
                ORDER BY ws.shift_date, ws.start_time";
            return await _dbHelper.QueryAsync<WorkShift>(sql, new { Start = start, End = end });
        }

        public async Task<WorkShift> GetCurrentShiftAsync(int userId)
        {
            var sql = @"
                SELECT * FROM WorkShifts 
                WHERE user_id = @UserId 
                AND shift_date = CURDATE() 
                AND end_time IS NULL
                ORDER BY start_time DESC 
                LIMIT 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<WorkShift>(sql, new { UserId = userId });
        }

        public async Task<bool> ClockInAsync(int userId, DateTime shiftDate, TimeSpan startTime)
        {
            var sql = @"
                INSERT INTO WorkShifts (user_id, shift_date, start_time)
                VALUES (@UserId, @ShiftDate, @StartTime)";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserId = userId, ShiftDate = shiftDate, StartTime = startTime });
            return affectedRows > 0;
        }

        public async Task<bool> ClockOutAsync(int shiftId, TimeSpan endTime)
        {
            var sql = "UPDATE WorkShifts SET end_time = @EndTime WHERE shift_id = @ShiftId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { ShiftId = shiftId, EndTime = endTime });
            return affectedRows > 0;
        }

        public async Task<bool> UpdateKpiScoreAsync(int shiftId, decimal kpiScore)
        {
            var sql = "UPDATE WorkShifts SET kpi_score = @KpiScore WHERE shift_id = @ShiftId";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { ShiftId = shiftId, KpiScore = kpiScore });
            return affectedRows > 0;
        }
    }
}