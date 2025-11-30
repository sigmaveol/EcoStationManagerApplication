using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Tests
{
    class FakeUserService : IUserService
    {
        private readonly Dictionary<string, (User user, string password)> _users = new Dictionary<string, (User, string)>();

        public Task<Result<UserDTO>> AuthenticateAsync(string username, string password)
        {
            if (_users.TryGetValue(username, out var tup) && tup.password == password && tup.user.IsActive == ActiveStatus.ACTIVE)
            {
                var dto = new EcoStationManagerApplication.Models.DTOs.UserDTO
                {
                    UserId = tup.user.UserId,
                    Username = tup.user.Username,
                    Fullname = tup.user.Fullname,
                    Role = tup.user.Role,
                    IsActive = tup.user.IsActive
                };
                return Task.FromResult(Result<UserDTO>.Ok(dto));
            }
            return Task.FromResult(Result<UserDTO>.Fail("Invalid credentials"));
        }

        public Task<Result<int>> CreateUserAsync(string username, string password, UserRole role, string fullname = null)
        {
            if (_users.ContainsKey(username))
                return Task.FromResult(Result<int>.Fail("Exists"));
            var user = new User { UserId = _users.Count + 1, Username = username, Fullname = fullname, Role = role, IsActive = ActiveStatus.ACTIVE };
            _users[username] = (user, password);
            return Task.FromResult(Result<int>.Ok(user.UserId));
        }

        public Task<Result<int>> CreateUserAsync(User user, string password)
        {
            if (_users.ContainsKey(user.Username))
                return Task.FromResult(Result<int>.Fail("Exists"));
            user.UserId = _users.Count + 1; user.IsActive = ActiveStatus.ACTIVE;
            _users[user.Username] = (user, password);
            return Task.FromResult(Result<int>.Ok(user.UserId));
        }

        public Task<Result<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var kv = _users.FirstOrDefault(x => x.Value.user.UserId == userId);
            if (kv.Value.user == null) return Task.FromResult(Result<bool>.Fail("NotFound"));
            if (kv.Value.password != currentPassword) return Task.FromResult(Result<bool>.Fail("WrongPass"));
            _users[kv.Key] = (kv.Value.user, newPassword);
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<bool>> ToggleUserStatusAsync(int userId, bool isActive)
        {
            var kv = _users.FirstOrDefault(x => x.Value.user.UserId == userId);
            if (kv.Value.user == null) return Task.FromResult(Result<bool>.Fail("NotFound"));
            kv.Value.user.IsActive = isActive ? ActiveStatus.ACTIVE : ActiveStatus.INACTIVE;
            _users[kv.Key] = (kv.Value.user, kv.Value.password);
            return Task.FromResult(Result<bool>.Ok(true));
        }

        public Task<Result<User>> GetUserByIdAsync(int userId) => Task.FromResult(Result<User>.Ok(_users.Values.FirstOrDefault(v => v.user.UserId == userId).user));
        public Task<Result<IEnumerable<User>>> GetAllActiveUsersAsync() => Task.FromResult(Result<IEnumerable<User>>.Ok(_users.Values.Select(v => v.user).Where(u => u.IsActive == ActiveStatus.ACTIVE)));
        public Task<Result<IEnumerable<User>>> GetAllAsync() => Task.FromResult(Result<IEnumerable<User>>.Ok(_users.Values.Select(v => v.user)));
        public Task<Result<IEnumerable<User>>> GetUsersByRoleAsync(UserRole role) => Task.FromResult(Result<IEnumerable<User>>.Ok(_users.Values.Select(v => v.user).Where(u => u.Role == role)));
        public Task<Result<bool>> UpdateUserAsync(User user) => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<bool>> ResetPasswordAsync(int userId, string newPassword) => Task.FromResult(Result<bool>.Ok(true));
        public Task<Result<IEnumerable<User>>> GetActiveDriversAsync() => Task.FromResult(Result<IEnumerable<User>>.Ok(Enumerable.Empty<User>()));
        public Task<Result<IEnumerable<User>>> GetActiveStaffAsync() => Task.FromResult(Result<IEnumerable<User>>.Ok(Enumerable.Empty<User>()));
        public Task<Result<IEnumerable<UserRoleStats>>> GetUserStatisticsAsync() => Task.FromResult(Result<IEnumerable<UserRoleStats>>.Ok(Enumerable.Empty<UserRoleStats>()));
    }

    [TestClass]
    public class CoreUserServiceTests
    {
        [TestMethod]
        public async Task Authenticate_Succeeds_With_Correct_Credentials()
        {
            var svc = new FakeUserService();
            var create = await svc.CreateUserAsync("admin", "123", UserRole.ADMIN, "Quản trị");
            Assert.IsTrue(create.Success);

            var auth = await svc.AuthenticateAsync("admin", "123");
            Assert.IsTrue(auth.Success);
            Assert.AreEqual("admin", auth.Data.Username);
        }

        [TestMethod]
        public async Task Authenticate_Fails_With_Wrong_Password()
        {
            var svc = new FakeUserService();
            var create = await svc.CreateUserAsync("staff", "abc", UserRole.STAFF, "Nhân viên");
            Assert.IsTrue(create.Success);
            var auth = await svc.AuthenticateAsync("staff", "xyz");
            Assert.IsFalse(auth.Success);
        }

        [TestMethod]
        public async Task CreateUser_Fails_When_Duplicate()
        {
            var svc = new FakeUserService();
            var r1 = await svc.CreateUserAsync("user", "p", UserRole.STAFF);
            var r2 = await svc.CreateUserAsync("user", "p", UserRole.STAFF);
            Assert.IsTrue(r1.Success);
            Assert.IsFalse(r2.Success);
        }
    }
}
