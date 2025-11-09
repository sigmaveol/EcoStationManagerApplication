using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    // Supporting classes for return types
    public class PackagingQuantities
    {
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
    }

    public class PackagingTransactionDetail
    {
        public int TransactionId { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public string PackagingBarcode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public PackagingTransactionType TransactionType { get; set; }
        public PackagingOwnershipType OwnershipType { get; set; }
        public int Quantity { get; set; }
        public decimal DepositPrice { get; set; }
        public decimal RefundAmount { get; set; }
        public string Notes { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PackagingTransactionSummary
    {
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int TotalIssued { get; set; }
        public int TotalReturned { get; set; }
        public int NetQuantity { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalRefund { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class CustomerPackagingSummary
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int TotalPackagingTypes { get; set; }
        public int TotalHoldingQuantity { get; set; }
        public decimal TotalDepositPaid { get; set; }
        public decimal TotalRefundReceived { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }

    public class CustomerHoldingPackaging
    {
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public string PackagingBarcode { get; set; }
        public int HoldingQuantity { get; set; }
        public decimal TotalDeposit { get; set; }
        public DateTime LastIssueDate { get; set; }
    }
}
