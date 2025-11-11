using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OrderSource Source { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }

    public class CreateOrderDTO
    {
        public int? CustomerId { get; set; }
        public OrderSource Source { get; set; }
        public decimal DiscountedAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
    }

    public class CreateOrderDetailDTO
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }

    public class UpdateOrderStatusDTO
    {
        public OrderStatus NewStatus { get; set; }
        public string Note { get; set; }
    }

    public class UpdatePaymentStatusDTO
    {
        public PaymentStatus NewStatus { get; set; }
        public string Note { get; set; }
    }

    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime LastUpdated { get; set; }
        public int ItemCount { get; set; }
    }

    public class OrderFilterDTO
    {
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchKeyword { get; set; }
    }
}
