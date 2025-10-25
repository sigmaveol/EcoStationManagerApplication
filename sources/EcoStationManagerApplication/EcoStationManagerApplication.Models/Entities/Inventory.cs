using EcostationManagerApplication.Models.Enums;
using System;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int? BatchId { get; set; }
        public int VariantId { get; set; }
        public int StationId { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal ReservedStock { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public Batch Batch { get; set; }
        public Variant Variant { get; set; }
        public Station Station { get; set; }
    }

    public class Batch
    {
        public int BatchId { get; set; }
        public string BatchNo { get; set; }
        public int VariantId { get; set; }
        public int StationId { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal InitialQuantity { get; set; }
        public decimal CurrentQuantity { get; set; }
        public QualityStatus QualityStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public Variant Variant { get; set; }
        public Station Station { get; set; }
    }

    public class StockIn
    {
        public int StockInId { get; set; }
        public string ReferenceNumber { get; set; }
        public int BatchId { get; set; }
        public int? StationId { get; set; }
        public int VariantId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? SupplierId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string SourceType { get; set; }
        public string QualityCheck { get; set; }
        public string Notes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Navigation properties
        public Batch Batch { get; set; }
        public Station Station { get; set; }
        public Variant Variant { get; set; }
        public Supplier Supplier { get; set; }
        public User CreatedByUser { get; set; }
    }

    public class StockOut
    {
        public int StockOutId { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Quantity { get; set; }
        public string Purpose { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StationId { get; set; }
        public int? DestStationId { get; set; }
        public int VariantId { get; set; }
        public int? OrderId { get; set; }
        public int CreatedBy { get; set; }
        public int BatchId { get; set; }
        
        // Navigation properties
        public Batch Batch { get; set; }
        public Station Station { get; set; }
        public Station DestStation { get; set; }
        public Variant Variant { get; set; }
        public Order Order { get; set; }
        public User CreatedByUser { get; set; }
    }
    public class StockAlert
    {
        public int AlertId { get; set; }
        public int VariantId { get; set; }
        public int StationId { get; set; }
        public string AlertType { get; set; }
        public decimal ThresholdValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Variant Variant { get; set; }
        public Station Station { get; set; }
    }

    public class StockAudit
    {
        public int AuditId { get; set; }
        public int StationId { get; set; }
        public int VariantId { get; set; }
        public int? BatchId { get; set; }
        public decimal SystemQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal Variance { get; set; }
        public string Reason { get; set; }
        public DateTime AuditDate { get; set; }
        public int AuditedBy { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public Station Station { get; set; }
        public Variant Variant { get; set; }
        public Batch Batch { get; set; }
        public User AuditedByUser { get; set; }
    }

    public class VariantStockRule
    {
        public int RuleId { get; set; }
        public int VariantId { get; set; }
        public int StationId { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public int ExpiryDays { get; set; }

        // Navigation properties
        public Variant Variant { get; set; }
        public Station Station { get; set; }
    }

}