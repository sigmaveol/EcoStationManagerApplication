using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
            : base("ReportService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RevenueReportDTO>> GetRevenueReportAsync(DateTime fromDate, DateTime toDate, string periodType = "day")
        {
            try
            {
                var report = new RevenueReportDTO();

                // Lấy tổng doanh thu
                var totalRevenueResult = await _unitOfWork.Reports.GetTotalRevenueAsync(fromDate, toDate);

                if (totalRevenueResult != null)
                {
                    report.TotalRevenue = Convert.ToDecimal(totalRevenueResult.total_revenue ?? 0);
                    report.TotalOrders = Convert.ToInt32(totalRevenueResult.total_orders ?? 0);
                    report.AverageOrderValue = Convert.ToDecimal(totalRevenueResult.avg_order_value ?? 0);
                }

                // Lấy dữ liệu theo period type
                switch (periodType.ToLower())
                {
                    case "day":
                        var dayResult = await GetRevenueByDayAsync(fromDate, toDate);
                        if (dayResult.Success)
                        {
                            report.DataPoints = dayResult.Data.ToList();
                            report.DailyRevenue = report.DataPoints.Sum(x => x.Revenue);
                        }
                        break;
                    case "week":
                        var weekResult = await GetRevenueByWeekAsync(fromDate, toDate);
                        if (weekResult.Success)
                        {
                            report.DataPoints = weekResult.Data.ToList();
                            report.WeeklyRevenue = report.DataPoints.Sum(x => x.Revenue);
                        }
                        break;
                    case "month":
                        var monthResult = await GetRevenueByMonthAsync(fromDate, toDate);
                        if (monthResult.Success)
                        {
                            report.DataPoints = monthResult.Data.ToList();
                            report.MonthlyRevenue = report.DataPoints.Sum(x => x.Revenue);
                        }
                        break;
                }

                return Result<RevenueReportDTO>.Ok(report, "Lấy báo cáo doanh thu thành công");
            }
            catch (Exception ex)
            {
                return HandleException<RevenueReportDTO>(ex, "lấy báo cáo doanh thu");
            }
        }

        public async Task<Result<List<RevenueDataPoint>>> GetRevenueByDayAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _unitOfWork.Reports.GetRevenueByDayAsync(fromDate, toDate);

                var result = data.Select(x => new RevenueDataPoint
                {
                    Period = x.period?.ToString() ?? "",
                    Date = Convert.ToDateTime(x.period_date),
                    Revenue = Convert.ToDecimal(x.revenue ?? 0),
                    OrderCount = Convert.ToInt32(x.order_count ?? 0)
                }).ToList();

                return Result<List<RevenueDataPoint>>.Ok(result, $"Lấy {result.Count} điểm dữ liệu doanh thu theo ngày");
            }
            catch (Exception ex)
            {
                return HandleException<List<RevenueDataPoint>>(ex, "lấy doanh thu theo ngày");
            }
        }

        public async Task<Result<List<RevenueDataPoint>>> GetRevenueByWeekAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _unitOfWork.Reports.GetRevenueByWeekAsync(fromDate, toDate);

                var result = data.Select(x => new RevenueDataPoint
                {
                    Period = x.period?.ToString() ?? "",
                    Date = Convert.ToDateTime(x.period_date),
                    Revenue = Convert.ToDecimal(x.revenue ?? 0),
                    OrderCount = Convert.ToInt32(x.order_count ?? 0)
                }).ToList();

                return Result<List<RevenueDataPoint>>.Ok(result, $"Lấy {result.Count} điểm dữ liệu doanh thu theo tuần");
            }
            catch (Exception ex)
            {
                return HandleException<List<RevenueDataPoint>>(ex, "lấy doanh thu theo tuần");
            }
        }

        public async Task<Result<List<RevenueDataPoint>>> GetRevenueByMonthAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _unitOfWork.Reports.GetRevenueByMonthAsync(fromDate, toDate);

                var result = data.Select(x => new RevenueDataPoint
                {
                    Period = x.period?.ToString() ?? "",
                    Date = Convert.ToDateTime(x.period_date),
                    Revenue = Convert.ToDecimal(x.revenue ?? 0),
                    OrderCount = Convert.ToInt32(x.order_count ?? 0)
                }).ToList();

                return Result<List<RevenueDataPoint>>.Ok(result, $"Lấy {result.Count} điểm dữ liệu doanh thu theo tháng");
            }
            catch (Exception ex)
            {
                return HandleException<List<RevenueDataPoint>>(ex, "lấy doanh thu theo tháng");
            }
        }

        public async Task<Result<CustomerReturnReportDTO>> GetCustomerReturnReportAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var report = new CustomerReturnReportDTO();

                var customerData = await GetCustomerReturnFrequencyAsync(fromDate, toDate);
                if (customerData.Success)
                {
                    report.CustomerData = customerData.Data.ToList();
                    report.ReturningCustomers = report.CustomerData.Count;
                    report.TotalCustomers = report.CustomerData.Count;

                    if (report.TotalCustomers > 0)
                    {
                        report.ReturnRate = (double)report.ReturningCustomers / report.TotalCustomers * 100;
                        report.AverageReturnFrequency = report.CustomerData.Average(x => x.ReturnFrequency);
                    }
                }

                return Result<CustomerReturnReportDTO>.Ok(report, "Lấy báo cáo tần suất khách hàng quay trở lại thành công");
            }
            catch (Exception ex)
            {
                return HandleException<CustomerReturnReportDTO>(ex, "lấy báo cáo tần suất khách hàng");
            }
        }

        public async Task<Result<List<CustomerReturnData>>> GetCustomerReturnFrequencyAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _unitOfWork.Reports.GetCustomerReturnFrequencyAsync(fromDate, toDate);

                var result = data.Select(x => new CustomerReturnData
                {
                    CustomerId = Convert.ToInt32(x.customer_id ?? 0),
                    CustomerName = x.customer_name?.ToString() ?? "",
                    Phone = x.phone?.ToString() ?? "",
                    TotalOrders = Convert.ToInt32(x.total_orders ?? 0),
                    ReturnCount = Convert.ToInt32(x.return_count ?? 0),
                    ReturnFrequency = Convert.ToDouble(x.return_frequency ?? 0),
                    LastOrderDate = x.last_order_date != null ? Convert.ToDateTime(x.last_order_date) : DateTime.MinValue
                }).ToList();

                return Result<List<CustomerReturnData>>.Ok(result, $"Lấy {result.Count} khách hàng có tần suất quay trở lại");
            }
            catch (Exception ex)
            {
                return HandleException<List<CustomerReturnData>>(ex, "lấy tần suất khách hàng quay trở lại");
            }
        }

        public async Task<Result<PackagingRecoveryReportDTO>> GetPackagingRecoveryReportAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var report = new PackagingRecoveryReportDTO();

                // Lấy tổng số bao bì đã phát hành và thu hồi
                var totalStats = await _unitOfWork.Reports.GetTotalPackagingStatsAsync(fromDate, toDate);

                if (totalStats != null)
                {
                    report.TotalIssued = Convert.ToInt32(totalStats.total_issued ?? 0);
                    report.TotalReturned = Convert.ToInt32(totalStats.total_returned ?? 0);

                    if (report.TotalIssued > 0)
                    {
                        report.RecoveryRate = (double)report.TotalReturned / report.TotalIssued * 100;
                    }
                }

                // Lấy chi tiết theo từng loại bao bì
                var packagingData = await GetPackagingRecoveryRateAsync(fromDate, toDate);
                if (packagingData.Success)
                {
                    report.PackagingData = packagingData.Data.ToList();
                }

                return Result<PackagingRecoveryReportDTO>.Ok(report, "Lấy báo cáo tỷ lệ thu hồi bao bì thành công");
            }
            catch (Exception ex)
            {
                return HandleException<PackagingRecoveryReportDTO>(ex, "lấy báo cáo tỷ lệ thu hồi bao bì");
            }
        }

        public async Task<Result<List<PackagingRecoveryData>>> GetPackagingRecoveryRateAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _unitOfWork.Reports.GetPackagingRecoveryRateAsync(fromDate, toDate);

                var result = data.Select(x => new PackagingRecoveryData
                {
                    PackagingId = Convert.ToInt32(x.packaging_id ?? 0),
                    PackagingName = x.packaging_name?.ToString() ?? "",
                    Issued = Convert.ToInt32(x.issued ?? 0),
                    Returned = Convert.ToInt32(x.returned ?? 0),
                    RecoveryRate = Convert.ToDouble(x.recovery_rate ?? 0)
                }).ToList();

                return Result<List<PackagingRecoveryData>>.Ok(result, $"Lấy {result.Count} loại bao bì");
            }
            catch (Exception ex)
            {
                return HandleException<List<PackagingRecoveryData>>(ex, "lấy tỷ lệ thu hồi bao bì");
            }
        }

        public async Task<Result<EnvironmentalImpactReportDTO>> GetEnvironmentalImpactReportAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var report = new EnvironmentalImpactReportDTO();

                // Lấy tổng tác động môi trường
                var totalImpact = await _unitOfWork.Reports.GetTotalEnvironmentalImpactAsync(fromDate, toDate);

                if (totalImpact != null)
                {
                    report.TotalRefills = Convert.ToInt32(totalImpact.total_refills ?? 0);
                    report.PlasticSavedKg = Convert.ToDecimal(totalImpact.plastic_saved_kg ?? 0);
                    report.PlasticSavedTons = Convert.ToDecimal(totalImpact.plastic_saved_tons ?? 0);
                    report.CO2SavedKg = Convert.ToDecimal(totalImpact.co2_saved_kg ?? 0);
                    report.CO2SavedTons = Convert.ToDecimal(totalImpact.co2_saved_tons ?? 0);
                }

                // Lấy dữ liệu theo ngày
                var dailyData = await _unitOfWork.Reports.GetEnvironmentalImpactAsync(fromDate, toDate);

                report.DataPoints = dailyData.Select(x => new EnvironmentalImpactDataPoint
                {
                    Period = x.period?.ToString() ?? "",
                    Date = Convert.ToDateTime(x.period_date),
                    Refills = Convert.ToInt32(x.refills ?? 0),
                    PlasticSavedKg = Convert.ToDecimal(x.plastic_saved_kg ?? 0),
                    CO2SavedKg = Convert.ToDecimal(x.co2_saved_kg ?? 0)
                }).ToList();

                return Result<EnvironmentalImpactReportDTO>.Ok(report, "Lấy báo cáo tác động môi trường thành công");
            }
            catch (Exception ex)
            {
                return HandleException<EnvironmentalImpactReportDTO>(ex, "lấy báo cáo tác động môi trường");
            }
        }
    }
}

