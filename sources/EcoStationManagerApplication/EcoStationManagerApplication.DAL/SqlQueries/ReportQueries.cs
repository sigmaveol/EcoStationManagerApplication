using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class ReportQueries
    {
        // Báo cáo doanh thu theo tháng
        public const string MonthlyRevenueReport = @"
            SELECT YEAR(last_updated) as year,
                   MONTH(last_updated) as month,
                   COUNT(*) as order_count,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue,
                   AVG(total_amount - discounted_amount) as avg_order_value
            FROM Orders
            WHERE status = 'COMPLETED'
            AND last_updated BETWEEN @StartDate AND @EndDate
            GROUP BY YEAR(last_updated), MONTH(last_updated)
            ORDER BY year DESC, month DESC";

        // Báo cáo tồn kho
        public const string InventoryReport = @"
            SELECT p.product_id, p.name, p.sku, p.unit, p.price,
                   c.name as category_name,
                   COALESCE(SUM(i.quantity), 0) as total_quantity,
                   COALESCE(SUM(i.quantity * p.price), 0) as total_value,
                   p.min_stock_level,
                   CASE 
                       WHEN COALESCE(SUM(i.quantity), 0) <= p.min_stock_level THEN 'LOW'
                       WHEN COALESCE(SUM(i.quantity), 0) <= p.min_stock_level * 2 THEN 'MEDIUM'
                       ELSE 'HIGH'
                   END as stock_status
            FROM Products p
            LEFT JOIN Categories c ON p.category_id = c.category_id
            LEFT JOIN Inventories i ON p.product_id = i.product_id
            WHERE p.is_active = TRUE
            GROUP BY p.product_id, p.name, p.sku, p.unit, p.price, c.name, p.min_stock_level
            ORDER BY total_value DESC";

        // Báo cáo khách hàng
        public const string CustomerReport = @"
            SELECT c.customer_id, c.name, c.phone, c.email, c.rank, c.total_point,
                   COUNT(o.order_id) as total_orders,
                   COALESCE(SUM(o.total_amount - o.discounted_amount), 0) as total_spent,
                   MAX(o.last_updated) as last_order_date
            FROM Customers c
            LEFT JOIN Orders o ON c.customer_id = o.customer_id AND o.status = 'COMPLETED'
            WHERE c.is_active = TRUE
            GROUP BY c.customer_id, c.name, c.phone, c.email, c.rank, c.total_point
            ORDER BY total_spent DESC";
    }
}
