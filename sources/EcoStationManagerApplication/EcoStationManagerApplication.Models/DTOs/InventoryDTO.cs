using EcoStationManagerApplication.Models.Enums;
using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class InventoryDTO
    {
        public int InventoryId { get; set; }
        public string BatchNo { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class CreateInventoryDTO
    {
        public string BatchNo { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class UpdateInventoryDTO
    {
        public int InventoryId { get; set; }
        public string BatchNo { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class StockInDTO
    {
        public int StockInId { get; set; }
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public string RefName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateStockInDTO
    {
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class StockOutDTO
    {
        public int StockOutId { get; set; }
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public string RefName { get; set; }
        public decimal Quantity { get; set; }
        public StockOutPurpose Purpose { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateStockOutDTO
    {
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public decimal Quantity { get; set; }
        public StockOutPurpose Purpose { get; set; }
        public string Notes { get; set; }
    }
}

