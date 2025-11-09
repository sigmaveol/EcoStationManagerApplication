using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class ProductQueries
    {
        // Sản phẩm sắp hết hàng
        public const string GetLowStockProducts = @"
            SELECT p.*, COALESCE(SUM(i.quantity), 0) as current_stock
            FROM Products p
            LEFT JOIN Inventories i ON p.product_id = i.product_id
            WHERE p.is_active = TRUE
            GROUP BY p.product_id
            HAVING current_stock <= p.min_stock_level
            ORDER BY p.name";
    }
}
