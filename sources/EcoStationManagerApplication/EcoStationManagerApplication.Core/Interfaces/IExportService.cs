using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    /// <summary>
    /// Interface cho Export Service - Tầng BLL/Application
    /// Chịu trách nhiệm chuẩn bị dữ liệu để export
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Lấy dữ liệu đơn hàng để export Excel/PDF
        /// </summary>
        /// <param name="filterTag">Filter tag (all, online, offline, new, ready, shipping, completed)</param>
        /// <returns>Danh sách OrderDTO đã được chuẩn bị sẵn</returns>
        Task<List<OrderExportDTO>> GetOrdersForExportAsync(string filterTag = "all");

        /// <summary>
        /// Lấy dữ liệu đơn hàng theo khoảng thời gian
        /// </summary>
        Task<List<OrderExportDTO>> GetOrdersForExportAsync(DateTime? fromDate, DateTime? toDate, OrderStatus? status = null);
    }

    /// <summary>
    /// DTO chuyên dụng cho export - chỉ chứa các field cần thiết
    /// </summary>
    public class OrderExportDTO
    {
        public int STT { get; set; }
        public string MaDon { get; set; }
        public string KhachHang { get; set; }
        public string Nguon { get; set; }
        public string TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public string ThanhToan { get; set; }
        public DateTime NgayTao { get; set; }
    }
}

