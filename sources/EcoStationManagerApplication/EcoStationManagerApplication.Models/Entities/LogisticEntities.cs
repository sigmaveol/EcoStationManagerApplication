using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    public class PackagingTransaction
    {
        public int TransactionId { get; set; }
        public int PackagingId { get; set; }
        public int? RefProductId { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public PackagingTransactionType Type { get; set; }
        public PackagingOwnershipType OwnershipType { get; set; }
        public int Quantity { get; set; }
        public decimal DepositPrice { get; set; }
        public decimal RefundAmount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DeliveryAssignment
    {
        public int AssignmentId { get; set; }
        public int OrderId { get; set; }
        public int DriverId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DeliveryStatus Status { get; set; }
        public decimal CodAmount { get; set; }
        public DeliveryPaymentStatus PaymentStatus { get; set; }
        public string Notes { get; set; }
    }
}
