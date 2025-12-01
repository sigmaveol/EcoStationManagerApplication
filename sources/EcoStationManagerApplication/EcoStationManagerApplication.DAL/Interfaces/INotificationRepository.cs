using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int? userId, int days = 3, int limit = 100);
        Task<int> GetUnreadCountAsync(int? userId, int days = 3);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int? userId, int days = 3);
        Task<bool> AddIfNotExistsAsync(Notification notification, int dedupHours = 24);
    }
}