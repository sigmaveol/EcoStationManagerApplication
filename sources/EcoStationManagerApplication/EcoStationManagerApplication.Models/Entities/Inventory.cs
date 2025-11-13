using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("Inventories")]
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("inventory_id")]
        public int InventoryId { get; set; }

        [StringLength(100)]
        [Column("batch_no")]
        public string BatchNo { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; } = 0;

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    [Table("PackagingInventories")]
    public class PackagingInventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pk_inv_id")]
        public int PkInvId { get; set; }

        [Required]
        [Column("packaging_id")]
        public int PackagingId { get; set; }

        [Column("qty_new")]
        public int QtyNew { get; set; } = 0;

        [Column("qty_in_use")]
        public int QtyInUse { get; set; } = 0;

        [Column("qty_returned")]
        public int QtyReturned { get; set; } = 0;

        [Column("qty_need_cleaning")]
        public int QtyNeedCleaning { get; set; } = 0;

        [Column("qty_cleaned")]
        public int QtyCleaned { get; set; } = 0;

        [Column("qty_damaged")]
        public int QtyDamaged { get; set; } = 0;

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("PackagingId")]
        public virtual Packaging Packaging { get; set; }
    }

    [Table("StockIn")]
    public class StockIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("stockin_id")]
        public int StockInId { get; set; }

        [StringLength(100)]
        [Column("batch_no")]
        public string BatchNo { get; set; }

        [Required]
        [Column("ref_type")]
        public RefType RefType { get; set; }

        [Required]
        [Column("ref_id")]
        public int RefId { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; } = 0.00m;

        [Column("notes")]
        public string Notes { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
    }



    [Table("StockOut")]
    public class StockOut
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("stockout_id")]
        public int StockOutId { get; set; }

        [StringLength(100)]
        [Column("batch_no")]
        public string BatchNo { get; set; }

        [Required]
        [Column("ref_type")]
        public RefType RefType { get; set; }

        [Required]
        [Column("ref_id")]
        public int RefId { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("purpose")]
        public StockOutPurpose Purpose { get; set; } = StockOutPurpose.SALE;

        [Column("notes")]
        public string Notes { get; set; }

        [Required]
        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
    }
}
