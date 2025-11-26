using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
            : base("NotificationService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Notification>>> GetRecentNotificationsAsync(int? userId)
        {
            try
            {
                var items = await _unitOfWork.Notifications.GetRecentNotificationsAsync(userId);
                return Result<List<Notification>>.Ok(items.ToList());
            }
            catch (System.Exception ex)
            {
                return HandleException<List<Notification>>(ex, "lấy thông báo");
            }
        }

        public async Task<Result<int>> GetUnreadCountAsync(int? userId)
        {
            try
            {
                var count = await _unitOfWork.Notifications.GetUnreadCountAsync(userId);
                return Result<int>.Ok(count);
            }
            catch (System.Exception ex)
            {
                return HandleException<int>(ex, "đếm thông báo chưa đọc");
            }
        }

        public async Task<Result> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var ok = await _unitOfWork.Notifications.MarkAsReadAsync(notificationId);
                if (!ok) return BusinessError("Không thể cập nhật trạng thái thông báo");
                return Result.Ok();
            }
            catch (System.Exception ex)
            {
                return HandleException(ex, "cập nhật trạng thái thông báo");
            }
        }

        public async Task<Result> MarkAllAsReadAsync(int? userId)
        {
            try
            {
                var ok = await _unitOfWork.Notifications.MarkAllAsReadAsync(userId);
                if (!ok) return BusinessError("Không thể cập nhật trạng thái thông báo");
                return Result.Ok();
            }
            catch (System.Exception ex)
            {
                return HandleException(ex, "cập nhật trạng thái thông báo");
            }
        }

        public async Task<Result> GenerateAutoNotificationsAsync()
        {
            try
            {
                var lowStocks = await _unitOfWork.Inventories.GetLowStockItemsAsync();
                if (lowStocks != null)
                {
                    foreach (var inv in lowStocks)
                    {
                        var min = inv.MinStockLevel > 0 ? inv.MinStockLevel : (inv.Product?.MinStockLevel ?? 0m);
                        if (min <= 0m) continue;
                        var pct = (inv.Quantity / min) * 100m;
                        if (pct <= 50m)
                        {
                            var n = new Notification
                            {
                                Title = "Cảnh báo tồn kho",
                                Message = $"{(string.IsNullOrWhiteSpace(inv.ProductName) ? inv.Product?.Name : inv.ProductName)} còn {inv.Quantity:N2}, tối thiểu {min:N2}",
                                Type = Models.Enums.NotificationType.LOWSTOCK,
                                IsRead = false,
                                CreatedAt = System.DateTime.Now
                            };
                            await _unitOfWork.Notifications.AddIfNotExistsAsync(n);
                        }
                        else if (pct <= 80m)
                        {
                            var n = new Notification
                            {
                                Title = "Cảnh báo tồn kho",
                                Message = $"{(string.IsNullOrWhiteSpace(inv.ProductName) ? inv.Product?.Name : inv.ProductName)} còn {inv.Quantity:N2}, tối thiểu {min:N2}",
                                Type = Models.Enums.NotificationType.LOWSTOCK,
                                IsRead = false,
                                CreatedAt = System.DateTime.Now
                            };
                            await _unitOfWork.Notifications.AddIfNotExistsAsync(n);
                        }
                    }
                }

                var expiring = await _unitOfWork.Inventories.GetExpiringItemsAsync(15);
                if (expiring != null)
                {
                    foreach (var inv in expiring)
                    {
                        if (inv.ExpiryDate.HasValue)
                        {
                            var n = new Notification
                            {
                                Title = "Sắp hết hạn",
                                Message = $"{(string.IsNullOrWhiteSpace(inv.ProductName) ? inv.Product?.Name : inv.ProductName)} · {inv.ExpiryDate.Value:dd/MM}",
                                Type = Models.Enums.NotificationType.SYSTEM,
                                IsRead = false,
                                CreatedAt = System.DateTime.Now
                            };
                            await _unitOfWork.Notifications.AddIfNotExistsAsync(n);
                        }
                    }
                }

                var overdue = await _unitOfWork.CleaningSchedules.GetOverdueSchedulesAsync();
                if (overdue != null)
                {
                    foreach (var cs in overdue)
                    {
                        var typeLabel = cs.CleaningType == Models.Enums.CleaningType.TANK ? "Bồn" : "Bao bì";
                        var when = cs.CleaningDate.ToString("dd/MM HH:mm");
                        var n = new Notification
                        {
                            Title = "Vệ sinh quá hạn",
                            Message = $"{typeLabel} · {when}",
                            Type = Models.Enums.NotificationType.SYSTEM,
                            IsRead = false,
                            CreatedAt = System.DateTime.Now
                        };
                        await _unitOfWork.Notifications.AddIfNotExistsAsync(n);
                    }
                }

                return Result.Ok();
            }
            catch (System.Exception ex)
            {
                return HandleException(ex, "tạo thông báo tự động");
            }
        }
    }
}