using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string BatchNo { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class PackagingInventory
    {
        public int PkInvId { get; set; }
        public int PackagingId { get; set; }
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class StockIn
    {
        public int StockInId { get; set; }
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class StockOut
    {
        public int StockOutId { get; set; }
        public string BatchNo { get; set; }
        public RefType RefType { get; set; }
        public int RefId { get; set; }
        public decimal Quantity { get; set; }
        public StockOutPurpose Purpose { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
