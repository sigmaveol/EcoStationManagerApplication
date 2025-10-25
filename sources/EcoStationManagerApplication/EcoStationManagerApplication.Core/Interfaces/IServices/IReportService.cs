using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IReportService
    {
        // Sales Reports
        Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate, int? stationId = null);
        Task<IEnumerable<SalesByProductReport>> GetSalesByProductReportAsync(DateTime startDate, DateTime endDate);

        // Inventory Reports
        Task<InventorySummaryReport> GetInventorySummaryReportAsync(int stationId);
        Task<IEnumerable<StockMovementReport>> GetStockMovementReportAsync(DateTime startDate, DateTime endDate, int variantId);

        // Customer Reports
        Task<CustomerLoyaltyReport> GetCustomerLoyaltyReportAsync();
        Task<IEnumerable<PackagingReturnReport>> GetPackagingReturnReportAsync(DateTime startDate, DateTime endDate);

        // Environmental Impact Reports
        Task<EnvironmentalImpactReport> GetEnvironmentalImpactReportAsync(DateTime startDate, DateTime endDate);
    }

    // Report DTOs
    public class SalesReport
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        // ... other properties
    }

    public class SalesByProductReport
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class InventorySummaryReport
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
    }

    public class StockMovementReport
    {
        public DateTime Date { get; set; }
        public string MovementType { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CustomerLoyaltyReport
    {
        public int TotalCustomers { get; set; }
        public int RepeatCustomers { get; set; }
        public decimal RepeatCustomerRate { get; set; }
    }

    public class PackagingReturnReport
    {
        public string StationName { get; set; }
        public int PackagingIssued { get; set; }
        public int PackagingReturned { get; set; }
        public decimal ReturnRate { get; set; }
    }

    public class EnvironmentalImpactReport
    {
        public int PlasticReduced { get; set; } // in kg
        public int PackagingReused { get; set; }
        public decimal CarbonFootprintReduced { get; set; } // in kg CO2
    }
}