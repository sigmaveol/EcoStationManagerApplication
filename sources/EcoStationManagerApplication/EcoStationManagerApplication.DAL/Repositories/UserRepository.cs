using Dapper;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{

    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int userId);
        Task<bool> ValidateUserAsync(string username, string passwordHash);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UserRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            var sql = "SELECT * FROM Users WHERE userID = @UserId";
            return await _dbHelper.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE username = @Username";
            return await _dbHelper.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users WHERE is_active = true";
            return await _dbHelper.QueryAsync<User>(sql);
        }

        public async Task<int> CreateAsync(User user)
        {
            var sql = @"
                INSERT INTO Users (username, password_hash, email, fullname, is_active, roleID) 
                VALUES (@Username, @PasswordHash, @Email, @Fullname, @IsActive, @RoleID);
                SELECT LAST_INSERT_ID();";

            return await _dbHelper.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var sql = @"
                UPDATE Users SET 
                username = @Username, 
                email = @Email, 
                fullname = @Fullname, 
                is_active = @IsActive, 
                roleID = @RoleID 
                WHERE userID = @UserID";

            var affectedRows = await _dbHelper.ExecuteAsync(sql, user);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var sql = "UPDATE Users SET is_active = false WHERE userID = @UserID";
            var affectedRows = await _dbHelper.ExecuteAsync(sql, new { UserID = userId });
            return affectedRows > 0;
        }

        public async Task<bool> ValidateUserAsync(string username, string passwordHash)
        {
            var sql = "SELECT COUNT(1) FROM Users WHERE username = @Username AND password_hash = @PasswordHash AND is_active = true";
            var count = await _dbHelper.ExecuteScalarAsync<int>(sql, new { Username = username, PasswordHash = passwordHash });
            return count > 0;
        }
    }
}
