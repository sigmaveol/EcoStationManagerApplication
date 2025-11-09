using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class OrderDetailQueries
    {
        // Lấy chi tiết đơn hàng theo orderId
        public const string GetByOrder = @"
            SELECT * FROM OrderDetails 
            WHERE order_id = @OrderId 
            ORDER BY order_detail_id";

        // Lấy chi tiết đơn hàng với thông tin sản phẩm
        public const string GetOrderDetailsWithProducts = @"
            SELECT od.*, p.name as product_name, p.sku, p.unit, p.price as product_price
            FROM OrderDetails od
            JOIN Products p ON od.product_id = p.product_id
            WHERE od.order_id = @OrderId 
            ORDER BY od.order_detail_id";

        // Xóa tất cả chi tiết theo orderId
        public const string DeleteByOrder = @"
            DELETE FROM OrderDetails 
            WHERE order_id = @OrderId";

        // Cập nhật số lượng
        public const string UpdateQuantity = @"
            UPDATE OrderDetails 
            SET quantity = @Quantity 
            WHERE order_detail_id = @OrderDetailId";

        // Cập nhật đơn giá
        public const string UpdateUnitPrice = @"
            UPDATE OrderDetails 
            SET unit_price = @UnitPrice 
            WHERE order_detail_id = @OrderDetailId";

        // Tổng số lượng sản phẩm đã bán
        public const string GetTotalSoldQuantity = @"
            SELECT COALESCE(SUM(od.quantity), 0)
            FROM OrderDetails od
            JOIN Orders o ON od.order_id = o.order_id
            WHERE od.product_id = @ProductId
            AND o.status = 'COMPLETED'";

        // Top sản phẩm bán chạy
        public const string GetTopSellingProducts = @"
            SELECT p.product_id, p.name as product_name, p.sku,
                   SUM(od.quantity) as total_quantity,
                   SUM(od.quantity * od.unit_price) as total_revenue
            FROM Products p
            JOIN OrderDetails od ON p.product_id = od.product_id
            JOIN Orders o ON od.order_id = o.order_id
            WHERE o.status = 'COMPLETED'";

        // Doanh thu theo sản phẩm
        public const string GetProductRevenue = @"
            SELECT p.product_id, p.name as product_name,
                   SUM(od.quantity) as total_quantity,
                   SUM(od.quantity * od.unit_price) as total_revenue,
                   AVG(od.unit_price) as average_price
            FROM Products p
            JOIN OrderDetails od ON p.product_id = od.product_id
            JOIN Orders o ON od.order_id = o.order_id
            WHERE o.status = 'COMPLETED'";

        // Kiểm tra sản phẩm có trong đơn hàng nào không
        public const string IsProductInAnyOrder = @"
            SELECT 1 FROM OrderDetails od
            JOIN Orders o ON od.order_id = o.order_id
            WHERE od.product_id = @ProductId
            AND o.status != 'CANCELLED'
            LIMIT 1";

        // Thêm nhiều chi tiết đơn hàng (batch insert)
        public const string InsertOrderDetail = @"
            INSERT INTO OrderDetails (order_id, product_id, quantity, unit_price)
            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)";

        // Cập nhật nhiều chi tiết (batch update)
        public const string UpdateOrderDetail = @"
            UPDATE OrderDetails 
            SET product_id = @ProductId, 
                quantity = @Quantity, 
                unit_price = @UnitPrice 
            WHERE order_detail_id = @OrderDetailId";
    }
}
