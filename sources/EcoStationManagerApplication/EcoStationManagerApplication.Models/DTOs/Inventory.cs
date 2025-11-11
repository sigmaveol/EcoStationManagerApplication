using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    public class InventoryAlert
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string AlertType { get; set; } // LOW_STOCK, EXPIRING, OUT_OF_STOCK
        public decimal CurrentQuantity { get; set; }
        public decimal MinStockLevel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; } // INFO, WARNING, DANGER
    }

    public class InventorySummary
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int ExpiringItems { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public class InventoryHistory
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } // STOCK_IN, STOCK_OUT
        public decimal Quantity { get; set; }
        public string BatchNo { get; set; }
        public string Reference { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class StockLevelInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; } // NORMAL, LOW, OUT
        public DateTime? NearestExpiry { get; set; }
    }
}
