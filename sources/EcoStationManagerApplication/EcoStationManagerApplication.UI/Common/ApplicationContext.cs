using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Lưu trữ thông tin context của ứng dụng (user hiện tại, role, etc.)
    /// </summary>
    public static class AppUserContext
    {
        /// <summary>
        /// Role của user hiện tại đang đăng nhập
        /// </summary>
        public static UserRole? CurrentUserRole { get; set; }

        /// <summary>
        /// ID của user hiện tại
        /// </summary>
        public static int? CurrentUserId { get; set; }

        /// <summary>
        /// Username của user hiện tại
        /// </summary>
        public static string CurrentUsername { get; set; }

        /// <summary>
        /// Kiểm tra xem user hiện tại có phải Admin không
        /// </summary>
        public static bool IsAdmin => CurrentUserRole == UserRole.ADMIN;

        /// <summary>
        /// Clear context khi logout
        /// </summary>
        public static void Clear()
        {
            CurrentUserRole = null;
            CurrentUserId = null;
            CurrentUsername = null;
        }
    }
}
