using EcostationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public OrderSource Source { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public int? StationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public User CreatedBy { get; set; }
        public Customer Customer { get; set; }
        public Station Station { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }
        public int? ComboId { get; set; }
        public int? VariantId { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Combo Combo { get; set; }
        public Variant Variant { get; set; }
    }
}