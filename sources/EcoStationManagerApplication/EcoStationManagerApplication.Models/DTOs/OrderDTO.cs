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
}
