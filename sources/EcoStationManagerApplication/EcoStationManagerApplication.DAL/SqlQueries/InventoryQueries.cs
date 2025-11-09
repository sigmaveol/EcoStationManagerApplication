using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class InventoryQueries
    {
        // Sản phẩm sắp hết hàng với thông tin chi tiết
        public const string GetLowStockItems = @"
            SELECT i.*, p.name as product_name, p.min_stock_level
            FROM Inventories i
            JOIN Products p ON i.product_id = p.product_id
            WHERE i.quantity <= p.min_stock_level
            AND p.is_active = TRUE
            ORDER BY p.name";

        // Sản phẩm sắp hết hạn
        public const string GetExpiringItems = @"
            SELECT i.*, p.name as product_name
            FROM Inventories i
            JOIN Products p ON i.product_id = p.product_id
            WHERE i.expiry_date IS NOT NULL 
            AND i.expiry_date BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @DaysThreshold DAY)
            AND p.is_active = TRUE
            ORDER BY i.expiry_date ASC";

        // Thống kê tồn kho theo danh mục
        public const string GetInventoryByCategory = @"
            SELECT c.category_id, c.name as category_name,
                   COUNT(DISTINCT p.product_id) as product_count,
                   COALESCE(SUM(i.quantity), 0) as total_quantity,
                   COALESCE(SUM(i.quantity * p.price), 0) as total_value
            FROM Categories c
            LEFT JOIN Products p ON c.category_id = p.category_id AND p.is_active = TRUE
            LEFT JOIN Inventories i ON p.product_id = i.product_id
            WHERE c.is_active = TRUE
            GROUP BY c.category_id, c.name
            ORDER BY total_value DESC";
    }
}
