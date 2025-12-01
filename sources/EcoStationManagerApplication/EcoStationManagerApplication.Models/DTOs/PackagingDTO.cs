using System;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class PackagingDTO
    {
        public int PackagingId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal DepositPrice { get; set; }
    }

    public class CreatePackagingDTO
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal DepositPrice { get; set; }
    }

    public class UpdatePackagingDTO
    {
        public int PackagingId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal DepositPrice { get; set; }
    }

    public class PackagingInventoryDTO
    {
        public int PkInvId { get; set; }
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public string PackagingBarcode { get; set; }
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class UpdatePackagingInventoryDTO
    {
        public int PkInvId { get; set; }
        public int QtyNew { get; set; }
        public int QtyInUse { get; set; }
        public int QtyReturned { get; set; }
        public int QtyNeedCleaning { get; set; }
        public int QtyCleaned { get; set; }
        public int QtyDamaged { get; set; }
    }
}

