using Dapper;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Users", "user_id")
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<User>(
                    UserQueries.GetByUsername,
                    new { Username = username }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByUsernameAsync error - Username: {username} - {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string passwordHash)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwordHash))
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<User>(
                    UserQueries.GetByUsernameAndPassword,
                    new { Username = username, PasswordHash = passwordHash }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByUsernameAndPasswordAsync error - Username: {username} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            try
            {
                return await _databaseHelper.QueryAsync<User>(
                    UserQueries.GetByRole,
                    new { Role = role.ToString() }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByRoleAsync error - Role: {role} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<User>(UserQueries.GetActiveUsers);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveUsersAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            try
            {
                if (userId <= 0 || string.IsNullOrWhiteSpace(newPasswordHash))
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    UserQueries.UpdatePassword,
                    new { UserId = userId, PasswordHash = newPasswordHash }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật mật khẩu user - UserId: {userId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdatePasswordAsync error - UserId: {userId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ToggleUserStatusAsync(int userId, bool isActive)
        {
            try
            {
                if (userId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    UserQueries.ToggleUserStatus,
                    new { UserId = userId, IsActive = isActive }
                );

                if (affectedRows > 0)
                {
                    var status = isActive ? "kích hoạt" : "vô hiệu hóa";
                    _logger.Info($"Đã {status} user - UserId: {userId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ToggleUserStatusAsync error - UserId: {userId}, IsActive: {isActive} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    return false;

                var sql = UserQueries.IsUsernameExists;
                var parameters = new DynamicParameters();
                parameters.Add("Username", username);

                if (excludeUserId.HasValue)
                {
                    sql += " AND user_id != @ExcludeUserId";
                    parameters.Add("ExcludeUserId", excludeUserId.Value);
                }

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(sql, parameters);
                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsUsernameExistsAsync error - Username: {username}, ExcludeId: {excludeUserId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetActiveDriversAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<User>(UserQueries.GetActiveDrivers);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveDriversAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetActiveStaffAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<User>(UserQueries.GetActiveStaff);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetActiveStaffAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            try
            {
                if (user == null || user.UserId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    UserQueries.UpdateUserProfile,
                    new
                    {
                        user.UserId,
                        user.Fullname,
                        Role = user.Role.ToString()
                    }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật profile user - UserId: {user.UserId}, Name: {user.Fullname}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateUserProfileAsync error - UserId: {user?.UserId} - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetUserCountByRoleAsync(UserRole role)
        {
            try
            {
                return await _databaseHelper.ExecuteScalarAsync<int>(
                    UserQueries.GetUserCountByRole,
                    new { Role = role.ToString() }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetUserCountByRoleAsync error - Role: {role} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetActiveUsersAsync();

                return await _databaseHelper.QueryAsync<User>(
                    UserQueries.SearchUsers,
                    new { Keyword = $"%{keyword}%" }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchUsersAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<UserWithShift> GetUserWithCurrentShiftAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;

                return await _databaseHelper.QueryFirstOrDefaultAsync<UserWithShift>(
                    UserQueries.GetUserWithCurrentShift,
                    new { UserId = userId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetUserWithCurrentShiftAsync error - UserId: {userId} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPagedUsersAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            UserRole? role = null,
            bool? isActive = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += " AND (username LIKE @Search OR fullname LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // Role filter
                if (role.HasValue)
                {
                    whereClause += " AND role = @Role";
                    parameters.Add("Role", role.Value.ToString());
                }

                // Active status filter
                if (isActive.HasValue)
                {
                    whereClause += " AND is_active = @IsActive";
                    parameters.Add("IsActive", isActive.Value);
                }

                // Get total count
                var countSql = UserQueries.PagedUsersCount + " " + whereClause;
                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = UserQueries.PagedUsersBase + " " + whereClause +
                         " ORDER BY user_id DESC LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var users = await _databaseHelper.QueryAsync<User>(sql, parameters);
                return (users, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedUsersAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UserRoleStats>> GetUserRoleStatisticsAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<UserRoleStats>(UserQueries.GetUserRoleStatistics);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetUserRoleStatisticsAsync error - {ex.Message}");
                throw;
            }
        }
    }
}
