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
    [Table("Categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Column("category_type")]
        public CategoryType CategoryType { get; set; } = CategoryType.PRODUCT;

        [Column("is_active")]
        public ActiveStatus IsActive { get; set; } = ActiveStatus.ACTIVE;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("product_id")]
        public int ProductId { get; set; }

        [StringLength(20)]
        [Column("sku")]
        public string Sku { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [StringLength(255)]
        [Column("image")]
        public string Image { get; set; } // Nullable - ảnh được lưu vào file system, không lưu vào DB

        [Required]
        [StringLength(20)]
        [Column("product_type")]
        public ProductType ProductType { get; set; } = ProductType.PACKED;

        [Required]
        [StringLength(50)]
        [Column("unit")]
        public string Unit { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("min_stock_level", TypeName = "decimal(10,2)")]
        public decimal MinStockLevel { get; set; } = 15;

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [Column("is_active")]
        public ActiveStatus IsActive { get; set; } = ActiveStatus.ACTIVE;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [NotMapped]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }

    [Table("Packaging")]
    public class Packaging
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("packaging_id")]
        public int PackagingId { get; set; }

        [StringLength(20)]
        [Column("barcode")]
        public string Barcode { get; set; }

        [Required]
        [StringLength(150)]
        [Column("name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Column("type")]
        public string Type { get; set; }

        [Column("deposit_price", TypeName = "decimal(10,2)")]
        public decimal DepositPrice { get; set; } = 0.00m;
    }
}
