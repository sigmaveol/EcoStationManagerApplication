using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IDeliveryService
    {
        /// <summary>
        /// Lấy tất cả phân công giao hàng
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetAllAsync();

        /// <summary>
        /// Lấy phân công giao hàng theo ID
        /// </summary>
        Task<Result<DeliveryAssignment>> GetByIdAsync(int assignmentId);

        /// <summary>
        /// Lấy phân công giao hàng theo driver
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetByDriverAsync(int driverId);

        /// <summary>
        /// Lấy phân công giao hàng theo trạng thái
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetByStatusAsync(DeliveryStatus status);

        /// <summary>
        /// Lấy phân công giao hàng theo khoảng thời gian
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy các phân công đang chờ
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetPendingDeliveriesAsync();

        /// <summary>
        /// Lấy các phân công đã hoàn thành
        /// </summary>
        Task<Result<IEnumerable<DeliveryAssignment>>> GetCompletedDeliveriesAsync();

        /// <summary>
        /// Tạo phân công giao hàng mới
        /// </summary>
        Task<Result<int>> CreateAsync(DeliveryAssignment assignment);

        /// <summary>
        /// Cập nhật phân công giao hàng
        /// </summary>
        Task<Result> UpdateAsync(DeliveryAssignment assignment);

        /// <summary>
        /// Cập nhật trạng thái phân công
        /// </summary>
        Task<Result> UpdateStatusAsync(int assignmentId, DeliveryStatus status);

        /// <summary>
        /// Cập nhật trạng thái thanh toán COD
        /// </summary>
        Task<Result> UpdatePaymentStatusAsync(int assignmentId, DeliveryPaymentStatus paymentStatus);

        /// <summary>
        /// Xóa phân công giao hàng
        /// </summary>
        Task<Result> DeleteAsync(int assignmentId);

        /// <summary>
        /// Tính tổng COD theo driver
        /// </summary>
        Task<Result<decimal>> GetTotalCODByDriverAsync(int driverId, DateTime? startDate = null, DateTime? endDate = null);
    }
}

