using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{

    public static class PackagingTransactionQueries
    {
        // Lấy lịch sử giao dịch theo bao bì
        public const string GetByPackaging = @"
        SELECT pt.*, p.name as packaging_name, c.name as customer_name, u.fullname as created_by_name
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Customers c ON pt.customer_id = c.customer_id
        LEFT JOIN Users u ON pt.user_id = u.user_id
        WHERE pt.packaging_id = @PackagingId
        ORDER BY pt.created_date DESC";

        // Lấy lịch sử giao dịch theo khách hàng
        public const string GetByCustomer = @"
        SELECT pt.*, p.name as packaging_name, p.barcode, u.fullname as created_by_name
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Users u ON pt.user_id = u.user_id
        WHERE pt.customer_id = @CustomerId
        ORDER BY pt.created_date DESC";

        // Lấy lịch sử giao dịch theo loại
        public const string GetByType = @"
        SELECT pt.*, p.name as packaging_name, c.name as customer_name, u.fullname as created_by_name
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Customers c ON pt.customer_id = c.customer_id
        LEFT JOIN Users u ON pt.user_id = u.user_id
        WHERE pt.type = @Type
        ORDER BY pt.created_date DESC";

        // Lấy lịch sử giao dịch theo khoảng thời gian
        public const string GetByDateRange = @"
        SELECT pt.*, p.name as packaging_name, c.name as customer_name, u.fullname as created_by_name
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Customers c ON pt.customer_id = c.customer_id
        LEFT JOIN Users u ON pt.user_id = u.user_id
        WHERE pt.created_date BETWEEN @StartDate AND @EndDate
        ORDER BY pt.created_date DESC";

        // Tổng tiền ký quỹ
        public const string GetTotalDepositAmount = @"
        SELECT COALESCE(SUM(deposit_price * quantity), 0)
        FROM PackagingTransactions
        WHERE type = 0";

        // Tổng tiền hoàn trả
        public const string GetTotalRefundAmount = @"
        SELECT COALESCE(SUM(refund_amount * quantity), 0)
        FROM PackagingTransactions
        WHERE type = 1";

        // Số lượng bao bì đang được khách hàng giữ
        public const string GetCustomerHoldingQuantity = @"
        SELECT COALESCE(SUM(
            CASE 
                WHEN type = 0 THEN quantity 
                WHEN type = 1 THEN -quantity 
                ELSE 0
            END
        ), 0) as holding_quantity
        FROM PackagingTransactions
        WHERE customer_id = @CustomerId AND packaging_id = @PackagingId";

        // Tất cả bao bì khách hàng đang giữ
        public const string GetCustomerHoldings = @"
        SELECT 
            pt.packaging_id,
            p.name as packaging_name,
            p.barcode as packaging_barcode,
            SUM(CASE WHEN pt.type = 0 THEN pt.quantity ELSE -pt.quantity END) as holding_quantity,
            SUM(CASE WHEN pt.type = 0 THEN pt.deposit_price * pt.quantity ELSE 0 END) as total_deposit,
            MAX(CASE WHEN pt.type = 0 THEN pt.created_date END) as last_issue_date
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        WHERE pt.customer_id = @CustomerId
        GROUP BY pt.packaging_id, p.name, p.barcode
        HAVING holding_quantity > 0";

        // Kiểm tra khách hàng có đang giữ bao bì không
        public const string IsCustomerHoldingPackaging = @"
        SELECT 1 FROM (
            SELECT packaging_id, 
                    SUM(CASE WHEN type = 0 THEN quantity ELSE -quantity END) as net_quantity
            FROM PackagingTransactions
            WHERE customer_id = @CustomerId
            GROUP BY packaging_id
            HAVING net_quantity > 0
        ) as holdings
        WHERE packaging_id = @PackagingId";

        // Phát hành bao bì
        public const string InsertTransaction = @"
        INSERT INTO PackagingTransactions 
        (packaging_id, ref_product_id, customer_id, user_id, type, ownership_type, quantity, deposit_price, refund_amount, notes)
        VALUES (@PackagingId, @RefProductId, @CustomerId, @UserId, @Type, @OwnershipType, @Quantity, @DepositPrice, @RefundAmount, @Notes)";

        // Lấy chi tiết giao dịch
        public const string GetTransactionDetails = @"
        SELECT 
            pt.transaction_id,
            pt.packaging_id,
            p.name as packaging_name,
            p.barcode as packaging_barcode,
            pt.customer_id,
            c.name as customer_name,
            c.phone as customer_phone,
            pt.type as transaction_type,
            pt.ownership_type,
            pt.quantity,
            pt.deposit_price,
            pt.refund_amount,
            pt.notes,
            u.fullname as created_by_name,
            pt.created_date
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Customers c ON pt.customer_id = c.customer_id
        LEFT JOIN Users u ON pt.user_id = u.user_id
        WHERE 1=1";

        // Thống kê giao dịch theo bao bì
        public const string GetTransactionSummaryByPackaging = @"
        SELECT 
            p.packaging_id,
            p.name as packaging_name,
            SUM(CASE WHEN pt.type = 0 THEN pt.quantity ELSE 0 END) as total_issued,
            SUM(CASE WHEN pt.type = 1 THEN pt.quantity ELSE 0 END) as total_returned,
            SUM(CASE WHEN pt.type = 0 THEN pt.quantity ELSE -pt.quantity END) as net_quantity,
            SUM(CASE WHEN pt.type = 0 THEN pt.deposit_price * pt.quantity ELSE 0 END) as total_deposit,
            SUM(CASE WHEN pt.type = 1 THEN pt.refund_amount * pt.quantity ELSE 0 END) as total_refund,
            SUM(CASE WHEN pt.type = 0 THEN pt.deposit_price * pt.quantity ELSE -pt.refund_amount * pt.quantity END) as net_amount
        FROM Packaging p
        LEFT JOIN PackagingTransactions pt ON p.packaging_id = pt.packaging_id
        WHERE 1=1
        GROUP BY p.packaging_id, p.name
        ORDER BY net_quantity DESC";

        // Thống kê giao dịch theo khách hàng
        public const string GetTransactionSummaryByCustomer = @"
        SELECT 
            c.customer_id,
            c.name as customer_name,
            c.phone as customer_phone,
            COUNT(DISTINCT pt.packaging_id) as total_packaging_types,
            SUM(CASE WHEN pt.type = 0 THEN pt.quantity ELSE -pt.quantity END) as total_holding_quantity,
            SUM(CASE WHEN pt.type = 0 THEN pt.deposit_price * pt.quantity ELSE 0 END) as total_deposit_paid,
            SUM(CASE WHEN pt.type = 1 THEN pt.refund_amount * pt.quantity ELSE 0 END) as total_refund_received,
            MAX(pt.created_date) as last_transaction_date
        FROM Customers c
        LEFT JOIN PackagingTransactions pt ON c.customer_id = pt.customer_id
        WHERE c.is_active = TRUE
        GROUP BY c.customer_id, c.name, c.phone
        HAVING total_holding_quantity > 0
        ORDER BY total_holding_quantity DESC";

        // Tổng số bao bì đang được khách hàng giữ
        public const string GetTotalCustomerHoldings = @"
        SELECT COUNT(*) FROM (
            SELECT customer_id, packaging_id
            FROM PackagingTransactions
            GROUP BY customer_id, packaging_id
            HAVING SUM(CASE WHEN type = 0 THEN quantity ELSE -quantity END) > 0
        ) as active_holdings";

        // Phân trang giao dịch
        public const string PagedTransactionsBase = @"
        SELECT pt.*, p.name as packaging_name, c.name as customer_name, u.fullname as created_by_name
        FROM PackagingTransactions pt
        JOIN Packaging p ON pt.packaging_id = p.packaging_id
        LEFT JOIN Customers c ON pt.customer_id = c.customer_id
        LEFT JOIN Users u ON pt.user_id = u.user_id";

        // Count cho phân trang
        public const string PagedTransactionsCount = @"
        SELECT COUNT(*)
        FROM PackagingTransactions pt
        WHERE 1=1";
    }
}
