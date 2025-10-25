using System;
using System.Collections.Generic;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public int? OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public List<Refund> Refunds { get; set; } = new List<Refund>();
    }

    public class Refund
    {
        public int RefundId { get; set; }
        public string RefundNumber { get; set; }
        public int OrderId { get; set; }
        public int? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Payment Payment { get; set; }
        public User CreatedByUser { get; set; }
    }
}