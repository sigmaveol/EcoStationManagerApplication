using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EcoStationManagerApplication.Tests
{
    class FakeReportService : IReportService
    {
        public Task<Result<RevenueReportDTO>> GetRevenueReportAsync(DateTime fromDate, DateTime toDate, string periodType = "day")
            => Task.FromResult(new Result<RevenueReportDTO>(true, new RevenueReportDTO { PeriodType = periodType }, null));
        public Task<Result<List<RevenueDataPoint>>> GetRevenueByDayAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<List<RevenueDataPoint>>(true, new List<RevenueDataPoint> { new RevenueDataPoint { Label = "2024-01-01", Value = 100m } }, null));
        public Task<Result<List<RevenueDataPoint>>> GetRevenueByWeekAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<List<RevenueDataPoint>>(true, new List<RevenueDataPoint> { new RevenueDataPoint { Label = "W1", Value = 700m } }, null));
        public Task<Result<List<RevenueDataPoint>>> GetRevenueByMonthAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<List<RevenueDataPoint>>(true, new List<RevenueDataPoint> { new RevenueDataPoint { Label = "2024-01", Value = 3000m } }, null));
        public Task<Result<CustomerReturnReportDTO>> GetCustomerReturnReportAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<CustomerReturnReportDTO>(true, new CustomerReturnReportDTO(), null));
        public Task<Result<List<CustomerReturnData>>> GetCustomerReturnFrequencyAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<List<CustomerReturnData>>(true, new List<CustomerReturnData>(), null));
        public Task<Result<PackagingRecoveryReportDTO>> GetPackagingRecoveryReportAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<PackagingRecoveryReportDTO>(true, new PackagingRecoveryReportDTO(), null));
        public Task<Result<List<PackagingRecoveryData>>> GetPackagingRecoveryRateAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<List<PackagingRecoveryData>>(true, new List<PackagingRecoveryData>(), null));
        public Task<Result<EnvironmentalImpactReportDTO>> GetEnvironmentalImpactReportAsync(DateTime fromDate, DateTime toDate)
            => Task.FromResult(new Result<EnvironmentalImpactReportDTO>(true, new EnvironmentalImpactReportDTO(), null));
    }

    [TestClass]
    public class CoreReportServiceTests
    {
        [TestMethod]
        public async Task Revenue_By_Day_Has_DataPoint()
        {
            var svc = new FakeReportService();
            var res = await svc.GetRevenueByDayAsync(DateTime.Today.AddDays(-7), DateTime.Today);
            Assert.IsTrue(res.Success);
            Assert.IsTrue(res.Data.Count > 0);
        }
    }
}
