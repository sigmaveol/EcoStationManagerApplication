using EcoStationManagerApplication.Common.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
            : base("UserService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> AuthenticateAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return Result<User>.Fail("Tên đăng nhập và mật khẩu không được để trống");

                // Hash password
                var passwordHash = SecurityHelper.HashPassword(password);

                var user = await _unitOfWork.Users.GetByUsernameAndPasswordAsync(username, passwordHash);
                if (user == null)
                    return Result<User>.Fail("Tên đăng nhập hoặc mật khẩu không đúng");

                if (!user.IsActive)
                    return Result<User>.Fail("Tài khoản đã bị vô hiệu hóa");

                _logger.Info($"User authenticated: {username} (ID: {user.UserId})");
                return Result<User>.Ok(user, "Đăng nhập thành công");
            }
            catch (Exception ex)
            {
                return HandleException<User>(ex, "xác thực người dùng");
            }
        }

        public async Task<Result<User>> GetUserByIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                    return Result<User>.Fail("ID người dùng không hợp lệ");

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return NotFoundError<User>("Người dùng", userId);

                return Result<User>.Ok(user, "Lấy thông tin người dùng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<User>(ex, "lấy thông tin người dùng");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetAllActiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetActiveUsersAsync();
                return Result<IEnumerable<User>>.Ok(users, "Lấy danh sách người dùng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<User>>(ex, "lấy danh sách người dùng");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetUsersByRoleAsync(UserRole role)
        {
            try
            {
                var users = await _unitOfWork.Users.GetByRoleAsync(role);
                return Result<IEnumerable<User>>.Ok(users, $"Lấy danh sách người dùng theo role {role} thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<User>>(ex, "lấy người dùng theo role");
            }
        }

        public async Task<Result<int>> CreateUserAsync(User user, string password)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidateUser(user);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                if (string.IsNullOrWhiteSpace(password))
                    return Result<int>.Fail("Mật khẩu không được để trống");

                if (password.Length < 6)
                    return Result<int>.Fail("Mật khẩu phải có ít nhất 6 ký tự");

                // Kiểm tra username trùng
                var isUsernameExists = await _unitOfWork.Users.IsUsernameExistsAsync(user.Username);
                if (isUsernameExists)
                    return Result<int>.Fail($"Tên đăng nhập '{user.Username}' đã tồn tại");

                // Hash password
                user.PasswordHash = SecurityHelper.HashPassword(password);
                user.IsActive = true;

                // Thêm user
                var userId = await _unitOfWork.Users.AddAsync(user);
                _logger.Info($"Đã tạo user mới: {user.Username} (ID: {userId})");

                return Result<int>.Ok(userId, $"Thêm người dùng '{user.Username}' thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm người dùng");
            }
        }

        public async Task<Result<bool>> UpdateUserAsync(User user)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidateUser(user);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra tồn tại
                var existingUser = await _unitOfWork.Users.GetByIdAsync(user.UserId);
                if (existingUser == null)
                    return NotFoundError<bool>("Người dùng", user.UserId);

                // Kiểm tra username trùng (trừ chính nó)
                var isUsernameExists = await _unitOfWork.Users.IsUsernameExistsAsync(user.Username, user.UserId);
                if (isUsernameExists)
                    return Result<bool>.Fail($"Tên đăng nhập '{user.Username}' đã tồn tại");

                // Cập nhật
                var success = await _unitOfWork.Users.UpdateUserProfileAsync(user);
                if (success)
                {
                    _logger.Info($"Đã cập nhật user: {user.Username} (ID: {user.UserId})");
                    return Result<bool>.Ok(true, $"Cập nhật người dùng '{user.Username}' thành công");
                }

                return Result<bool>.Fail("Cập nhật người dùng thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật người dùng");
            }
        }

        public async Task<Result<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                if (userId <= 0)
                    return Result<bool>.Fail("ID người dùng không hợp lệ");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return Result<bool>.Fail("Mật khẩu mới không được để trống");

                if (newPassword.Length < 6)
                    return Result<bool>.Fail("Mật khẩu mới phải có ít nhất 6 ký tự");

                // Kiểm tra user tồn tại
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return NotFoundError<bool>("Người dùng", userId);

                // Xác thực mật khẩu hiện tại
                var currentPasswordHash = SecurityHelper.HashPassword(currentPassword);
                if (user.PasswordHash != currentPasswordHash)
                    return Result<bool>.Fail("Mật khẩu hiện tại không đúng");

                // Cập nhật mật khẩu mới
                var newPasswordHash = SecurityHelper.HashPassword(newPassword);
                var success = await _unitOfWork.Users.UpdatePasswordAsync(userId, newPasswordHash);
                if (success)
                {
                    _logger.Info($"Đã đổi mật khẩu user: {user.Username} (ID: {userId})");
                    return Result<bool>.Ok(true, "Đổi mật khẩu thành công");
                }

                return Result<bool>.Fail("Đổi mật khẩu thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đổi mật khẩu");
            }
        }

        public async Task<Result<bool>> ToggleUserStatusAsync(int userId, bool isActive)
        {
            try
            {
                if (userId <= 0)
                    return Result<bool>.Fail("ID người dùng không hợp lệ");

                // Kiểm tra tồn tại
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return NotFoundError<bool>("Người dùng", userId);

                // Không cho vô hiệu hóa chính mình
                // Note: Cần implement current user context
                // if (userId == GetCurrentUserId())
                //     return Result<bool>.Fail("Không thể vô hiệu hóa chính mình");

                var success = await _unitOfWork.Users.ToggleUserStatusAsync(userId, isActive);
                var status = isActive ? "kích hoạt" : "vô hiệu hóa";

                if (success)
                {
                    _logger.Info($"Đã {status} user: {user.Username} (ID: {userId})");
                    return Result<bool>.Ok(true, $"Đã {status} người dùng '{user.Username}'");
                }

                return Result<bool>.Fail($"{status} người dùng thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái người dùng");
            }
        }

        public async Task<Result<bool>> ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                if (userId <= 0)
                    return Result<bool>.Fail("ID người dùng không hợp lệ");

                if (string.IsNullOrWhiteSpace(newPassword))
                    return Result<bool>.Fail("Mật khẩu mới không được để trống");

                if (newPassword.Length < 6)
                    return Result<bool>.Fail("Mật khẩu mới phải có ít nhất 6 ký tự");

                // Kiểm tra user tồn tại
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return NotFoundError<bool>("Người dùng", userId);

                // Reset password
                var newPasswordHash = SecurityHelper.HashPassword(newPassword);
                var success = await _unitOfWork.Users.UpdatePasswordAsync(userId, newPasswordHash);
                if (success)
                {
                    _logger.Info($"Đã reset mật khẩu user: {user.Username} (ID: {userId})");
                    return Result<bool>.Ok(true, "Reset mật khẩu thành công");
                }

                return Result<bool>.Fail("Reset mật khẩu thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "reset mật khẩu");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetActiveDriversAsync()
        {
            try
            {
                var drivers = await _unitOfWork.Users.GetActiveDriversAsync();
                return Result<IEnumerable<User>>.Ok(drivers, "Lấy danh sách tài xế thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<User>>(ex, "lấy danh sách tài xế");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetActiveStaffAsync()
        {
            try
            {
                var staff = await _unitOfWork.Users.GetActiveStaffAsync();
                return Result<IEnumerable<User>>.Ok(staff, "Lấy danh sách nhân viên thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<User>>(ex, "lấy danh sách nhân viên");
            }
        }

        public async Task<Result<IEnumerable<UserRoleStats>>> GetUserStatisticsAsync()
        {
            try
            {
                var stats = await _unitOfWork.Users.GetUserRoleStatisticsAsync();
                return Result<IEnumerable<UserRoleStats>>.Ok(stats, "Lấy thống kê người dùng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<UserRoleStats>>(ex, "lấy thống kê người dùng");
            }
        }

        private List<string> ValidateUser(User user)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(user.Username))
                errors.Add("Tên đăng nhập không được để trống");

            if (user.Username?.Length < 3)
                errors.Add("Tên đăng nhập phải có ít nhất 3 ký tự");

            if (string.IsNullOrWhiteSpace(user.Fullname))
                errors.Add("Họ tên không được để trống");

            if (user.Fullname?.Length > 255)
                errors.Add("Họ tên không được vượt quá 255 ký tự");

            return errors;
        }
    }
}
