using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public CategoryType CategoryType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public decimal MinStockLevel { get; set; }
        public int? CategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class Packaging
    {
        public int PackagingId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // bottle, box, container
        public decimal DepositPrice { get; set; }
    }
}
