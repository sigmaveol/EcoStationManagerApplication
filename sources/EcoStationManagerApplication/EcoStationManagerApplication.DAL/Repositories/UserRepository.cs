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
            : base(databaseHelper, "Users", "user_id") { }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "SELECT * FROM Users WHERE username = @Username AND is_active = TRUE";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User> GetByUsernameAndPasswordAsync(string username, string passwordHash)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "SELECT * FROM Users WHERE username = @Username AND password_hash = @PasswordHash AND is_active = TRUE";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new 
            {
                Username = username,
                PasswordHash = passwordHash
            });
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "SELECT * FROM Users WHERE role = @Role AND is_active = TRUE ORDER BY fullname";
            return await connection.QueryAsync<User>(sql, new { Role = role });
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "SELECT * FROM Users WHERE is_active = TRUE ORDER BY fullname";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "UPDATE Users SET password_hash = @PasswordHash WHERE user_id = @UserId";
            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                PasswordHash = newPasswordHash
            });
            return affectedRows > 0;
        }

        public async Task<bool> ToggleUserStatusAsync(int userId, bool isActive)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();
            var sql = "UPDATE Users SET is_active = @IsActive WHERE user_id = @UserId";
            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                IsActive = isActive
            });
            return affectedRows > 0;
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize, string search = null, UserRole? role = null)
        {
            using var connection = await _databaseHelper.CreateConnectionAsync();

            var whereClause = "WHERE is_active = TRUE";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(search))
            {
                whereClause += " AND (username LIKE @Search OR fullname LIKE @Search)";
                parameters.Add("Search", $"%{search}%");
            }

            if (role.HasValue)
            {
                whereClause += " AND role = @Role";
                parameters.Add("Role", role.Value);
            }

            var countSql = $"SELECT COUNT(*) FROM Users {whereClause}";
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

            var sql = $@"
                SELECT * FROM Users 
                {whereClause}
                ORDER BY user_id DESC 
                LIMIT @PageSize OFFSET @Offset";

            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (pageNumber - 1) * pageSize);

            var users = await connection.QueryAsync<User>(sql, parameters);
            return (users, totalCount);
        }
    }
}
