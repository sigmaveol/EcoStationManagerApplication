using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class DashboardQueries
    {
        // Thống kê tổng quan
        public const string GetDashboardStats = @"
            SELECT 
                -- Tổng doanh thu
                (SELECT COALESCE(SUM(total_amount - discounted_amount), 0) 
                 FROM Orders 
                 WHERE status = 5
                 AND DATE(last_updated) = CURDATE()) as today_revenue,
                
                -- Số đơn hàng hôm nay
                (SELECT COUNT(*) 
                 FROM Orders 
                 WHERE DATE(last_updated) = CURDATE()) as today_orders,
                
                -- Số đơn hàng chờ xử lý
                (SELECT COUNT(*) 
                 FROM Orders 
                 WHERE status IN (0, 1, 2)) as pending_orders,
                
                -- Sản phẩm sắp hết hàng
                (SELECT COUNT(DISTINCT p.product_id)
                 FROM Products p
                 LEFT JOIN Inventories i ON p.product_id = i.product_id
                 WHERE p.is_active = TRUE
                 GROUP BY p.product_id
                 HAVING COALESCE(SUM(i.quantity), 0) <= p.min_stock_level) as low_stock_products";

        // Doanh thu 7 ngày gần nhất
        public const string GetWeeklyRevenue = @"
            SELECT DATE(last_updated) as date,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue,
                   COUNT(*) as order_count
            FROM Orders
            WHERE status = 5
            AND last_updated >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)
            GROUP BY DATE(last_updated)
            ORDER BY date ASC";

        // Top sản phẩm bán chạy
        public const string GetTopSellingProducts = @"
            SELECT p.name, p.sku,
                   SUM(od.quantity) as total_sold,
                   SUM(od.quantity * od.unit_price) as revenue
            FROM Products p
            JOIN OrderDetails od ON p.product_id = od.product_id
            JOIN Orders o ON od.order_id = o.order_id
            WHERE o.status = 5
            AND o.last_updated >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
            GROUP BY p.product_id, p.name, p.sku
            ORDER BY total_sold DESC
            LIMIT 10";

        // Thống kê theo nguồn đơn hàng
        public const string GetOrderSourceStats = @"
            SELECT source,
                   COUNT(*) as order_count,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue
            FROM Orders
            WHERE status = 5
            AND last_updated >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
            GROUP BY source
            ORDER BY revenue DESC";
    }
}
