using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Xác thực người dùng (đăng nhập)
        /// </summary>
        Task<Result<UserDTO>> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        Task<Result<User>> GetUserByIdAsync(int userId);

        /// <summary>
        /// Lấy tất cả người dùng đang active
        /// </summary>
        Task<Result<IEnumerable<User>>> GetAllActiveUsersAsync();
        Task<Result<IEnumerable<User>>> GetAllAsync();

        /// <summary>
        /// Lấy người dùng theo role
        /// </summary>
        Task<Result<IEnumerable<User>>> GetUsersByRoleAsync(UserRole role);

        /// <summary>
        /// Tạo người dùng mới với username, password và role
        /// </summary>
        Task<Result<int>> CreateUserAsync(string username, string password, UserRole role, string fullname = null);

        /// <summary>
        /// Tạo người dùng mới
        /// </summary>
        Task<Result<int>> CreateUserAsync(User user, string password);

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        Task<Result<bool>> UpdateUserAsync(User user);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        Task<Result<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        /// <summary>
        /// Bật/tắt trạng thái người dùng
        /// </summary>
        Task<Result<bool>> ToggleUserStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Reset mật khẩu (quên mật khẩu)
        /// </summary>
        Task<Result<bool>> ResetPasswordAsync(int userId, string newPassword);

        /// <summary>
        /// Lấy danh sách tài xế đang active
        /// </summary>
        Task<Result<IEnumerable<User>>> GetActiveDriversAsync();

        /// <summary>
        /// Lấy danh sách nhân viên đang active
        /// </summary>
        Task<Result<IEnumerable<User>>> GetActiveStaffAsync();

        /// <summary>
        /// Lấy thống kê người dùng theo role
        /// </summary>
        Task<Result<IEnumerable<UserRoleStats>>> GetUserStatisticsAsync();
    }
}
