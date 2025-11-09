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
    public interface IUserRepository : IRepository<User>
    {

        /// <summary>
        /// Lấy user theo username
        /// </summary>
        Task<User> GetByUsernameAsync(string username);

        /// <summary>
        /// Lấy user theo username và password (đăng nhập)
        /// </summary>
        Task<User> GetByUsernameAndPasswordAsync(string username, string passwordHash);

        /// <summary>
        /// Lấy danh sách user theo role
        /// </summary>
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);

        /// <summary>
        /// Lấy tất cả user đang active
        /// </summary>
        Task<IEnumerable<User>> GetActiveUsersAsync();

        /// <summary>
        /// Cập nhật mật khẩu user
        /// </summary>
        Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash);

        /// <summary>
        /// Bật/tắt trạng thái active của user
        /// </summary>
        Task<bool> ToggleUserStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Kiểm tra username đã tồn tại chưa
        /// </summary>
        Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null);

        /// <summary>
        /// Lấy danh sách driver (tài xế) đang active
        /// </summary>
        Task<IEnumerable<User>> GetActiveDriversAsync();

        /// <summary>
        /// Lấy danh sách staff (nhân viên) đang active
        /// </summary>
        Task<IEnumerable<User>> GetActiveStaffAsync();

        /// <summary>
        /// Cập nhật thông tin user (không bao gồm password)
        /// </summary>
        Task<bool> UpdateUserProfileAsync(User user);

        /// <summary>
        /// Lấy số lượng user theo role
        /// </summary>
        Task<int> GetUserCountByRoleAsync(UserRole role);

        /// <summary>
        /// Tìm kiếm user theo tên hoặc username
        /// </summary>
        Task<IEnumerable<User>> SearchUsersAsync(string keyword);

        /// <summary>
        /// Lấy user với thông tin ca làm việc hiện tại
        /// </summary>
        Task<UserWithShift> GetUserWithCurrentShiftAsync(int userId);

        /// <summary>
        /// Phân trang user
        /// </summary>
        Task<(IEnumerable<User> Users, int TotalCount)> GetPagedUsersAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            UserRole? role = null,
            bool? isActive = null
        );

        /// <summary>
        /// Lấy thống kê user theo role
        /// </summary>
        Task<IEnumerable<UserRoleStats>> GetUserRoleStatisticsAsync();

    }
}
