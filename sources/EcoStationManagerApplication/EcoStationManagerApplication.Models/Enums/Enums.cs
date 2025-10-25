

namespace EcostationManagerApplication.Models.Enums
{
    // OrdersStatus, OrderSource, PaymentMethod, PaymentStatus
    // StationType, ProductType, StockAlertType
    // CustomerStatus, QualityStatus, 

    public enum OrderStatus
    {
        Draft,
        Confirmed,
        Processing,
        Ready,
        Shipped,
        Completed,
        Cancelled,
        Returned
    }

    public enum OrderSource
    {
        GoogleForm,
        Excel,
        Email,
        Manual,
        Other
    }

    public enum PaymentMethod
    {
        Cash,
        Transfer,
        EWallet
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded,
        Other
    }

    public enum StationType
    {
        Warehouse,
        Refill,
        Hybrid,
        Other
    }

    public enum ProductType
    {
        Single,
        Service,
        Other
    }

    public enum CustomerStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public enum QualityStatus
    {
        Good,
        Pending,
        Expired,
        Rejected,
        Other
    }

    public enum StockAlertType
    {
        LowStock,
        Overstock,
        ExpiryWarning,
        Other
    }
}