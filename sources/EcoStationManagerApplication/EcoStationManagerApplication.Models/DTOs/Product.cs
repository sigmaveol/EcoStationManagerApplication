using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
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
}
