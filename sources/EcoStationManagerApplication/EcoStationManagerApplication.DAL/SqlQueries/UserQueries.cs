using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class UserQueries
    {
        // Lấy user theo username
        public const string GetByUsername = @"
            SELECT * FROM Users 
            WHERE username = @Username AND is_active = TRUE";

        // Lấy user theo username và password
        public const string GetByUsernameAndPassword = @"
            SELECT * FROM Users 
            WHERE username = @Username AND password_hash = @PasswordHash AND is_active = TRUE";

        // Lấy user theo role
        // @Role sẽ là số nguyên (TINYINT) từ enum UserRole
        public const string GetByRole = @"
            SELECT * FROM Users 
            WHERE role = @Role AND is_active = TRUE 
            ORDER BY fullname";

        // Lấy user active
        public const string GetActiveUsers = @"
            SELECT * FROM Users 
            WHERE is_active = TRUE 
            ORDER BY fullname";

        // Cập nhật mật khẩu
        public const string UpdatePassword = @"
            UPDATE Users 
            SET password_hash = @PasswordHash 
            WHERE user_id = @UserId";

        // Bật/tắt trạng thái user
        public const string ToggleUserStatus = @"
            UPDATE Users 
            SET is_active = @IsActive 
            WHERE user_id = @UserId";

        // Kiểm tra username tồn tại
        public const string IsUsernameExists = @"
            SELECT 1 FROM Users 
            WHERE username = @Username";

        // Lấy driver active
        // UserRole: ADMIN=0, STAFF=1, MANAGER=2, DRIVER=3
        public const string GetActiveDrivers = @"
            SELECT * FROM Users 
            WHERE role = 3 AND is_active = TRUE 
            ORDER BY fullname";

        // Lấy staff active
        // UserRole: ADMIN=0, STAFF=1, MANAGER=2, DRIVER=3
        public const string GetActiveStaff = @"
            SELECT * FROM Users 
            WHERE role IN (1, 2) AND is_active = TRUE 
            ORDER BY fullname";

        // Cập nhật profile user (không bao gồm password)
        public const string UpdateUserProfile = @"
            UPDATE Users 
            SET fullname = @Fullname, role = @Role, is_active = @IsActive 
            WHERE user_id = @UserId";

        // Đếm user theo role
        public const string GetUserCountByRole = @"
            SELECT COUNT(*) FROM Users 
            WHERE role = @Role AND is_active = TRUE";

        // Tìm kiếm user
        public const string SearchUsers = @"
            SELECT * FROM Users 
            WHERE (username LIKE @Keyword OR fullname LIKE @Keyword) 
            AND is_active = TRUE 
            ORDER BY fullname";

        // User với ca làm hiện tại
        public const string GetUserWithCurrentShift = @"
            SELECT 
                u.user_id, u.username, u.fullname, u.role, u.is_active,
                ws.shift_id as current_shift_id,
                ws.status as shift_status,
                ws.start_time as shift_start_time,
                ws.end_time as shift_end_time,
                ws.kpi_score as current_kpi_score
            FROM Users u
            LEFT JOIN WorkShifts ws ON u.user_id = ws.user_id 
                AND ws.shift_date = CURDATE() 
                AND ws.status IN (1, 0)
            WHERE u.user_id = @UserId";
            // WorkShiftStatus: SCHEDULED=0, IN_PROGRESS=1, COMPLETED=2, CANCELLED=3

        // Phân trang user
        public const string PagedUsersBase = @"
            SELECT * FROM Users 
            WHERE 1=1";

        // Count cho phân trang
        public const string PagedUsersCount = @"
            SELECT COUNT(*) FROM Users 
            WHERE 1=1";

        // Thống kê user theo role
        public const string GetUserRoleStatistics = @"
            SELECT 
                role,
                COUNT(*) as total_users,
                SUM(CASE WHEN is_active = TRUE THEN 1 ELSE 0 END) as active_users,
                SUM(CASE WHEN is_active = FALSE THEN 1 ELSE 0 END) as inactive_users
            FROM Users
            GROUP BY role
            ORDER BY total_users DESC";
    }
}
