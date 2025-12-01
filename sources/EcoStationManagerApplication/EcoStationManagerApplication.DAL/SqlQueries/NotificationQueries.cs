namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class NotificationQueries
    {
        public const string GetRecent = @"
            SELECT * FROM Notifications
            WHERE created_at >= DATE_SUB(NOW(), INTERVAL @Days DAY)
              AND (@UserId IS NULL OR user_id = @UserId OR user_id IS NULL)
            ORDER BY created_at DESC
            LIMIT @Limit";

        public const string GetUnreadCount = @"
            SELECT COUNT(*) FROM Notifications
            WHERE is_read = FALSE
              AND created_at >= DATE_SUB(NOW(), INTERVAL @Days DAY)
              AND (@UserId IS NULL OR user_id = @UserId OR user_id IS NULL)";

        public const string MarkAsRead = @"
            UPDATE Notifications SET is_read = TRUE
            WHERE notification_id = @NotificationId";

        public const string MarkAllAsRead = @"
            UPDATE Notifications SET is_read = TRUE
            WHERE is_read = FALSE
              AND created_at >= DATE_SUB(NOW(), INTERVAL @Days DAY)
              AND (@UserId IS NULL OR user_id = @UserId OR user_id IS NULL)";

        public const string ExistsSimilar = @"
            SELECT 1 FROM Notifications
            WHERE type = @Type AND title = @Title AND message = @Message
              AND created_at >= DATE_SUB(NOW(), INTERVAL @Hours HOUR)
            LIMIT 1";
    }
}