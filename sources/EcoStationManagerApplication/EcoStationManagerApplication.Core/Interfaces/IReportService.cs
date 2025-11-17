using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IReportService
    {
        // Báo cáo doanh thu
        Task<Result<RevenueReportDTO>> GetRevenueReportAsync(DateTime fromDate, DateTime toDate, string periodType = "day");
        Task<Result<List<RevenueDataPoint>>> GetRevenueByDayAsync(DateTime fromDate, DateTime toDate);
        Task<Result<List<RevenueDataPoint>>> GetRevenueByWeekAsync(DateTime fromDate, DateTime toDate);
        Task<Result<List<RevenueDataPoint>>> GetRevenueByMonthAsync(DateTime fromDate, DateTime toDate);

        // Báo cáo khách hàng
        Task<Result<CustomerReturnReportDTO>> GetCustomerReturnReportAsync(DateTime fromDate, DateTime toDate);
        Task<Result<List<CustomerReturnData>>> GetCustomerReturnFrequencyAsync(DateTime fromDate, DateTime toDate);

        // Báo cáo bao bì
        Task<Result<PackagingRecoveryReportDTO>> GetPackagingRecoveryReportAsync(DateTime fromDate, DateTime toDate);
        Task<Result<List<PackagingRecoveryData>>> GetPackagingRecoveryRateAsync(DateTime fromDate, DateTime toDate);

        // Báo cáo tác động môi trường
        Task<Result<EnvironmentalImpactReportDTO>> GetEnvironmentalImpactReportAsync(DateTime fromDate, DateTime toDate);
    }
}

