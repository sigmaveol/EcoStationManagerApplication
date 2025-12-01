using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface INotificationService
    {
        Task<Result<List<Notification>>> GetRecentNotificationsAsync(int? userId);
        Task<Result<int>> GetUnreadCountAsync(int? userId);
        Task<Result> MarkAsReadAsync(int notificationId);
        Task<Result> MarkAllAsReadAsync(int? userId);
        Task<Result> GenerateAutoNotificationsAsync();
    }
}