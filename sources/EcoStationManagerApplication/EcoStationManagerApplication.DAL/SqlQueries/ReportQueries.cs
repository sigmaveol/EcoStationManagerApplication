using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class ReportQueries
    {
        // Báo cáo doanh thu theo ngày
        public const string RevenueByDay = @"
            SELECT DATE(last_updated) as period_date,
                   DATE_FORMAT(last_updated, '%d/%m/%Y') as period,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue,
                   COUNT(*) as order_count
            FROM Orders
            WHERE status = 'COMPLETED'
            AND DATE(last_updated) BETWEEN @FromDate AND @ToDate
            GROUP BY DATE(last_updated)
            ORDER BY period_date ASC";

        // Báo cáo doanh thu theo tuần
        public const string RevenueByWeek = @"
            SELECT YEARWEEK(last_updated) as week_number,
                   CONCAT('Tuần ', YEARWEEK(last_updated) - YEARWEEK(@FromDate) + 1) as period,
                   MIN(DATE(last_updated)) as period_date,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue,
                   COUNT(*) as order_count
            FROM Orders
            WHERE status = 'COMPLETED'
            AND DATE(last_updated) BETWEEN @FromDate AND @ToDate
            GROUP BY YEARWEEK(last_updated)
            ORDER BY week_number ASC";

        // Báo cáo doanh thu theo tháng
        public const string RevenueByMonth = @"
            SELECT YEAR(last_updated) as year,
                   MONTH(last_updated) as month,
                   CONCAT(MONTH(last_updated), '/', YEAR(last_updated)) as period,
                   DATE_FORMAT(LAST_DAY(last_updated), '%Y-%m-%d') as period_date,
                   COALESCE(SUM(total_amount - discounted_amount), 0) as revenue,
                   COUNT(*) as order_count
            FROM Orders
            WHERE status = 'COMPLETED'
            AND DATE(last_updated) BETWEEN @FromDate AND @ToDate
            GROUP BY YEAR(last_updated), MONTH(last_updated)
            ORDER BY year ASC, month ASC";

        // Tổng doanh thu trong khoảng thời gian
        public const string TotalRevenue = @"
            SELECT COALESCE(SUM(total_amount - discounted_amount), 0) as total_revenue,
                   COUNT(*) as total_orders,
                   COALESCE(AVG(total_amount - discounted_amount), 0) as avg_order_value
            FROM Orders
            WHERE status = 'COMPLETED'
            AND DATE(last_updated) BETWEEN @FromDate AND @ToDate";

        // Báo cáo tần suất khách hàng quay trở lại
        public const string CustomerReturnFrequency = @"
            SELECT c.customer_id,
                   c.name as customer_name,
                   c.phone,
                   COUNT(DISTINCT o.order_id) as total_orders,
                   COUNT(DISTINCT DATE(o.last_updated)) as return_count,
                   MAX(o.last_updated) as last_order_date,
                   CASE 
                       WHEN COUNT(DISTINCT DATE(o.last_updated)) > 0 
                       THEN ROUND(COUNT(DISTINCT o.order_id) * 1.0 / COUNT(DISTINCT DATE(o.last_updated)), 2)
                       ELSE 0 
                   END as return_frequency
            FROM Customers c
            INNER JOIN Orders o ON c.customer_id = o.customer_id
            WHERE o.status = 'COMPLETED'
            AND DATE(o.last_updated) BETWEEN @FromDate AND @ToDate
            GROUP BY c.customer_id, c.name, c.phone
            HAVING total_orders > 1
            ORDER BY return_count DESC, total_orders DESC";

        // Báo cáo tỷ lệ thu hồi bao bì
        public const string PackagingRecoveryRate = @"
            SELECT p.packaging_id,
                   p.name as packaging_name,
                   COALESCE(SUM(CASE WHEN pt.type = 'ISSUE' THEN pt.quantity ELSE 0 END), 0) as issued,
                   COALESCE(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END), 0) as returned,
                   CASE 
                       WHEN COALESCE(SUM(CASE WHEN pt.type = 'ISSUE' THEN pt.quantity ELSE 0 END), 0) > 0
                       THEN ROUND(COALESCE(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END), 0) * 100.0 / 
                                  COALESCE(SUM(CASE WHEN pt.type = 'ISSUE' THEN pt.quantity ELSE 0 END), 0), 2)
                       ELSE 0
                   END as recovery_rate
            FROM Packaging p
            LEFT JOIN PackagingTransactions pt ON p.packaging_id = pt.packaging_id
                AND DATE(pt.created_date) BETWEEN @FromDate AND @ToDate
            WHERE p.is_active = TRUE
            GROUP BY p.packaging_id, p.name
            HAVING issued > 0
            ORDER BY recovery_rate DESC";

        // Tổng số bao bì đã phát hành và thu hồi
        public const string TotalPackagingStats = @"
            SELECT COALESCE(SUM(CASE WHEN pt.type = 'ISSUE' THEN pt.quantity ELSE 0 END), 0) as total_issued,
                   COALESCE(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END), 0) as total_returned
            FROM PackagingTransactions pt
            WHERE DATE(pt.created_date) BETWEEN @FromDate AND @ToDate";

        // Báo cáo tác động môi trường
        public const string EnvironmentalImpact = @"
            SELECT DATE(pt.created_date) as period_date,
                   DATE_FORMAT(pt.created_date, '%d/%m/%Y') as period,
                   SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) as refills,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25, 2) as plastic_saved_kg,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25 * 6 / 1000, 2) as co2_saved_kg
            FROM PackagingTransactions pt
            WHERE pt.type = 'RETURN'
            AND DATE(pt.created_date) BETWEEN @FromDate AND @ToDate
            GROUP BY DATE(pt.created_date)
            ORDER BY period_date ASC";

        // Tổng tác động môi trường
        public const string TotalEnvironmentalImpact = @"
            SELECT SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) as total_refills,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25, 2) as plastic_saved_kg,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25 / 1000, 2) as plastic_saved_tons,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25 * 6 / 1000, 2) as co2_saved_kg,
                   ROUND(SUM(CASE WHEN pt.type = 'RETURN' THEN pt.quantity ELSE 0 END) * 0.25 * 6 / 1000000, 2) as co2_saved_tons
            FROM PackagingTransactions pt
            WHERE pt.type = 'RETURN'
            AND DATE(pt.created_date) BETWEEN @FromDate AND @ToDate";

        // Báo cáo tồn kho (giữ nguyên)
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

        // Báo cáo khách hàng (giữ nguyên)
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
