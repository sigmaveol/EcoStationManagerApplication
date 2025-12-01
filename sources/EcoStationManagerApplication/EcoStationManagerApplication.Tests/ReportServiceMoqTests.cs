using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class ReportServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IReportRepository> _repo;
        private ReportService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IReportRepository>();
            _uow.Setup(u => u.Reports).Returns(_repo.Object);
            _svc = new ReportService(_uow.Object);
        }

        [TestMethod]
        public async Task GetRevenueReport_Day_Aggregates()
        {
            var from = DateTime.Today.AddDays(-7);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetTotalRevenueAsync(from, to)).ReturnsAsync(new { total_revenue = (decimal?)1000m, total_orders = (int?)10, avg_order_value = (decimal?)100m });
            _repo.Setup(r => r.GetRevenueByDayAsync(from, to)).ReturnsAsync(new[] { new { period = (int?)1, period_date = (DateTime?)DateTime.Today, revenue = (decimal?)200m, order_count = (int?)2 } }.AsEnumerable());

            var res = await _svc.GetRevenueReportAsync(from, to, "day");
            Assert.IsTrue(res.Success);
            Assert.AreEqual(1000m, res.Data.TotalRevenue);
            Assert.AreEqual(200m, res.Data.DailyRevenue);
        }

        [TestMethod]
        public async Task GetPackagingRecoveryReport_Aggregates()
        {
            var from = DateTime.Today.AddDays(-30);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetTotalPackagingStatsAsync(from, to)).ReturnsAsync(new { total_issued = (int?)100, total_returned = (int?)60 });
            _repo.Setup(r => r.GetPackagingRecoveryRateAsync(from, to)).ReturnsAsync(new[] { new { packaging_id = (int?)1, packaging_name = (string)"Bottle", issued = (int?)70, returned = (int?)50, recovery_rate = (double?)71.4 } }.AsEnumerable());

            var res = await _svc.GetPackagingRecoveryReportAsync(from, to);
            Assert.IsTrue(res.Success);
            Assert.AreEqual(100, res.Data.TotalIssued);
            Assert.AreEqual(60, res.Data.TotalReturned);
            Assert.IsTrue(res.Data.RecoveryRate > 0);
            Assert.IsTrue(res.Data.PackagingData.Count > 0);
        }

        [TestMethod]
        public async Task GetRevenueReport_Week_Aggregates()
        {
            var from = DateTime.Today.AddDays(-14);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetTotalRevenueAsync(from, to)).ReturnsAsync(new { total_revenue = (decimal?)2000m, total_orders = (int?)20, avg_order_value = (decimal?)100m });
            _repo.Setup(r => r.GetRevenueByWeekAsync(from, to)).ReturnsAsync(new[] { new { period = (int?)1, period_date = (DateTime?)DateTime.Today, revenue = (decimal?)800m, order_count = (int?)8 } }.AsEnumerable());

            var res = await _svc.GetRevenueReportAsync(from, to, "week");
            Assert.IsTrue(res.Success);
            Assert.AreEqual(2000m, res.Data.TotalRevenue);
            Assert.AreEqual(800m, res.Data.WeeklyRevenue);
        }

        [TestMethod]
        public async Task GetRevenueReport_Month_Aggregates()
        {
            var from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetTotalRevenueAsync(from, to)).ReturnsAsync(new { total_revenue = (decimal?)5000m, total_orders = (int?)50, avg_order_value = (decimal?)100m });
            _repo.Setup(r => r.GetRevenueByMonthAsync(from, to)).ReturnsAsync(new[] { new { period = (int?)DateTime.Today.Month, period_date = (DateTime?)DateTime.Today, revenue = (decimal?)1500m, order_count = (int?)15 } }.AsEnumerable());

            var res = await _svc.GetRevenueReportAsync(from, to, "month");
            Assert.IsTrue(res.Success);
            Assert.AreEqual(5000m, res.Data.TotalRevenue);
            Assert.AreEqual(1500m, res.Data.MonthlyRevenue);
        }

        [TestMethod]
        public async Task GetEnvironmentalImpactReport_Aggregates()
        {
            var from = DateTime.Today.AddDays(-7);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetTotalEnvironmentalImpactAsync(from, to)).ReturnsAsync(new { total_refills = (int?)100, plastic_saved_kg = (decimal?)25m, plastic_saved_tons = (decimal?)0.025m, co2_saved_kg = (decimal?)80m, co2_saved_tons = (decimal?)0.08m });
            _repo.Setup(r => r.GetEnvironmentalImpactAsync(from, to)).ReturnsAsync(new[] { new { period = (int?)1, period_date = (DateTime?)DateTime.Today, refills = (int?)20, plastic_saved_kg = (decimal?)5m, co2_saved_kg = (decimal?)16m } }.AsEnumerable());

            var res = await _svc.GetEnvironmentalImpactReportAsync(from, to);
            Assert.IsTrue(res.Success);
            Assert.AreEqual(100, res.Data.TotalRefills);
            Assert.IsTrue(res.Data.DataPoints.Count > 0);
        }

        [TestMethod]
        public async Task GetCustomerReturnReport_Aggregates()
        {
            var from = DateTime.Today.AddDays(-30);
            var to = DateTime.Today;
            _repo.Setup(r => r.GetCustomerReturnFrequencyAsync(from, to)).ReturnsAsync(new[] { new { customer_id = (int?)1, customer_name = (string)"A", phone = (string)"123", total_orders = (int?)5, return_count = (int?)3, return_frequency = (double?)0.6, last_order_date = (DateTime?)DateTime.Today } }.AsEnumerable());

            var res = await _svc.GetCustomerReturnReportAsync(from, to);
            Assert.IsTrue(res.Success);
            Assert.IsTrue(res.Data.CustomerData.Count > 0);
            Assert.IsTrue(res.Data.ReturnRate >= 0);
        }
    }
}
