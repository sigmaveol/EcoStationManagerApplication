using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces
{

    /// <summary>
    /// Order repository interface extending the generic IRepository.
    /// Provides extra methods for order management and reporting.
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Get an order by its unique order number.
        /// </summary>
        Task<Order> GetByOrderNumberAsync(string orderNumber);

        /// <summary>
        /// Get all orders placed by a specific customer.
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);

        /// <summary>
        /// Get all orders with a specific status.
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);

        /// <summary>
        /// Get all orders within a given date range.
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Update the status of an order.
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);

        /// <summary>
        /// Calculate total revenue within an optional date range.
        /// </summary>
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Count total orders by a specific status.
        /// </summary>
        Task<int> GetOrderCountByStatusAsync(string status);

    }
}
