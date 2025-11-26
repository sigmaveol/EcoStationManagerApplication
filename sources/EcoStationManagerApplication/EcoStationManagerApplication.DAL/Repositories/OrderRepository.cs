using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "Orders", "order_id")
        { }

        public async Task<Order> GetByOrderCodeAsync(string orderCode)
        {
            try
            {
                return await _databaseHelper.QueryFirstOrDefaultAsync<Order>
                    (OrderQueries.GetByOrderCode, new { OrderCode = orderCode });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByOrderCodeAsync error - OrderCode: {orderCode} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            try
            {
                // Với TINYINT, cần pass số nguyên thay vì string
                return await _databaseHelper.QueryAsync<Order>
                    (OrderQueries.GetByStatus, new { Status = (int)status });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStatusAsync error - Status: {status} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<Order>
                    (OrderQueries.GetByCustomer, new { CustomerId = customerId });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByCustomerAsync error - CustomerId: {customerId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetByUserAsync(int userId)
        {
            try
            {
                return await _databaseHelper.QueryAsync<Order>
                    (OrderQueries.GetByUser, new { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByUserAsync error - UserId: {userId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<Order>
                    (OrderQueries.GetByDateRange, new
                {
                    StartDate = startDate.Date,
                    EndDate = endDate.Date
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDateRangeAsync error - Start: {startDate}, End: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetTodayOrdersAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<Order>(OrderQueries.GetTodayOrders);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTodayOrdersAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            try
            {
                // Lấy thông tin đơn hàng
                var order = await _databaseHelper.QueryFirstOrDefaultAsync<Order>(OrderQueries.GetOrderWithDetails, new { OrderId = orderId });

                if (order != null)
                {
                    // Lấy chi tiết đơn hàng
                    var orderDetails = await _databaseHelper.QueryAsync<OrderDetail>(OrderQueries.GetOrderDetails, new { OrderId = orderId });

                    // Gán chi tiết vào đơn hàng (cần mapping property trong Order model)
                     order.OrderDetails = orderDetails.ToList();
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOrderWithDetailsAsync error - OrderId: {orderId} - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = "SELECT COALESCE(SUM(total_amount - discounted_amount), 0) FROM Orders WHERE status = @CompletedStatus";
                var parameters = new DynamicParameters();
                parameters.Add("CompletedStatus", (int)OrderStatus.COMPLETED);

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND DATE(last_updated) BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value.Date);
                    parameters.Add("EndDate", endDate.Value.Date);
                }

                return await _databaseHelper.ExecuteScalarAsync<decimal>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalRevenueAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetOrderCountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM Orders WHERE status = @CompletedStatus";
                var parameters = new DynamicParameters();
                parameters.Add("CompletedStatus", (int)OrderStatus.COMPLETED);

                if (startDate.HasValue && endDate.HasValue)
                {
                    sql += " AND DATE(last_updated) BETWEEN @StartDate AND @EndDate";
                    parameters.Add("StartDate", startDate.Value.Date);
                    parameters.Add("EndDate", endDate.Value.Date);
                }

                return await _databaseHelper.ExecuteScalarAsync<int>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOrderCountAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            try
            {
                return await _databaseHelper.ExecuteScalarAsync<decimal>(OrderQueries.MonthlyRevenue, new
                {
                    Year = year,
                    Month = month
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetMonthlyRevenueAsync error - Year: {year}, Month: {month} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> SearchOrdersAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllAsync();

                return await _databaseHelper.QueryAsync<Order>(OrderQueries.SearchOrders, new { Keyword = $"%{keyword}%" });
            }
            catch (Exception ex)
            {
                _logger.Error($"SearchOrdersAsync error - Keyword: {keyword} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var sql = "UPDATE Orders SET status = @NewStatus, last_updated = NOW() WHERE order_id = @OrderId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    OrderId = orderId,
                    NewStatus = (int)newStatus // Với TINYINT, pass số nguyên
                });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateOrderStatusAsync error - OrderId: {orderId}, Status: {newStatus} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int orderId, PaymentStatus newPaymentStatus)
        {
            try
            {
                var sql = "UPDATE Orders SET payment_status = @NewStatus, last_updated = NOW() WHERE order_id = @OrderId";
                var affectedRows = await _databaseHelper.ExecuteAsync(sql, new
                {
                    OrderId = orderId,
                    NewStatus = (int)newPaymentStatus // Với TINYINT, pass số nguyên
                });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdatePaymentStatusAsync error - OrderId: {orderId}, PaymentStatus: {newPaymentStatus} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<Order>(OrderQueries.GetPendingOrders);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPendingOrdersAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetPagedOrdersAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            OrderStatus? status = null,
            PaymentStatus? paymentStatus = null,
            ProductType? productType = null,
            OrderSource? source = null,
            decimal? minTotal = null,
            int? customerId = null,
            int? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            try
            {
                var whereClause = "WHERE 1=1";
                var parameters = new DynamicParameters();

                // Search condition
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause += @" AND (c.name LIKE @Search OR o.order_id LIKE @Search OR o.address LIKE @Search)";
                    parameters.Add("Search", $"%{search}%");
                }

                // Status filter
                if (status.HasValue)
                {
                    whereClause += " AND o.status = @Status";
                    parameters.Add("Status", (int)status.Value); // Với TINYINT, pass số nguyên
                }

                // Payment status filter
                if (paymentStatus.HasValue)
                {
                    whereClause += " AND o.payment_status = @PaymentStatus";
                    parameters.Add("PaymentStatus", (int)paymentStatus.Value); // Với TINYINT, pass số nguyên
                }

                // Source filter
                if (source.HasValue)
                {
                    whereClause += " AND o.source = @Source";
                    parameters.Add("Source", source);
                }

                // Minimum total filter
                if (minTotal.HasValue)
                {
                    whereClause += " AND o.total_amount >= @MinTotal";
                    parameters.Add("MinTotal", minTotal.Value);
                }

                // Customer filter
                if (customerId.HasValue)
                {
                    whereClause += " AND o.customer_id = @CustomerId";
                    parameters.Add("CustomerId", customerId.Value);
                }

                // User filter
                if (userId.HasValue)
                {
                    whereClause += " AND o.user_id = @UserId";
                    parameters.Add("UserId", userId.Value);
                }

                // Date range filter
                if (fromDate.HasValue)
                {
                    whereClause += " AND DATE(o.last_updated) >= @FromDate";
                    parameters.Add("FromDate", fromDate.Value.Date);
                }

                if (toDate.HasValue)
                {
                    whereClause += " AND DATE(o.last_updated) <= @ToDate";
                    parameters.Add("ToDate", toDate.Value.Date);
                }

                // Product type filter (cần join với OrderDetails và Products)
                if (productType.HasValue)
                {
                    whereClause += @" AND EXISTS (
                        SELECT 1 FROM OrderDetails od 
                        JOIN Products p ON od.product_id = p.product_id 
                        WHERE od.order_id = o.order_id AND p.product_type = @ProductType
                    )";
                    parameters.Add("ProductType", (int)productType.Value); // Với TINYINT, pass số nguyên
                }

                // Get total count
                var countSql = $@"
                    SELECT COUNT(DISTINCT o.order_id) 
                    FROM Orders o
                    LEFT JOIN Customers c ON o.customer_id = c.customer_id
                    {whereClause}";

                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(countSql, parameters);

                // Get paged data
                var sql = $@"
                    SELECT o.*, c.name as customer_name, u.fullname as user_name 
                    FROM Orders o
                    LEFT JOIN Customers c ON o.customer_id = c.customer_id
                    LEFT JOIN Users u ON o.user_id = u.user_id
                    {whereClause}
                    ORDER BY o.last_updated DESC
                    LIMIT @PageSize OFFSET @Offset";

                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (pageNumber - 1) * pageSize);

                var orders = await _databaseHelper.QueryAsync<Order>(sql, parameters);
                return (orders, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedOrdersAsync error - Page: {pageNumber}, Size: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}
