using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Notifications", "notification_id")
        { }

        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int? userId, int days = 3, int limit = 100)
        {
            var dp = new DynamicParameters();
            dp.Add("UserId", userId);
            dp.Add("Days", days);
            dp.Add("Limit", limit);
            return await _databaseHelper.QueryAsync<Notification>(NotificationQueries.GetRecent, dp);
        }

        public async Task<int> GetUnreadCountAsync(int? userId, int days = 3)
        {
            var dp = new DynamicParameters();
            dp.Add("UserId", userId);
            dp.Add("Days", days);
            return await _databaseHelper.ExecuteScalarAsync<int>(NotificationQueries.GetUnreadCount, dp);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var rows = await _databaseHelper.ExecuteAsync(NotificationQueries.MarkAsRead, new { NotificationId = notificationId });
            return rows > 0;
        }

        public async Task<bool> MarkAllAsReadAsync(int? userId, int days = 3)
        {
            var dp = new DynamicParameters();
            dp.Add("UserId", userId);
            dp.Add("Days", days);
            var rows = await _databaseHelper.ExecuteAsync(NotificationQueries.MarkAllAsRead, dp);
            return rows > 0;
        }

        public async Task<bool> AddIfNotExistsAsync(Notification notification, int dedupHours = 24)
        {
            var dp = new DynamicParameters();
            dp.Add("Type", (int)notification.Type);
            dp.Add("Title", notification.Title);
            dp.Add("Message", notification.Message);
            dp.Add("Hours", dedupHours);
            var exists = await _databaseHelper.ExecuteScalarAsync<int?>(NotificationQueries.ExistsSimilar, dp);
            if (exists.HasValue)
            {
                return false;
            }
            var id = await AddAsync(notification);
            return id > 0;
        }
    }
}