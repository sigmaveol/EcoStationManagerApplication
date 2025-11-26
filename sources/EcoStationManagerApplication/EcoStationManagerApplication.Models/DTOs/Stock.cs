using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    // StockIn Summary Classes
    public class StockInSummary
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class StockInDetail
    {
        public int StockInId { get; set; }
        public string BatchNo { get; set; }
        public string RefType { get; set; }
        public int RefId { get; set; }
        public string ProductName { get; set; }
        public string PackagingName { get; set; }
        public string SupplierName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; }
    }

    public class SupplierStockInSummary
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime LastTransactionDate { get; set; }
    }

    // StockOut Summary Classes
    public class StockOutSummary
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public string MostCommonPurpose { get; set; }
    }

    public class StockOutDetail
    {
        public int StockOutId { get; set; }
        public string BatchNo { get; set; }
        public string RefType { get; set; }
        public int RefId { get; set; }
        public string ProductName { get; set; }
        public string PackagingName { get; set; }
        public decimal Quantity { get; set; }
        public StockOutPurpose Purpose { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? OrderId { get; set; }
        public string Notes { get; set; }
    }

    public class PurposeStockOutSummary
    {
        public string Purpose { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Percentage { get; set; }
    }

    // Combined Stock Summary
    public class StockMovementSummary
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal StockInQuantity { get; set; }
        public decimal StockOutQuantity { get; set; }
        public decimal ClosingStock { get; set; }
        public decimal StockInValue { get; set; }
        public decimal StockOutValue { get; set; }
    }

    // Inventory Valuation
    public class InventoryValuation
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string CategoryName { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal AverageCost { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime LastStockInDate { get; set; }
        public DateTime LastStockOutDate { get; set; }
    }
}
