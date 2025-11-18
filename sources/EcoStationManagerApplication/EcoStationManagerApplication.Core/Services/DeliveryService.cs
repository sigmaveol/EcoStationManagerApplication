using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class DeliveryService : BaseService, IDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeliveryService(IUnitOfWork unitOfWork)
            : base("DeliveryService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetAllAsync()
        {
            try
            {
                var deliveries = await _unitOfWork.Deliveries.GetAllAsync();
                if (!deliveries.Any())
                {
                    return NotFoundError<IEnumerable<DeliveryAssignment>>("phân công giao hàng");
                }

                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công giao hàng");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy danh sách phân công giao hàng");
            }
        }

        public async Task<Result<DeliveryAssignment>> GetByIdAsync(int assignmentId)
        {
            try
            {
                if (assignmentId <= 0)
                {
                    return ValidationError<DeliveryAssignment>(new List<string> { "ID phân công không hợp lệ" });
                }

                var assignment = await _unitOfWork.Deliveries.GetByIdAsync(assignmentId);
                if (assignment == null)
                {
                    return NotFoundError<DeliveryAssignment>("phân công giao hàng", assignmentId);
                }

                return Result<DeliveryAssignment>.Ok(assignment, "Đã tải thông tin phân công giao hàng");
            }
            catch (Exception ex)
            {
                return HandleException<DeliveryAssignment>(ex, "lấy thông tin phân công giao hàng");
            }
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetByDriverAsync(int driverId)
        {
            try
            {
                if (driverId <= 0)
                {
                    return ValidationError<IEnumerable<DeliveryAssignment>>(new List<string> { "ID tài xế không hợp lệ" });
                }

                var deliveries = await _unitOfWork.Deliveries.GetByDriverAsync(driverId);
                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công giao hàng");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy phân công giao hàng theo tài xế");
            }
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetByStatusAsync(DeliveryStatus status)
        {
            try
            {
                var deliveries = await _unitOfWork.Deliveries.GetByStatusAsync(status);
                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công giao hàng");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy phân công giao hàng theo trạng thái");
            }
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return ValidationError<IEnumerable<DeliveryAssignment>>(new List<string> { "Ngày bắt đầu không được lớn hơn ngày kết thúc" });
                }

                var deliveries = await _unitOfWork.Deliveries.GetByDateRangeAsync(startDate, endDate);
                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công giao hàng");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy phân công giao hàng theo khoảng thời gian");
            }
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetPendingDeliveriesAsync()
        {
            try
            {
                var deliveries = await _unitOfWork.Deliveries.GetPendingDeliveriesAsync();
                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công đang chờ");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy phân công đang chờ");
            }
        }

        public async Task<Result<IEnumerable<DeliveryAssignment>>> GetCompletedDeliveriesAsync()
        {
            try
            {
                var deliveries = await _unitOfWork.Deliveries.GetCompletedDeliveriesAsync();
                return Result<IEnumerable<DeliveryAssignment>>.Ok(deliveries, $"Đã tải {deliveries.Count()} phân công đã hoàn thành");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<DeliveryAssignment>>(ex, "lấy phân công đã hoàn thành");
            }
        }

        public async Task<Result<int>> CreateAsync(DeliveryAssignment assignment)
        {
            try
            {
                var errors = new List<string>();

                if (assignment.OrderId <= 0)
                    errors.Add("ID đơn hàng không hợp lệ");

                if (assignment.DriverId <= 0)
                    errors.Add("ID tài xế không hợp lệ");

                if (errors.Any())
                {
                    return ValidationError<int>(errors);
                }

                var assignmentId = await _unitOfWork.Deliveries.AddAsync(assignment);
                _logger.Info($"Đã tạo phân công giao hàng mới - AssignmentId: {assignmentId}, OrderId: {assignment.OrderId}, DriverId: {assignment.DriverId}");

                return Result<int>.Ok(assignmentId, "Đã tạo phân công giao hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo phân công giao hàng");
            }
        }

        public async Task<Result> UpdateAsync(DeliveryAssignment assignment)
        {
            try
            {
                if (assignment.AssignmentId <= 0)
                {
                    return ValidationError(new List<string> { "ID phân công không hợp lệ" });
                }

                var existing = await _unitOfWork.Deliveries.GetByIdAsync(assignment.AssignmentId);
                if (existing == null)
                {
                    return NotFoundError("phân công giao hàng", assignment.AssignmentId);
                }

                var updated = await _unitOfWork.Deliveries.UpdateAsync(assignment);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật phân công giao hàng");
                }

                _logger.Info($"Đã cập nhật phân công giao hàng - AssignmentId: {assignment.AssignmentId}");
                return Result.Ok("Đã cập nhật phân công giao hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật phân công giao hàng");
            }
        }

        public async Task<Result> UpdateStatusAsync(int assignmentId, DeliveryStatus status)
        {
            try
            {
                if (assignmentId <= 0)
                {
                    return ValidationError(new List<string> { "ID phân công không hợp lệ" });
                }

                var updated = await _unitOfWork.Deliveries.UpdateStatusAsync(assignmentId, status);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật trạng thái phân công");
                }

                _logger.Info($"Đã cập nhật trạng thái phân công - AssignmentId: {assignmentId}, Status: {status}");
                return Result.Ok("Đã cập nhật trạng thái thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật trạng thái phân công");
            }
        }

        public async Task<Result> UpdatePaymentStatusAsync(int assignmentId, DeliveryPaymentStatus paymentStatus)
        {
            try
            {
                if (assignmentId <= 0)
                {
                    return ValidationError(new List<string> { "ID phân công không hợp lệ" });
                }

                var updated = await _unitOfWork.Deliveries.UpdatePaymentStatusAsync(assignmentId, paymentStatus);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật trạng thái thanh toán");
                }

                _logger.Info($"Đã cập nhật trạng thái thanh toán - AssignmentId: {assignmentId}, PaymentStatus: {paymentStatus}");
                return Result.Ok("Đã cập nhật trạng thái thanh toán thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật trạng thái thanh toán");
            }
        }

        public async Task<Result> DeleteAsync(int assignmentId)
        {
            try
            {
                if (assignmentId <= 0)
                {
                    return ValidationError(new List<string> { "ID phân công không hợp lệ" });
                }

                var existing = await _unitOfWork.Deliveries.GetByIdAsync(assignmentId);
                if (existing == null)
                {
                    return NotFoundError("phân công giao hàng", assignmentId);
                }

                var deleted = await _unitOfWork.Deliveries.DeleteAsync(assignmentId);
                if (!deleted)
                {
                    return BusinessError("Không thể xóa phân công giao hàng");
                }

                _logger.Info($"Đã xóa phân công giao hàng - AssignmentId: {assignmentId}");
                return Result.Ok("Đã xóa phân công giao hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "xóa phân công giao hàng");
            }
        }

        public async Task<Result<decimal>> GetTotalCODByDriverAsync(int driverId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (driverId <= 0)
                {
                    return ValidationError<decimal>(new List<string> { "ID tài xế không hợp lệ" });
                }

                var total = await _unitOfWork.Deliveries.GetTotalCODByDriverAsync(driverId, startDate, endDate);
                return Result<decimal>.Ok(total, $"Tổng COD: {total:N0} VNĐ");
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "tính tổng COD theo tài xế");
            }
        }
    }
}

