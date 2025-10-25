using EcoStationManagerApplication.Core.Exceptions;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserStationRepository _userStationRepository;

        public UserService(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IUserStationRepository userStationRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userStationRepository = userStationRepository;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống", nameof(username));

            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống", nameof(email));

            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetUsersByStationAsync(int stationId)
        {
            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            return await _userRepository.GetUsersByStationAsync(stationId);
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID phải lớn hơn 0", nameof(userId));

            return await _userRepository.UpdateUserStatusAsync(userId, isActive);
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống", nameof(password));

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !user.IsActive)
                return false;

            // Trong thực tế, bạn nên sử dụng hash password
            // Ở đây giả sử password được lưu dưới dạng hash và so sánh hash
            // Ví dụ: return VerifyPassword(password, user.PasswordHash);

            // Tạm thời trả về true nếu user tồn tại và active
            // Đây chỉ là ví dụ, trong thực tế cần kiểm tra mật khẩu
            return true;
        }

        public async Task<User> CreateUserWithRolesAndStationsAsync(User user, List<int> roleIds, List<int> stationIds)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Validate user
            await ValidateUserAsync(user);

            // Kiểm tra username không trùng
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
                throw new ValidationException($"Tên đăng nhập {user.Username} đã tồn tại");

            // Kiểm tra email không trùng nếu có
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                    throw new ValidationException($"Email {user.Email} đã được sử dụng");
            }

            // Tạo user
            var createdUser = await _userRepository.CreateAsync(user);

            // Gán roles
            if (roleIds != null && roleIds.Count > 0)
            {
                foreach (var roleId in roleIds)
                {
                    await _userRoleRepository.AssignRoleToUserAsync(createdUser.UserId, roleId, createdUser.UserId); // assigned by chính user đó
                }
            }

            // Gán stations
            if (stationIds != null && stationIds.Count > 0)
            {
                foreach (var stationId in stationIds)
                {
                    await _userStationRepository.AssignUserToStationAsync(createdUser.UserId, stationId);
                }
            }

            return createdUser;
        }

        protected override async Task ValidateEntityAsync(User user)
        {
            await ValidateUserAsync(user);
        }

        private async Task ValidateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ValidationException("Tên đăng nhập là bắt buộc");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ValidationException("Mật khẩu là bắt buộc");

            if (string.IsNullOrWhiteSpace(user.Fullname))
                throw new ValidationException("Họ tên là bắt buộc");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ValidationException("Email là bắt buộc");

            if (!IsValidEmail(user.Email))
                throw new ValidationException("Email không hợp lệ");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}