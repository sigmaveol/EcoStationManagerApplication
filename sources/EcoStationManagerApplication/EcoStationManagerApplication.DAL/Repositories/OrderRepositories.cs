using Dapper;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    // ==================== ORDER REPOSITORY IMPLEMENTATION ====================
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository() : base("Orders", "order_id", true) { }
        public OrderRepository(IDbHelper dbHelper) : base(dbHelper, "Orders", "order_id", true) { }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName, u.fullname as CreatedByName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       LEFT JOIN Users u ON o.user_id = u.user_id
                       WHERE o.status = @Status AND o.is_active = 1
                       ORDER BY o.created_date DESC";
            return await _dbHelper.QueryAsync<Order>(sql, new { Status = status });
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       WHERE o.customer_id = @CustomerId AND o.is_active = 1
                       ORDER BY o.created_date DESC";
            return await _dbHelper.QueryAsync<Order>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Order>> GetOrdersByStationAsync(int stationId)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       WHERE o.station_id = @StationId AND o.is_active = 1
                       ORDER BY o.created_date DESC";
            return await _dbHelper.QueryAsync<Order>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       WHERE o.created_date BETWEEN @StartDate AND @EndDate AND o.is_active = 1
                       ORDER BY o.created_date DESC";
            return await _dbHelper.QueryAsync<Order>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<Order>> GetOrdersBySourceAsync(string source)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       WHERE o.source = @Source AND o.is_active = 1
                       ORDER BY o.created_date DESC";
            return await _dbHelper.QueryAsync<Order>(sql, new { Source = source });
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName,
                               od.*, v.name as VariantName, v.sku as VariantSKU,
                               co.name as ComboName, co.code as ComboCode
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       LEFT JOIN OrderDetails od ON o.order_id = od.order_id
                       LEFT JOIN Variants v ON od.variant_id = v.variant_id
                       LEFT JOIN Combos co ON od.combo_id = co.combo_id
                       WHERE o.order_id = @OrderId AND o.is_active = 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Order>(sql, new { OrderId = orderId });
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var sql = "UPDATE Orders SET status = @Status, updated_date = NOW() WHERE order_id = @OrderId";
            var result = await _dbHelper.ExecuteAsync(sql, new { OrderId = orderId, Status = status });
            return result > 0;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(total_amount), 0) 
                       FROM Orders 
                       WHERE status = 'completed' 
                       AND created_date BETWEEN @StartDate AND @EndDate 
                       AND is_active = 1";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetOrderCountByStatusAsync(string status)
        {
            var sql = "SELECT COUNT(*) FROM Orders WHERE status = @Status AND is_active = 1";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { Status = status });
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count)
        {
            var sql = @"SELECT o.*, c.name as CustomerName, s.name as StationName
                       FROM Orders o
                       LEFT JOIN Customers c ON o.customer_id = c.customer_id
                       LEFT JOIN Stations s ON o.station_id = s.station_id
                       WHERE o.is_active = 1
                       ORDER BY o.created_date DESC
                       LIMIT @Count";
            return await _dbHelper.QueryAsync<Order>(sql, new { Count = count });
        }
    }

    // ==================== ORDER DETAIL REPOSITORY IMPLEMENTATION ====================
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository() : base("OrderDetails", "order_detail_id", false) { }
        public OrderDetailRepository(IDbHelper dbHelper) : base(dbHelper, "OrderDetails", "order_detail_id", false) { }

        public async Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId)
        {
            var sql = @"SELECT od.*, v.name as VariantName, v.sku as VariantSKU, v.price as VariantPrice,
                               co.name as ComboName, co.code as ComboCode, co.total_price as ComboPrice
                       FROM OrderDetails od
                       LEFT JOIN Variants v ON od.variant_id = v.variant_id
                       LEFT JOIN Combos co ON od.combo_id = co.combo_id
                       WHERE od.order_id = @OrderId
                       ORDER BY od.order_detail_id";
            return await _dbHelper.QueryAsync<OrderDetail>(sql, new { OrderId = orderId });
        }

        public async Task<bool> DeleteDetailsByOrderAsync(int orderId)
        {
            var sql = "DELETE FROM OrderDetails WHERE order_id = @OrderId";
            var result = await _dbHelper.ExecuteAsync(sql, new { OrderId = orderId });
            return result > 0;
        }

        public async Task<decimal> GetOrderTotalAsync(int orderId)
        {
            var sql = @"SELECT COALESCE(SUM(od.quantity * od.unit_price), 0)
                       FROM OrderDetails od
                       WHERE od.order_id = @OrderId";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<OrderDetail>> GetBestSellingItemsAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT v.variant_id, v.name as VariantName, v.sku as VariantSKU,
                               SUM(od.quantity) as TotalQuantity,
                               SUM(od.quantity * od.unit_price) as TotalRevenue
                       FROM OrderDetails od
                       JOIN Variants v ON od.variant_id = v.variant_id
                       JOIN Orders o ON od.order_id = o.order_id
                       WHERE o.created_date BETWEEN @StartDate AND @EndDate
                       AND o.status = 'completed'
                       GROUP BY v.variant_id, v.name, v.sku
                       ORDER BY TotalQuantity DESC
                       LIMIT 10";
            return await _dbHelper.QueryAsync<OrderDetail>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsWithProductsAsync(int orderId)
        {
            var sql = @"SELECT od.*, 
                               COALESCE(v.name, co.name) as ProductName,
                               COALESCE(v.sku, co.code) as ProductCode,
                               COALESCE(v.unit, 'cái') as ProductUnit
                       FROM OrderDetails od
                       LEFT JOIN Variants v ON od.variant_id = v.variant_id
                       LEFT JOIN Combos co ON od.combo_id = co.combo_id
                       WHERE od.order_id = @OrderId";
            return await _dbHelper.QueryAsync<OrderDetail>(sql, new { OrderId = orderId });
        }

        public async Task<decimal> GetTotalSalesByVariantAsync(int variantId, DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(od.quantity * od.unit_price), 0)
                       FROM OrderDetails od
                       JOIN Orders o ON od.order_id = o.order_id
                       WHERE od.variant_id = @VariantId
                       AND o.created_date BETWEEN @StartDate AND @EndDate
                       AND o.status = 'completed'";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { VariantId = variantId, StartDate = startDate, EndDate = endDate });
        }
    }

    // ==================== PAYMENT REPOSITORY IMPLEMENTATION ====================
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository() : base("Payments", "payment_id", false) { }
        public PaymentRepository(IDbHelper dbHelper) : base(dbHelper, "Payments", "payment_id", false) { }

        public async Task<Payment> GetPaymentByOrderAsync(int orderId)
        {
            var sql = @"SELECT p.*, o.order_id, o.total_amount
                       FROM Payments p
                       JOIN Orders o ON p.order_id = o.order_id
                       WHERE p.order_id = @OrderId
                       ORDER BY p.created_date DESC
                       LIMIT 1";
            return await _dbHelper.QueryFirstOrDefaultAsync<Payment>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            var sql = @"SELECT p.*, o.order_id, c.name as CustomerName
                       FROM Payments p
                       JOIN Orders o ON p.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE p.status = @Status
                       ORDER BY p.created_date DESC";
            return await _dbHelper.QueryAsync<Payment>(sql, new { Status = status });
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT p.*, o.order_id, c.name as CustomerName
                       FROM Payments p
                       JOIN Orders o ON p.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE p.created_date BETWEEN @StartDate AND @EndDate
                       ORDER BY p.created_date DESC";
            return await _dbHelper.QueryAsync<Payment>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string method)
        {
            var sql = @"SELECT p.*, o.order_id, c.name as CustomerName
                       FROM Payments p
                       JOIN Orders o ON p.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE p.method = @Method
                       ORDER BY p.created_date DESC";
            return await _dbHelper.QueryAsync<Payment>(sql, new { Method = method });
        }

        public async Task<Payment> GetPaymentByReferenceAsync(string reference)
        {
            var sql = @"SELECT p.*, o.order_id, c.name as CustomerName
                       FROM Payments p
                       JOIN Orders o ON p.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE p.reference = @Reference";
            return await _dbHelper.QueryFirstOrDefaultAsync<Payment>(sql, new { Reference = reference });
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var sql = "UPDATE Payments SET status = @Status WHERE payment_id = @PaymentId";
            var result = await _dbHelper.ExecuteAsync(sql, new { PaymentId = paymentId, Status = status });
            return result > 0;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(amount), 0)
                       FROM Payments
                       WHERE status = 'completed'
                       AND created_date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetPaymentCountByStatusAsync(string status)
        {
            var sql = "SELECT COUNT(*) FROM Payments WHERE status = @Status";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { Status = status });
        }
    }

    // ==================== REFUND REPOSITORY IMPLEMENTATION ====================
    public class RefundRepository : BaseRepository<Refund>, IRefundRepository
    {
        public RefundRepository() : base("Refunds", "refund_id", false) { }
        public RefundRepository(IDbHelper dbHelper) : base(dbHelper, "Refunds", "refund_id", false) { }

        public async Task<IEnumerable<Refund>> GetRefundsByOrderAsync(int orderId)
        {
            var sql = @"SELECT r.*, p.payment_number, o.order_id
                       FROM Refunds r
                       LEFT JOIN Payments p ON r.payment_id = p.payment_id
                       JOIN Orders o ON r.order_id = o.order_id
                       WHERE r.order_id = @OrderId
                       ORDER BY r.created_date DESC";
            return await _dbHelper.QueryAsync<Refund>(sql, new { OrderId = orderId });
        }

        public async Task<IEnumerable<Refund>> GetRefundsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT r.*, p.payment_number, o.order_id, c.name as CustomerName
                       FROM Refunds r
                       LEFT JOIN Payments p ON r.payment_id = p.payment_id
                       JOIN Orders o ON r.order_id = o.order_id
                       JOIN Customers c ON o.customer_id = c.customer_id
                       WHERE r.created_date BETWEEN @StartDate AND @EndDate
                       ORDER BY r.created_date DESC";
            return await _dbHelper.QueryAsync<Refund>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<IEnumerable<Refund>> GetRefundsByStatusAsync(string status)
        {
            var sql = @"SELECT r.*, p.payment_number, o.order_id
                       FROM Refunds r
                       LEFT JOIN Payments p ON r.payment_id = p.payment_id
                       JOIN Orders o ON r.order_id = o.order_id
                       WHERE r.status = @Status
                       ORDER BY r.created_date DESC";
            return await _dbHelper.QueryAsync<Refund>(sql, new { Status = status });
        }

        public async Task<decimal> GetTotalRefundAmountAsync(DateTime startDate, DateTime endDate)
        {
            var sql = @"SELECT COALESCE(SUM(amount), 0)
                       FROM Refunds
                       WHERE status = 'completed'
                       AND created_date BETWEEN @StartDate AND @EndDate";
            return await _dbHelper.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> GetRefundCountByStatusAsync(string status)
        {
            var sql = "SELECT COUNT(*) FROM Refunds WHERE status = @Status";
            return await _dbHelper.ExecuteScalarAsync<int>(sql, new { Status = status });
        }
    }
}