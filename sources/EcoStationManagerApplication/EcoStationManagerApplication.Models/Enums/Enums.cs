using System;

namespace EcoStationManagerApplication.Models.Enums
{
    // Vai trò người dùng
    public enum UserRole
    {
        ADMIN,
        STAFF,
        MANAGER,
        DRIVER
    }

    // Nguồn đơn hàng
    public enum OrderSource
    {
        GOOGLEFORM,
        EXCEL,
        EMAIL,
        MANUAL
    }

    // Trạng thái đơn hàng
    public enum OrderStatus
    {
        DRAFT,
        CONFIRMED,
        PROCESSING,
        READY,
        SHIPPED,
        COMPLETED,
        CANCELLED
    }

    // Trạng thái thanh toán
    public enum PaymentStatus
    {
        UNPAID,
        PAID
    }

    // Phương thức thanh toán
    public enum PaymentMethod
    {
        CASH,
        TRANSFER
    }

    // Phân hạng khách hàng
    public enum CustomerRank
    {
        MEMBER,
        SILVER,
        GOLD,
        DIAMONDS
    }

    // Loại danh mục sản phẩm
    public enum CategoryType
    {
        PRODUCT,
        SERVICE,
        OTHER
    }

    // Loại sản phẩm (đóng gói / refill)
    public enum ProductType
    {
        PACKED,
        REFILLED,
        OTHER
    }

    // Loại tham chiếu trong nhập/xuất kho
    public enum RefType
    {
        PRODUCT,
        PACKAGING
    }

    // Mục đích xuất kho
    public enum StockOutPurpose
    {
        SALE,
        DAMAGE,
        TRANSFER
    }

    // Loại vệ sinh
    public enum CleaningType
    {
        TANK,
        PACKAGING
    }

    // Trạng thái vệ sinh
    public enum CleaningStatus
    {
        SCHEDULED,
        COMPLETED,
        OVERDUE,
        CANCELLED
    }

    // Loại giao dịch bao bì (phát / thu hồi)
    public enum PackagingTransactionType
    {
        ISSUE,
        RETURN
    }

    // Hình thức sở hữu bao bì (ký quỹ / bán đứt)
    public enum PackagingOwnershipType
    {
        DEPOSIT,
        SOLD
    }

    // Trạng thái phân công giao hàng
    public enum DeliveryStatus
    {
        PENDING,
        INTRANSIT,
        DELIVERED,
        FAILED
    }

    // Trạng thái thanh toán COD
    public enum DeliveryPaymentStatus
    {
        UNPAID,
        PAID
    }

    // Trạng thái hoạt động người dùng / sản phẩm / khách hàng
    public enum ActiveStatus
    {
        INACTIVE = 0,
        ACTIVE = 1
    }

    public enum NotificationType
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2,
        SUCCESS = 3,
        LOWSTOCK = 4,
        ORDER = 5,
        REFILL = 6,
        SYSTEM = 7
    }
}
