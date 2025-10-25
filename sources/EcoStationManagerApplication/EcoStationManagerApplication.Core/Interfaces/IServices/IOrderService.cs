using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== ORDER SERVICE INTERFACE ====================
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStationAsync(int stationId);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<Order> CreateOrderWithDetailsAsync(Order order, List<OrderDetail> details);
        Task<bool> CancelOrderAsync(int orderId, string reason);
        Task<bool> ValidateOrderAsync(Order order);
        Task<Dictionary<string, object>> GetOrderStatisticsAsync(DateTime startDate, DateTime endDate);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }

    // ==================== ORDER DETAIL SERVICE INTERFACE ====================
    public interface IOrderDetailService : IService<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId);
        Task<bool> DeleteDetailsByOrderAsync(int orderId);
        Task<decimal> GetOrderTotalAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetBestSellingItemsAsync(DateTime startDate, DateTime endDate);
        Task<bool> AddDetailsToOrderAsync(int orderId, List<OrderDetail> details);
        Task<bool> UpdateOrderDetailQuantityAsync(int orderDetailId, decimal quantity);
        Task<Dictionary<string, object>> GetSalesAnalysisAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== PAYMENT SERVICE INTERFACE ====================
    public interface IPaymentService : IService<Payment>
    {
        Task<Payment> GetPaymentByOrderAsync(int orderId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
        Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string method);
        Task<bool> ProcessPaymentAsync(Payment payment);
        Task<bool> RefundPaymentAsync(int paymentId, decimal amount, string reason);
        Task<Dictionary<string, object>> GetPaymentStatisticsAsync(DateTime startDate, DateTime endDate);
    }

    // ==================== REFUND SERVICE INTERFACE ====================
    public interface IRefundService : IService<Refund>
    {
        Task<IEnumerable<Refund>> GetRefundsByOrderAsync(int orderId);
        Task<IEnumerable<Refund>> GetRefundsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRefundAmountAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Refund>> GetRefundsByStatusAsync(string status);
        Task<bool> ProcessRefundAsync(Refund refund);
        Task<Dictionary<string, object>> GetRefundStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}