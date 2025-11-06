using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Enums
{
    public enum UserRole
    {
        ADMIN,
        STAFF,
        MANAGER,
        DRIVER
    }

    // Enum cho nguồn đơn hàng
    public enum OrderSource
    {
        GOOGLEFORM,
        EXCEL,
        EMAIL,
        MANUAL
    }

    // Enum trạng thái đơn hàng
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

    // Enum trạng thái thanh toán
    public enum PaymentStatus
    {
        UNPAID,
        PAID
    }

    // Enum phương thức thanh toán
    public enum PaymentMethod
    {
        CASH,
        TRANSFER
    }
}
