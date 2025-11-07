using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetByCodeAsync(string orderCode);
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetByUserAsync(int userId);
        Task<IEnumerable<Order>> GetByStatusAsync(string status);
        Task<IEnumerable<Order>> GetUnpaidOrdersAsync();
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int limit);
        Task<bool> UpdateStatusAsync(int orderId, string newStatus);
        Task<bool> UpdatePaymentStatusAsync(int orderId, string paymentStatus);
        Task<(IEnumerable<Order> Orders, int TotalCount)> GetPagedOrdersAsync(
            int pageNumber,
            int pageSize,
            string searchKeyword = null,
            string status = null,
            string paymentStatus = null,
            int? customerId = null
        );
    }
}
