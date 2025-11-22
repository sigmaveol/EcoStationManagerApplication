using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class DeliveryQueries
    {
        // Lấy DeliveryAssignment theo driver_id
        public const string GetByDriver = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE da.driver_id = @DriverId
            ORDER BY da.assigned_date DESC";

        // Lấy DeliveryAssignment theo status
        // @Status sẽ là số nguyên (TINYINT) từ enum DeliveryStatus: PENDING=0, INTRANSIT=1, DELIVERED=2, FAILED=3
        public const string GetByStatus = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE da.status = @Status
            ORDER BY da.assigned_date DESC";

        // Lấy DeliveryAssignment theo khoảng thời gian
        public const string GetByDateRange = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE DATE(da.assigned_date) BETWEEN @StartDate AND @EndDate
            ORDER BY da.assigned_date DESC";

        // Lấy các delivery đang chờ (pending)
        // DeliveryStatus: PENDING=0, INTRANSIT=1, DELIVERED=2, FAILED=3
        public const string GetPendingDeliveries = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE da.status = 0
            ORDER BY da.assigned_date ASC";

        // Lấy các delivery đã hoàn thành
        // DeliveryStatus: DELIVERED = 2
        public const string GetCompletedDeliveries = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE da.status = 2
            ORDER BY da.assigned_date DESC";

        // Tính tổng COD theo driver
        // DeliveryPaymentStatus: UNPAID=0, PAID=1
        public const string GetTotalCODByDriver = @"
            SELECT COALESCE(SUM(cod_amount), 0) as total_cod
            FROM DeliveryAssignments
            WHERE driver_id = @DriverId
            AND payment_status = 1
            AND (@StartDate IS NULL OR DATE(assigned_date) >= @StartDate)
            AND (@EndDate IS NULL OR DATE(assigned_date) <= @EndDate)";

        // Cập nhật status
        // @Status sẽ là số nguyên (TINYINT) từ enum DeliveryStatus: PENDING=0, INTRANSIT=1, DELIVERED=2, FAILED=3
        public const string UpdateStatus = @"
            UPDATE DeliveryAssignments 
            SET status = @Status
            WHERE assignment_id = @AssignmentId";

        // Cập nhật payment_status
        // @PaymentStatus sẽ là số nguyên (TINYINT) từ enum DeliveryPaymentStatus: UNPAID=0, PAID=1
        public const string UpdatePaymentStatus = @"
            UPDATE DeliveryAssignments 
            SET payment_status = @PaymentStatus
            WHERE assignment_id = @AssignmentId";

        // Lấy phân trang với tìm kiếm
        // @Status và @PaymentStatus sẽ là số nguyên (TINYINT) hoặc NULL
        // DeliveryStatus: PENDING=0, INTRANSIT=1, DELIVERED=2, FAILED=3
        // DeliveryPaymentStatus: UNPAID=0, PAID=1
        public const string GetPaged = @"
            SELECT da.*, o.order_code, o.status as order_status, u.fullname as driver_name
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE (@Search IS NULL OR o.order_code LIKE CONCAT('%', @Search, '%') OR u.fullname LIKE CONCAT('%', @Search, '%'))
            AND (@Status IS NULL OR da.status = @Status)
            AND (@PaymentStatus IS NULL OR da.payment_status = @PaymentStatus)
            ORDER BY da.assigned_date DESC
            LIMIT @PageSize OFFSET @Offset";

        // Đếm tổng số bản ghi cho phân trang
        // @Status và @PaymentStatus sẽ là số nguyên (TINYINT) hoặc NULL
        // DeliveryStatus: PENDING=0, INTRANSIT=1, DELIVERED=2, FAILED=3
        // DeliveryPaymentStatus: UNPAID=0, PAID=1
        public const string GetPagedCount = @"
            SELECT COUNT(1)
            FROM DeliveryAssignments da
            LEFT JOIN Orders o ON da.order_id = o.order_id
            LEFT JOIN Users u ON da.driver_id = u.user_id
            WHERE (@Search IS NULL OR o.order_code LIKE CONCAT('%', @Search, '%') OR u.fullname LIKE CONCAT('%', @Search, '%'))
            AND (@Status IS NULL OR da.status = @Status)
            AND (@PaymentStatus IS NULL OR da.payment_status = @PaymentStatus)";
    }
}

