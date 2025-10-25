using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== ORDER REPOSITORY INTERFACE ====================
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStationAsync(int stationId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Order>> GetOrdersBySourceAsync(string source);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
        Task<int> GetOrderCountByStatusAsync(string status);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    }

    // ==================== ORDER DETAIL REPOSITORY INTERFACE ====================
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId);
        Task<bool> DeleteDetailsByOrderAsync(int orderId);
        Task<decimal> GetOrderTotalAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetBestSellingItemsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsWithProductsAsync(int orderId);
        Task<decimal> GetTotalSalesByVariantAsync(int variantId, DateTime startDate, DateTime endDate);
    }

    // ==================== PAYMENT REPOSITORY INTERFACE ====================
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetPaymentByOrderAsync(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string method);
        Task<Payment> GetPaymentByReferenceAsync(string reference);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
        Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
        Task<int> GetPaymentCountByStatusAsync(string status);
    }

    // ==================== REFUND REPOSITORY INTERFACE ====================
    public interface IRefundRepository : IRepository<Refund>
    {
        Task<IEnumerable<Refund>> GetRefundsByOrderAsync(int orderId);
        Task<IEnumerable<Refund>> GetRefundsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Refund>> GetRefundsByStatusAsync(string status);
        Task<decimal> GetTotalRefundAmountAsync(DateTime startDate, DateTime endDate);
        Task<int> GetRefundCountByStatusAsync(string status);
    }
}