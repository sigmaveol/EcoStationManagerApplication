using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    /// <summary>
    /// Service xử lý export dữ liệu - Tầng BLL/Application
    /// Chịu trách nhiệm chuẩn bị dữ liệu để export
    /// </summary>
    public class ExportService : BaseService, IExportService
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public ExportService(IOrderService orderService, ICustomerService customerService)
            : base("ExportService")
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        public async Task<List<OrderExportDTO>> GetOrdersForExportAsync(string filterTag = "all")
        {
            try
            {
                // Lấy danh sách đơn hàng
                var ordersResult = await _orderService.GetProcessingOrdersAsync();
                if (!ordersResult.Success || ordersResult.Data == null)
                {
                    return new List<OrderExportDTO>();
                }

                var orders = ordersResult.Data.ToList();

                // Áp dụng filter
                if (filterTag != "all")
                {
                    orders = orders.Where(order =>
                    {
                        switch (filterTag)
                        {
                            case "online":
                                return order.Source == OrderSource.EXCEL ||
                                       order.Source == OrderSource.EMAIL ||
                                       order.Source == OrderSource.GOOGLEFORM;
                            case "offline":
                                return order.Source == OrderSource.MANUAL;
                            case "new":
                                return order.Status == OrderStatus.CONFIRMED;
                            case "ready":
                                return order.Status == OrderStatus.READY;
                            case "shipping":
                                return order.Status == OrderStatus.SHIPPED;
                            case "completed":
                                return order.Status == OrderStatus.COMPLETED;
                            default:
                                return true;
                        }
                    }).ToList();
                }

                // Chuyển đổi sang OrderExportDTO
                var exportData = new List<OrderExportDTO>();
                int stt = 1;

                foreach (var order in orders)
                {
                    string customerName = "Khách lẻ";
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await _customerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name;
                        }
                    }

                    exportData.Add(new OrderExportDTO
                    {
                        STT = stt,
                        MaDon = order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                        KhachHang = customerName,
                        Nguon = GetOrderSourceDisplay(order.Source),
                        TrangThai = GetOrderStatusDisplay(order.Status),
                        TongTien = order.TotalAmount,
                        ThanhToan = order.PaymentStatus == PaymentStatus.PAID ? "Đã thanh toán" : "Chưa thanh toán",
                        NgayTao = order.LastUpdated
                    });

                    stt++;
                }

                return exportData;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOrdersForExportAsync error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<OrderExportDTO>> GetOrdersForExportAsync(DateTime? fromDate, DateTime? toDate, OrderStatus? status = null)
        {
            try
            {
                // Lấy tất cả đơn hàng
                var ordersResult = await _orderService.GetAllAsync();
                if (!ordersResult.Success || ordersResult.Data == null)
                {
                    return new List<OrderExportDTO>();
                }

                var orders = ordersResult.Data.ToList();

                // Lọc theo thời gian
                if (fromDate.HasValue)
                {
                    orders = orders.Where(o => o.LastUpdated >= fromDate.Value).ToList();
                }

                if (toDate.HasValue)
                {
                    orders = orders.Where(o => o.LastUpdated <= toDate.Value.AddDays(1)).ToList();
                }

                // Lọc theo trạng thái
                if (status.HasValue)
                {
                    orders = orders.Where(o => o.Status == status.Value).ToList();
                }

                // Chuyển đổi sang OrderExportDTO
                var exportData = new List<OrderExportDTO>();
                int stt = 1;

                foreach (var order in orders)
                {
                    string customerName = "Khách lẻ";
                    if (order.CustomerId.HasValue)
                    {
                        var customerResult = await _customerService.GetCustomerByIdAsync(order.CustomerId.Value);
                        if (customerResult.Success && customerResult.Data != null)
                        {
                            customerName = customerResult.Data.Name;
                        }
                    }

                    exportData.Add(new OrderExportDTO
                    {
                        STT = stt,
                        MaDon = order.OrderCode ?? $"ORD-{order.OrderId:D5}",
                        KhachHang = customerName,
                        Nguon = GetOrderSourceDisplay(order.Source),
                        TrangThai = GetOrderStatusDisplay(order.Status),
                        TongTien = order.TotalAmount,
                        ThanhToan = order.PaymentStatus == PaymentStatus.PAID ? "Đã thanh toán" : "Chưa thanh toán",
                        NgayTao = order.LastUpdated
                    });

                    stt++;
                }

                return exportData;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetOrdersForExportAsync error: {ex.Message}");
                throw;
            }
        }

        private string GetOrderSourceDisplay(OrderSource source)
        {
            switch (source)
            {
                case OrderSource.GOOGLEFORM:
                case OrderSource.EMAIL:
                    return "Email";
                case OrderSource.MANUAL:
                    return "Offline";
                case OrderSource.EXCEL:
                    return "Excel";
                default:
                    return "Khác";
            }
        }

        private string GetOrderStatusDisplay(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.DRAFT:
                    return "Nháp";
                case OrderStatus.CONFIRMED:
                    return "Mới";
                case OrderStatus.PROCESSING:
                    return "Đang xử lý";
                case OrderStatus.READY:
                    return "Chuẩn bị";
                case OrderStatus.SHIPPED:
                    return "Đang giao";
                case OrderStatus.COMPLETED:
                    return "Hoàn thành";
                case OrderStatus.CANCELLED:
                    return "Đã hủy";
                default:
                    return "Không xác định";
            }
        }
    }
}

