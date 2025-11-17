using Dapper;
using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly ILogHelper _logger;

        public ReportRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _logger = LogHelperFactory.CreateLogger("ReportRepository");
        }

        public async Task<dynamic> GetTotalRevenueAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryFirstOrDefaultAsync<dynamic>(
                    ReportQueries.TotalRevenue,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalRevenueAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetRevenueByDayAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.RevenueByDay,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetRevenueByDayAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetRevenueByWeekAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.RevenueByWeek,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetRevenueByWeekAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetRevenueByMonthAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.RevenueByMonth,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetRevenueByMonthAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetCustomerReturnFrequencyAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.CustomerReturnFrequency,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetCustomerReturnFrequencyAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> GetTotalPackagingStatsAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryFirstOrDefaultAsync<dynamic>(
                    ReportQueries.TotalPackagingStats,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalPackagingStatsAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetPackagingRecoveryRateAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.PackagingRecoveryRate,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPackagingRecoveryRateAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> GetTotalEnvironmentalImpactAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryFirstOrDefaultAsync<dynamic>(
                    ReportQueries.TotalEnvironmentalImpact,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalEnvironmentalImpactAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<dynamic>> GetEnvironmentalImpactAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<dynamic>(
                    ReportQueries.EnvironmentalImpact,
                    new { FromDate = fromDate.Date, ToDate = toDate.Date });
            }
            catch (Exception ex)
            {
                _logger.Error($"GetEnvironmentalImpactAsync error - {ex.Message}");
                throw;
            }
        }
    }
}

