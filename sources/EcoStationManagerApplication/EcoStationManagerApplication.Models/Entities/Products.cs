using EcostationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using static EcoStationManagerApplication.Models.Entities.Order;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CategoryType { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Category Parent { get; set; }
        public List<Category> Children { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? BasePrice { get; set; }
        public string UnitMeasure { get; set; }
        public ProductType ProductType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryId { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public List<Variant> Variants { get; set; } = new List<Variant>();
    }

    public class Variant
    {
        public int VariantId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public string QrCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int ProductId { get; set; }
        public int TypeId { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public VariantType VariantType { get; set; }
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
    }

    public class VariantType
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Combo
    {
        public int ComboId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public List<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public class ComboItem
    {
        public int ComboId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }

        // Navigation properties
        public Combo Combo { get; set; }
        public Variant Variant { get; set; }
    }
}