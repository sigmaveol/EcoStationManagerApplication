using EcoStationManagerApplication.DAL.SqlQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IReportRepository
    {
        // Doanh thu
        Task<dynamic> GetTotalRevenueAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<dynamic>> GetRevenueByDayAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<dynamic>> GetRevenueByWeekAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<dynamic>> GetRevenueByMonthAsync(DateTime fromDate, DateTime toDate);

        // Khách hàng
        Task<IEnumerable<dynamic>> GetCustomerReturnFrequencyAsync(DateTime fromDate, DateTime toDate);

        // Bao bì
        Task<dynamic> GetTotalPackagingStatsAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<dynamic>> GetPackagingRecoveryRateAsync(DateTime fromDate, DateTime toDate);

        // Tác động môi trường
        Task<dynamic> GetTotalEnvironmentalImpactAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<dynamic>> GetEnvironmentalImpactAsync(DateTime fromDate, DateTime toDate);
    }
}

