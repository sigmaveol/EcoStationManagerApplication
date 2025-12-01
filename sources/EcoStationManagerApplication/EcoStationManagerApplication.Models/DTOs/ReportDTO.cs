using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Models.DTOs
{
    // DTOs cho báo cáo
    public class RevenueReportDTO
    {
        public decimal TotalRevenue { get; set; }
        public decimal DailyRevenue { get; set; }
        public decimal WeeklyRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<RevenueDataPoint> DataPoints { get; set; } = new List<RevenueDataPoint>();
    }

    public class RevenueDataPoint
    {
        public string Period { get; set; }
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class CustomerReturnReportDTO
    {
        public int TotalCustomers { get; set; }
        public int ReturningCustomers { get; set; }
        public double ReturnRate { get; set; }
        public double AverageReturnFrequency { get; set; }
        public List<CustomerReturnData> CustomerData { get; set; } = new List<CustomerReturnData>();
    }

    public class CustomerReturnData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public int TotalOrders { get; set; }
        public int ReturnCount { get; set; }
        public double ReturnFrequency { get; set; }
        public DateTime LastOrderDate { get; set; }
    }

    public class PackagingRecoveryReportDTO
    {
        public int TotalIssued { get; set; }
        public int TotalReturned { get; set; }
        public double RecoveryRate { get; set; }
        public List<PackagingRecoveryData> PackagingData { get; set; } = new List<PackagingRecoveryData>();
    }

    public class PackagingRecoveryData
    {
        public int PackagingId { get; set; }
        public string PackagingName { get; set; }
        public int Issued { get; set; }
        public int Returned { get; set; }
        public double RecoveryRate { get; set; }
    }

    public class EnvironmentalImpactReportDTO
    {
        public int TotalRefills { get; set; }
        public decimal PlasticSavedKg { get; set; }
        public decimal PlasticSavedTons { get; set; }
        public decimal CO2SavedKg { get; set; }
        public decimal CO2SavedTons { get; set; }
        public List<EnvironmentalImpactDataPoint> DataPoints { get; set; } = new List<EnvironmentalImpactDataPoint>();
    }

    public class EnvironmentalImpactDataPoint
    {
        public string Period { get; set; }
        public DateTime Date { get; set; }
        public int Refills { get; set; }
        public decimal PlasticSavedKg { get; set; }
        public decimal CO2SavedKg { get; set; }
    }
}
