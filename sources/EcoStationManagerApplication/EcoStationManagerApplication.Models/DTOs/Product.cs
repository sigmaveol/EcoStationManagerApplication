using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class CategorySales
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalQuantity { get; set; }
        public int ProductCount { get; set; }
    }

    public class ProductSales
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ProductRevenue
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class OrderDetailSummary
    {
        public int OrderId { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public IEnumerable<ProductInOrder> Products { get; set; }
    }

    public class ProductInOrder
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
