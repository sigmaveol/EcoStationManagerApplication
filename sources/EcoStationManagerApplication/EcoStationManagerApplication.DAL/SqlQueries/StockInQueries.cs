using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class StockInQueries
    {
        // Lấy lịch sử nhập kho theo sản phẩm
        public const string GetByProduct = @"
            SELECT si.*, s.name as supplier_name, u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE si.ref_type = 'PRODUCT' AND si.ref_id = @ProductId
            ORDER BY si.created_date DESC";

        // Lấy lịch sử nhập kho theo nhà cung cấp
        public const string GetBySupplier = @"
            SELECT si.*, 
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.name
                       WHEN si.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Products p ON si.ref_type = 'PRODUCT' AND si.ref_id = p.product_id
            LEFT JOIN Packaging pk ON si.ref_type = 'PACKAGING' AND si.ref_id = pk.packaging_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE si.supplier_id = @SupplierId
            ORDER BY si.created_date DESC";

        // Lấy lịch sử nhập kho theo reference
        public const string GetByReference = @"
            SELECT si.*, s.name as supplier_name, u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE si.ref_type = @RefType AND si.ref_id = @RefId
            ORDER BY si.created_date DESC";

        // Lấy lịch sử nhập kho theo khoảng thời gian
        public const string GetByDateRange = @"
            SELECT si.*, 
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.name
                       WHEN si.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   s.name as supplier_name, 
                   u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Products p ON si.ref_type = 'PRODUCT' AND si.ref_id = p.product_id
            LEFT JOIN Packaging pk ON si.ref_type = 'PACKAGING' AND si.ref_id = pk.packaging_id
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE si.created_date BETWEEN @StartDate AND @EndDate
            ORDER BY si.created_date DESC";

        // Lấy lịch sử nhập kho theo số lô
        public const string GetByBatch = @"
            SELECT si.*, 
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.name
                       WHEN si.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   s.name as supplier_name, 
                   u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Products p ON si.ref_type = 'PRODUCT' AND si.ref_id = p.product_id
            LEFT JOIN Packaging pk ON si.ref_type = 'PACKAGING' AND si.ref_id = pk.packaging_id
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE si.batch_no = @BatchNo
            ORDER BY si.created_date DESC";

        // Tổng giá trị nhập kho
        public const string GetTotalStockInValue = @"
            SELECT COALESCE(SUM(si.quantity * si.unit_price), 0)
            FROM StockIn si
            WHERE 1=1";

        // Tổng số lượng nhập kho theo sản phẩm
        public const string GetTotalQuantityByProduct = @"
            SELECT COALESCE(SUM(si.quantity), 0)
            FROM StockIn si
            WHERE si.ref_type = 'PRODUCT' AND si.ref_id = @ProductId";

        // Top sản phẩm nhập nhiều nhất
        public const string GetTopStockInProducts = @"
            SELECT p.product_id, p.name as product_name, p.sku,
                   SUM(si.quantity) as total_quantity,
                   SUM(si.quantity * si.unit_price) as total_value,
                   COUNT(si.stockin_id) as stockin_count
            FROM StockIn si
            JOIN Products p ON si.ref_id = p.product_id
            WHERE si.ref_type = 'PRODUCT'";

        // Nhật ký nhập kho với thông tin chi tiết
        public const string GetStockInDetails = @"
            SELECT si.*,
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.name
                       WHEN si.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.sku
                       WHEN si.ref_type = 'PACKAGING' THEN pk.barcode
                   END as ref_code,
                   s.name as supplier_name,
                   u.fullname as created_by_name,
                   (si.quantity * si.unit_price) as total_value
            FROM StockIn si
            LEFT JOIN Products p ON si.ref_type = 'PRODUCT' AND si.ref_id = p.product_id
            LEFT JOIN Packaging pk ON si.ref_type = 'PACKAGING' AND si.ref_id = pk.packaging_id
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id
            WHERE 1=1";

        // Thống kê nhập kho theo nhà cung cấp
        public const string GetStockInBySupplier = @"
            SELECT s.supplier_id, s.name as supplier_name,
                   COUNT(si.stockin_id) as stockin_count,
                   SUM(si.quantity) as total_quantity,
                   SUM(si.quantity * si.unit_price) as total_value,
                   AVG(si.unit_price) as avg_unit_price
            FROM Suppliers s
            LEFT JOIN StockIn si ON s.supplier_id = si.supplier_id
            WHERE 1=1
            GROUP BY s.supplier_id, s.name
            ORDER BY total_value DESC";

        // Kiểm tra số lô đã tồn tại
        public const string IsBatchExists = @"
            SELECT 1 FROM StockIn 
            WHERE batch_no = @BatchNo 
            AND ref_type = @RefType 
            AND ref_id = @RefId
            LIMIT 1";

        // Phân trang nhập kho
        public const string PagedStockInsBase = @"
            SELECT si.*,
                   CASE 
                       WHEN si.ref_type = 'PRODUCT' THEN p.name
                       WHEN si.ref_type = 'PACKAGING' THEN pk.name
                   END as ref_name,
                   s.name as supplier_name,
                   u.fullname as created_by_name
            FROM StockIn si
            LEFT JOIN Products p ON si.ref_type = 'PRODUCT' AND si.ref_id = p.product_id
            LEFT JOIN Packaging pk ON si.ref_type = 'PACKAGING' AND si.ref_id = pk.packaging_id
            LEFT JOIN Suppliers s ON si.supplier_id = s.supplier_id
            LEFT JOIN Users u ON si.created_by = u.user_id";

        // Count cho phân trang
        public const string PagedStockInsCount = @"
            SELECT COUNT(*) 
            FROM StockIn si
            WHERE 1=1";
    }
}
