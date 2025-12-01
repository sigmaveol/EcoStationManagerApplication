using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class StockOutQueries
    {
        // Lấy lịch sử xuất kho theo sản phẩm
        public const string GetByProduct = @"
            SELECT so.*, u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE so.ref_type = 'PRODUCT' AND so.ref_id = @ProductId
            ORDER BY so.created_date DESC";

        // Lấy lịch sử xuất kho theo reference
        public const string GetByReference = @"
            SELECT so.*, u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE so.ref_type = @RefType AND so.ref_id = @RefId
            ORDER BY so.created_date DESC";

        // Lấy lịch sử xuất kho theo khoảng thời gian
        public const string GetByDateRange = @"
            SELECT so.*, 
                   CASE 
                       WHEN so.ref_type = 'PRODUCT' THEN p.name
                       WHEN so.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE so.created_date BETWEEN @StartDate AND @EndDate
            ORDER BY so.created_date DESC";

        // Lấy lịch sử xuất kho theo mục đích
        public const string GetByPurpose = @"
            SELECT so.*, 
                   CASE 
                       WHEN so.ref_type = 'PRODUCT' THEN p.name
                       WHEN so.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE so.purpose = @Purpose
            ORDER BY so.created_date DESC";

        // Lấy lịch sử xuất kho theo số lô
        public const string GetByBatch = @"
            SELECT so.*, 
                   CASE 
                       WHEN so.ref_type = 'PRODUCT' THEN p.name
                       WHEN so.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE so.batch_no = @BatchNo
            ORDER BY so.created_date DESC";

        // Tổng giá trị xuất kho
        public const string GetTotalStockOutValue = @"
            SELECT COALESCE(SUM(so.quantity * 
                CASE 
                    WHEN so.ref_type = 'PRODUCT' THEN p.price
                    WHEN so.ref_type = 'PACKAGING' THEN pk.deposit_price
                    ELSE 0 
                END), 0)
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            WHERE 1=1";

        // Tổng số lượng xuất kho theo sản phẩm
        public const string GetTotalQuantityByProduct = @"
            SELECT COALESCE(SUM(so.quantity), 0)
            FROM StockOut so
            WHERE so.ref_type = 'PRODUCT' AND so.ref_id = @ProductId";

        // Top sản phẩm xuất nhiều nhất
        public const string GetTopStockOutProducts = @"
            SELECT p.product_id, p.name as product_name, p.sku,
                   SUM(so.quantity) as total_quantity,
                   SUM(so.quantity * p.price) as total_value,
                   COUNT(so.stockout_id) as stockout_count
            FROM StockOut so
            JOIN Products p ON so.ref_id = p.product_id
            WHERE so.ref_type = 'PRODUCT'";

        // Xuất kho cho đơn hàng
        public const string StockOutForOrder = @"
            INSERT INTO StockOut (batch_no, ref_type, ref_id, quantity, purpose, notes, created_by)
            VALUES (@BatchNo, 'PRODUCT', @ProductId, @Quantity, 'SALE', @Notes, @UserId)";

        // Nhật ký xuất kho với thông tin chi tiết
        public const string GetStockOutDetails = @"
            SELECT so.stockout_id as StockOutId,
                   so.batch_no as BatchNo,
                   so.ref_type as RefType,
                   so.ref_id as RefId,
                   so.quantity as Quantity,
                   so.purpose as Purpose,
                   so.notes as Notes,
                   so.created_by,
                   so.created_date as CreatedDate,
                   CASE 
                       WHEN so.ref_type = 'PRODUCT' THEN p.name
                       ELSE NULL
                   END as ProductName,
                   CASE 
                       WHEN so.ref_type = 'PACKAGING' THEN pk.name
                       ELSE NULL
                   END as PackagingName,
                   u.fullname as CreatedBy
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            LEFT JOIN Users u ON so.created_by = u.user_id
            WHERE 1=1";

        // Thống kê xuất kho theo mục đích
        public const string GetStockOutByPurpose = @"
            SELECT so.purpose,
                   COUNT(so.stockout_id) as stockout_count,
                   SUM(so.quantity) as total_quantity,
                   SUM(so.quantity * 
                       CASE 
                           WHEN so.ref_type = 'PRODUCT' THEN p.price
                           WHEN so.ref_type = 'PACKAGING' THEN pk.deposit_price
                           ELSE 0 
                       END) as total_value
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            WHERE 1=1
            GROUP BY so.purpose
            ORDER BY total_quantity DESC";

        // Kiểm tra có thể xuất kho (đủ số lượng)
        public const string CanStockOut = @"
            SELECT 1 FROM Inventories 
            WHERE product_id = @ProductId 
            AND batch_no = @BatchNo 
            AND quantity >= @Quantity
            LIMIT 1";

        // Phân trang xuất kho
        public const string PagedStockOutsBase = @"
            SELECT so.*,
                   CASE 
                       WHEN so.ref_type = 'PRODUCT' THEN p.name
                       WHEN so.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   u.fullname as created_by_name
            FROM StockOut so
            LEFT JOIN Products p ON so.ref_type = 'PRODUCT' AND so.ref_id = p.product_id
            LEFT JOIN Packaging pk ON so.ref_type = 'PACKAGING' AND so.ref_id = pk.packaging_id
            LEFT JOIN Users u ON so.created_by = u.user_id";

        // Count cho phân trang
        public const string PagedStockOutsCount = @"
            SELECT COUNT(*) 
            FROM StockOut so
            WHERE 1=1";

        // Insert StockOut
        public const string InsertStockOut = @"
            INSERT INTO StockOut (batch_no, ref_type, ref_id, quantity, purpose, notes, created_by)
            VALUES (@BatchNo, @RefType, @RefId, @Quantity, @Purpose, @Notes, @CreatedBy)";
    }
}
