using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class OrderQueries
    {
        // Lấy đơn hàng theo code

        public const string GetByOrderCode = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.order_code = @OrderCode";

        // Lấy đơn hàng theo trạng thái
        public const string GetByStatus = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.status = @Status
            ORDER BY o.last_updated DESC";

        // Lấy đơn hàng theo khách hàng
        public const string GetByCustomer = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.customer_id = @CustomerId 
            ORDER BY o.last_updated DESC";

        // Lấy đơn hàng theo người tạo
        public const string GetByUser = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.user_id = @UserId 
            ORDER BY o.last_updated DESC";

        // Lấy đơn hàng theo khoảng thời gian
        public const string GetByDateRange = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE DATE(o.last_updated) BETWEEN @StartDate AND @EndDate
            ORDER BY o.last_updated DESC";

        // Lấy đơn hàng hôm nay
        public const string GetTodayOrders = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE DATE(o.last_updated) = CURDATE()
            ORDER BY o.last_updated DESC";

        // Lấy đơn hàng với chi tiết đầy đủ
        public const string GetOrderWithDetails = @"
            SELECT o.*, c.name as customer_name, c.phone as customer_phone, 
                   u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.order_id = @OrderId";

        public const string GetOrderDetails = @"
            SELECT od.*, p.name as product_name, p.sku, p.unit, p.price
            FROM OrderDetails od
            JOIN Products p ON od.product_id = p.product_id
            WHERE od.order_id = @OrderId";

        // Lấy đơn hàng cần xử lý (DRAFT, CONFIRMED, PROCESSING)
        public const string GetPendingOrders = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE o.status IN ('DRAFT', 'CONFIRMED', 'PROCESSING')
            ORDER BY 
                CASE o.status 
                    WHEN 'PROCESSING' THEN 1
                    WHEN 'CONFIRMED' THEN 2  
                    WHEN 'DRAFT' THEN 3
                    ELSE 4
                END,
                o.last_updated ASC";

        // Tìm kiếm đơn hàng
        public const string SearchOrders = @"
            SELECT o.*, c.name as customer_name, u.fullname as user_name 
            FROM Orders o
            LEFT JOIN Customers c ON o.customer_id = c.customer_id
            LEFT JOIN Users u ON o.user_id = u.user_id
            WHERE c.name LIKE @Keyword 
               OR o.order_id LIKE @Keyword
               OR o.address LIKE @Keyword
            ORDER BY o.last_updated DESC";

        // Doanh thu theo tháng
        public const string MonthlyRevenue = @"
            SELECT COALESCE(SUM(total_amount - discounted_amount), 0) 
            FROM Orders 
            WHERE status = 'COMPLETED' 
            AND YEAR(last_updated) = @Year 
            AND MONTH(last_updated) = @Month";

    }
}
