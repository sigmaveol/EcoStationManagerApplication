namespace EcoStationManagerApplication.Common.Helpers
{
    /// <summary>
    /// Helper để lưu trữ và lấy thông tin user hiện tại (dùng trong Core layer)
    /// UI layer sẽ set giá trị này khi đăng nhập
    /// </summary>
    public static class UserContextHelper
    {
        /// <summary>
        /// ID của user hiện tại đang đăng nhập
        /// </summary>
        public static int? CurrentUserId { get; set; }

        /// <summary>
        /// Lấy CurrentUserId, trả về null nếu chưa đăng nhập
        /// </summary>
        public static int? GetCurrentUserId()
        {
            return CurrentUserId;
        }

        /// <summary>
        /// Clear context khi logout
        /// </summary>
        public static void Clear()
        {
            CurrentUserId = null;
        }
    }
}

