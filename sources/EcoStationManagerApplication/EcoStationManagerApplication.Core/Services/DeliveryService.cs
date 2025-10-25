using EcostationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    // ==================== TRANSFER SERVICE IMPLEMENTATION ====================
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ITransferDetailRepository _transferDetailRepository;
        private readonly IStockOutRepository _stockOutRepository;
        private readonly IStockInRepository _stockInRepository;

        public TransferService(
            ITransferRepository transferRepository,
            ITransferDetailRepository transferDetailRepository,
            IStockOutRepository stockOutRepository,
            IStockInRepository stockInRepository)
        {
            _transferRepository = transferRepository;
            _transferDetailRepository = transferDetailRepository;
            _stockOutRepository = stockOutRepository;
            _stockInRepository = stockInRepository;
        }

        public async Task<Transfer> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            var transfer = await _transferRepository.GetByIdAsync(id);
            return transfer ?? throw new KeyNotFoundException($"Không tìm thấy chuyển kho với ID: {id}");
        }

        public async Task<IEnumerable<Transfer>> GetAllAsync()
        {
            return await _transferRepository.GetAllAsync();
        }

        public async Task<Transfer> CreateAsync(Transfer entity)
        {
            ValidateTransfer(entity);
            return await _transferRepository.CreateAsync(entity);
        }

        public async Task<Transfer> UpdateAsync(Transfer entity)
        {
            if (entity.TransferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            ValidateTransfer(entity);

            var existing = await _transferRepository.GetByIdAsync(entity.TransferId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy chuyển kho với ID: {entity.TransferId}");

            return await _transferRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");

            // Check if transfer has details
            var details = await _transferDetailRepository.GetDetailsByTransferAsync(id);
            if (details.Any())
                throw new InvalidOperationException("Không thể xóa chuyển kho đã có chi tiết");

            return await _transferRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Chuyển kho không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByStationAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _transferRepository.GetTransfersByStationAsync(stationId);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được trống");
            return await _transferRepository.GetTransfersByStatusAsync(status);
        }

        public async Task<IEnumerable<Transfer>> GetTransfersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _transferRepository.GetTransfersByDateRangeAsync(startDate, endDate);
        }

        public async Task<Transfer> GetTransferWithDetailsAsync(int transferId)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            return await _transferRepository.GetTransferWithDetailsAsync(transferId);
        }

        public async Task<bool> CreateTransferWithDetailsAsync(Transfer transfer, List<TransferDetail> details)
        {
            ValidateTransfer(transfer);

            if (details == null || !details.Any())
                throw new ArgumentException("Chi tiết chuyển kho không được trống");

            // Create transfer
            var createdTransfer = await _transferRepository.CreateAsync(transfer);

            // Add details
            foreach (var detail in details)
            {
                detail.TransferId = createdTransfer.TransferId;
                await _transferDetailRepository.CreateAsync(detail);
            }

            return true;
        }

        public async Task<bool> UpdateTransferStatusAsync(int transferId, string status)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được trống");

            var validStatuses = new[] { "pending", "in_transit", "completed", "cancelled", "other" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException($"Trạng thái không hợp lệ: {status}");

            return await _transferRepository.UpdateTransferStatusAsync(transferId, status);
        }

        public async Task<bool> ProcessTransferAsync(int transferId, int processedBy)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            if (processedBy <= 0) throw new ArgumentException("ID người xử lý không hợp lệ");

            var transfer = await _transferRepository.GetByIdAsync(transferId);
            if (transfer == null) throw new KeyNotFoundException($"Không tìm thấy chuyển kho với ID: {transferId}");

            var details = await _transferDetailRepository.GetDetailsByTransferAsync(transferId);
            if (!details.Any()) throw new InvalidOperationException("Chuyển kho không có chi tiết");

            // Create stock out from source station
            foreach (var detail in details)
            {
                var stockOut = new StockOut
                {
                    VariantId = detail.VariantId,
                    StationId = transfer.FromStationId,
                    Quantity = detail.Quantity,
                    Purpose = "transfer",
                    Reason = $"Chuyển kho đến trạm {transfer.ToStationId}",
                    CreatedBy = processedBy,
                    BatchId = 1 // Default batch, should be handled properly
                };
                await _stockOutRepository.CreateAsync(stockOut);
            }

            // Create stock in to destination station
            foreach (var detail in details)
            {
                var stockIn = new StockIn
                {
                    VariantId = detail.VariantId,
                    StationId = transfer.ToStationId,
                    Quantity = detail.Quantity,
                    SourceType = "transfer",
                    CreatedBy = processedBy,
                    BatchId = 1 // Default batch
                };
                await _stockInRepository.CreateAsync(stockIn);
            }

            return await _transferRepository.UpdateTransferStatusAsync(transferId, "completed");
        }

        public async Task<bool> CancelTransferAsync(int transferId, string reason)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Lý do hủy không được trống");

            return await _transferRepository.UpdateTransferStatusAsync(transferId, "cancelled");
        }

        public async Task<Dictionary<string, object>> GetTransferStatisticsAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");

            var transfers = await _transferRepository.GetTransfersByStationAsync(stationId);
            var pendingTransfers = transfers.Where(t => t.Status == "pending");
            var completedTransfers = transfers.Where(t => t.Status == "completed");
            var inTransitTransfers = transfers.Where(t => t.Status == "in_transit");

            return new Dictionary<string, object>
            {
                ["TotalTransfers"] = transfers.Count(),
                ["PendingTransfers"] = pendingTransfers.Count(),
                ["CompletedTransfers"] = completedTransfers.Count(),
                ["InTransitTransfers"] = inTransitTransfers.Count(),
                ["OutgoingTransfers"] = transfers.Count(t => t.FromStationId == stationId),
                ["IncomingTransfers"] = transfers.Count(t => t.ToStationId == stationId),
                ["StationId"] = stationId
            };
        }

        private void ValidateTransfer(Transfer transfer)
        {
            if (transfer == null) throw new ArgumentNullException(nameof(transfer));
            if (transfer.FromStationId <= 0) throw new ArgumentException("ID trạm nguồn không hợp lệ");
            if (transfer.ToStationId <= 0) throw new ArgumentException("ID trạm đích không hợp lệ");
            if (transfer.FromStationId == transfer.ToStationId) throw new ArgumentException("Trạm nguồn và trạm đích không được trùng nhau");
            if (string.IsNullOrWhiteSpace(transfer.Code)) throw new ArgumentException("Mã chuyển kho không được trống");
        }
    }

    // ==================== TRANSFER DETAIL SERVICE IMPLEMENTATION ====================
    public class TransferDetailService : ITransferDetailService
    {
        private readonly ITransferDetailRepository _transferDetailRepository;
        private readonly IVariantRepository _variantRepository;

        public TransferDetailService(ITransferDetailRepository transferDetailRepository, IVariantRepository variantRepository)
        {
            _transferDetailRepository = transferDetailRepository;
            _variantRepository = variantRepository;
        }

        public async Task<TransferDetail> GetByIdAsync(int id)
        {
            throw new NotSupportedException("TransferDetail không hỗ trợ GetByIdAsync do composite key");
        }

        public async Task<IEnumerable<TransferDetail>> GetAllAsync()
        {
            throw new NotSupportedException("TransferDetail không hỗ trợ GetAllAsync");
        }

        public async Task<TransferDetail> CreateAsync(TransferDetail entity)
        {
            ValidateTransferDetail(entity);
            return await _transferDetailRepository.CreateAsync(entity);
        }

        public async Task<TransferDetail> UpdateAsync(TransferDetail entity)
        {
            throw new NotSupportedException("TransferDetail không hỗ trợ UpdateAsync do composite key");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotSupportedException("TransferDetail không hỗ trợ DeleteAsync do composite key");
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("TransferDetail không hỗ trợ SoftDeleteAsync");
        }

        public async Task<IEnumerable<TransferDetail>> GetDetailsByTransferAsync(int transferId)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            return await _transferDetailRepository.GetDetailsByTransferAsync(transferId);
        }

        public async Task<bool> DeleteDetailsByTransferAsync(int transferId)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            return await _transferDetailRepository.DeleteDetailsByTransferAsync(transferId);
        }

        public async Task<bool> AddDetailsToTransferAsync(int transferId, List<TransferDetail> details)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            if (details == null || !details.Any()) throw new ArgumentException("Danh sách chi tiết không được trống");

            foreach (var detail in details)
            {
                detail.TransferId = transferId;
                ValidateTransferDetail(detail);
                await _transferDetailRepository.CreateAsync(detail);
            }

            return true;
        }

        public async Task<decimal> GetTransferTotalQuantityAsync(int transferId)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            return await _transferDetailRepository.GetTransferTotalQuantityAsync(transferId);
        }

        public async Task<bool> ValidateTransferDetailsAsync(int transferId)
        {
            if (transferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            return await _transferDetailRepository.ValidateTransferDetailsAsync(transferId);
        }

        private void ValidateTransferDetail(TransferDetail detail)
        {
            if (detail == null) throw new ArgumentNullException(nameof(detail));
            if (detail.TransferId <= 0) throw new ArgumentException("ID chuyển kho không hợp lệ");
            if (detail.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (detail.Quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
        }
    }

    // ==================== DELIVERY ASSIGNMENT SERVICE IMPLEMENTATION ====================
    public class DeliveryAssignmentService : IDeliveryAssignmentService
    {
        private readonly IDeliveryAssignmentRepository _deliveryAssignmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public DeliveryAssignmentService(
            IDeliveryAssignmentRepository deliveryAssignmentRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository)
        {
            _deliveryAssignmentRepository = deliveryAssignmentRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<DeliveryAssignment> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            var assignment = await _deliveryAssignmentRepository.GetByIdAsync(id);
            return assignment ?? throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {id}");
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAllAsync()
        {
            return await _deliveryAssignmentRepository.GetAllAsync();
        }

        public async Task<DeliveryAssignment> CreateAsync(DeliveryAssignment entity)
        {
            ValidateDeliveryAssignment(entity);
            return await _deliveryAssignmentRepository.CreateAsync(entity);
        }

        public async Task<DeliveryAssignment> UpdateAsync(DeliveryAssignment entity)
        {
            if (entity.AssignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            ValidateDeliveryAssignment(entity);

            var existing = await _deliveryAssignmentRepository.GetByIdAsync(entity.AssignmentId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {entity.AssignmentId}");

            return await _deliveryAssignmentRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            return await _deliveryAssignmentRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Phân công giao hàng không hỗ trợ xóa mềm");
        }

        public async Task<DeliveryAssignment> GetAssignmentByOrderAsync(int orderId)
        {
            if (orderId <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ");
            return await _deliveryAssignmentRepository.GetAssignmentByOrderAsync(orderId);
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDriverAsync(int driverId)
        {
            if (driverId <= 0) throw new ArgumentException("ID tài xế không hợp lệ");
            return await _deliveryAssignmentRepository.GetAssignmentsByDriverAsync(driverId);
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được trống");
            return await _deliveryAssignmentRepository.GetAssignmentsByStatusAsync(status);
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _deliveryAssignmentRepository.GetAssignmentsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetTodayAssignmentsAsync(int driverId)
        {
            if (driverId <= 0) throw new ArgumentException("ID tài xế không hợp lệ");

            var today = DateTime.Today;
            var assignments = await _deliveryAssignmentRepository.GetAssignmentsByDriverAsync(driverId);
            return assignments.Where(a => a.AssignedDate.Date == today);
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int assignmentId, string status)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Trạng thái không được trống");

            var validStatuses = new[] { "pending", "intransit", "delivered", "failed" };
            if (!validStatuses.Contains(status))
                throw new ArgumentException($"Trạng thái không hợp lệ: {status}");

            return await _deliveryAssignmentRepository.UpdateDeliveryStatusAsync(assignmentId, status);
        }

        public async Task<bool> AssignDriverToOrderAsync(int orderId, int driverId)
        {
            if (orderId <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ");
            if (driverId <= 0) throw new ArgumentException("ID tài xế không hợp lệ");

            // Check if order exists
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId}");

            // Check if driver exists and is active
            var driver = await _userRepository.GetByIdAsync(driverId);
            if (driver == null) throw new KeyNotFoundException($"Không tìm thấy tài xế với ID: {driverId}");
            if (!driver.IsActive) throw new InvalidOperationException("Tài xế không hoạt động");

            // Check if order already has assignment
            var existingAssignment = await _deliveryAssignmentRepository.GetAssignmentByOrderAsync(orderId);
            if (existingAssignment != null)
                throw new InvalidOperationException("Đơn hàng đã được phân công cho tài xế khác");

            var assignment = new DeliveryAssignment
            {
                OrderId = orderId,
                DriverId = driverId,
                Status = "pending",
                AssignedDate = DateTime.Now
            };

            await _deliveryAssignmentRepository.CreateAsync(assignment);
            return true;
        }

        public async Task<bool> CompleteDeliveryAsync(int assignmentId, string notes)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");

            var assignment = await _deliveryAssignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null) throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {assignmentId}");

            // Update order status to completed
            var order = await _orderRepository.GetByIdAsync(assignment.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Completed;
                await _orderRepository.UpdateAsync(order);
            }

            assignment.Status = "delivered";
            assignment.Notes = notes;
            await _deliveryAssignmentRepository.UpdateAsync(assignment);

            return true;
        }

        public async Task<bool> CancelDeliveryAsync(int assignmentId, string reason)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Lý do hủy không được trống");

            return await _deliveryAssignmentRepository.UpdateDeliveryStatusAsync(assignmentId, "failed");
        }

        public async Task<Dictionary<string, object>> GetDeliveryPerformanceAsync(int driverId)
        {
            if (driverId <= 0) throw new ArgumentException("ID tài xế không hợp lệ");

            var assignments = await _deliveryAssignmentRepository.GetAssignmentsByDriverAsync(driverId);
            var delivered = assignments.Count(a => a.Status == "delivered");
            var pending = assignments.Count(a => a.Status == "pending");
            var inTransit = assignments.Count(a => a.Status == "intransit");
            var failed = assignments.Count(a => a.Status == "failed");

            var successRate = assignments.Any() ? (decimal)delivered / assignments.Count() * 100 : 0;

            return new Dictionary<string, object>
            {
                ["TotalAssignments"] = assignments.Count(),
                ["Delivered"] = delivered,
                ["Pending"] = pending,
                ["InTransit"] = inTransit,
                ["Failed"] = failed,
                ["SuccessRate"] = Math.Round(successRate, 2),
                ["DriverId"] = driverId
            };
        }

        private void ValidateDeliveryAssignment(DeliveryAssignment assignment)
        {
            if (assignment == null) throw new ArgumentNullException(nameof(assignment));
            if (assignment.OrderId <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ");
            if (assignment.DriverId <= 0) throw new ArgumentException("ID tài xế không hợp lệ");
            if (string.IsNullOrWhiteSpace(assignment.Status)) throw new ArgumentException("Trạng thái không được trống");
        }
    }

    // ==================== DELIVERY ROUTE SERVICE IMPLEMENTATION ====================
    public class DeliveryRouteService : IDeliveryRouteService
    {
        private readonly IDeliveryRouteRepository _deliveryRouteRepository;
        private readonly IDeliveryAssignmentRepository _deliveryAssignmentRepository;

        public DeliveryRouteService(
            IDeliveryRouteRepository deliveryRouteRepository,
            IDeliveryAssignmentRepository deliveryAssignmentRepository)
        {
            _deliveryRouteRepository = deliveryRouteRepository;
            _deliveryAssignmentRepository = deliveryAssignmentRepository;
        }

        public async Task<DeliveryRoute> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID tuyến đường không hợp lệ");
            var route = await _deliveryRouteRepository.GetByIdAsync(id);
            return route ?? throw new KeyNotFoundException($"Không tìm thấy tuyến đường với ID: {id}");
        }

        public async Task<IEnumerable<DeliveryRoute>> GetAllAsync()
        {
            return await _deliveryRouteRepository.GetAllAsync();
        }

        public async Task<DeliveryRoute> CreateAsync(DeliveryRoute entity)
        {
            ValidateDeliveryRoute(entity);
            return await _deliveryRouteRepository.CreateAsync(entity);
        }

        public async Task<DeliveryRoute> UpdateAsync(DeliveryRoute entity)
        {
            if (entity.RouteId <= 0) throw new ArgumentException("ID tuyến đường không hợp lệ");
            ValidateDeliveryRoute(entity);

            var existing = await _deliveryRouteRepository.GetByIdAsync(entity.RouteId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy tuyến đường với ID: {entity.RouteId}");

            return await _deliveryRouteRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID tuyến đường không hợp lệ");
            return await _deliveryRouteRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("Tuyến đường không hỗ trợ xóa mềm");
        }

        public async Task<IEnumerable<DeliveryRoute>> GetRoutesByAssignmentAsync(int assignmentId)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            return await _deliveryRouteRepository.GetRoutesByAssignmentAsync(assignmentId);
        }

        public async Task<IEnumerable<DeliveryRoute>> GetRoutesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Ngày bắt đầu phải trước ngày kết thúc");
            return await _deliveryRouteRepository.GetRoutesByDateRangeAsync(startDate, endDate);
        }

        public async Task<bool> AddRoutePointAsync(int assignmentId, decimal latitude, decimal longitude, DateTime recordedTime)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");

            // Validate coordinates
            if (!IsValidLatitude(latitude)) throw new ArgumentException("Vĩ độ không hợp lệ");
            if (!IsValidLongitude(longitude)) throw new ArgumentException("Kinh độ không hợp lệ");

            // Check if assignment exists
            var assignment = await _deliveryAssignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null) throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {assignmentId}");

            var routePoint = new DeliveryRoute
            {
                AssignmentId = assignmentId,
                Latitude = latitude,
                Longitude = longitude,
                RecordedTime = recordedTime,
                Source = "app"
            };

            await _deliveryRouteRepository.CreateAsync(routePoint);
            return true;
        }

        public async Task<bool> ImportRouteFromExcelAsync(int assignmentId, string filePath)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Đường dẫn file không được trống");

            // Check if assignment exists
            var assignment = await _deliveryAssignmentRepository.GetByIdAsync(assignmentId);
            if (assignment == null) throw new KeyNotFoundException($"Không tìm thấy phân công với ID: {assignmentId}");

            // In a real implementation, this would parse Excel file
            // For now, we'll simulate with some dummy data
            var simulatedRoutePoints = new List<DeliveryRoute>
            {
                new DeliveryRoute { AssignmentId = assignmentId, Latitude = 10.762622m, Longitude = 106.660172m, RecordedTime = DateTime.Now.AddMinutes(-30), Source = "manualexcel" },
                new DeliveryRoute { AssignmentId = assignmentId, Latitude = 10.763456m, Longitude = 106.661234m, RecordedTime = DateTime.Now.AddMinutes(-25), Source = "manualexcel" },
                new DeliveryRoute { AssignmentId = assignmentId, Latitude = 10.764789m, Longitude = 106.662345m, RecordedTime = DateTime.Now.AddMinutes(-20), Source = "manualexcel" },
                new DeliveryRoute { AssignmentId = assignmentId, Latitude = 10.765123m, Longitude = 106.663456m, RecordedTime = DateTime.Now.AddMinutes(-15), Source = "manualexcel" },
                new DeliveryRoute { AssignmentId = assignmentId, Latitude = 10.766456m, Longitude = 106.664567m, RecordedTime = DateTime.Now.AddMinutes(-10), Source = "manualexcel" }
            };

            foreach (var point in simulatedRoutePoints)
            {
                await _deliveryRouteRepository.CreateAsync(point);
            }

            return true;
        }

        public async Task<IEnumerable<DeliveryRoute>> GetRouteByAssignmentAndDateAsync(int assignmentId, DateTime date)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");

            var routes = await _deliveryRouteRepository.GetRoutesByAssignmentAsync(assignmentId);
            return routes.Where(r => r.RecordedTime.Date == date.Date)
                        .OrderBy(r => r.RecordedTime);
        }

        public async Task<bool> ClearRoutePointsAsync(int assignmentId)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            return await _deliveryRouteRepository.ClearRoutePointsAsync(assignmentId);
        }

        public async Task<Dictionary<string, object>> GetRouteStatisticsAsync(int assignmentId)
        {
            if (assignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");

            var routes = await _deliveryRouteRepository.GetRoutesByAssignmentAsync(assignmentId);
            if (!routes.Any())
                throw new InvalidOperationException($"Không có dữ liệu tuyến đường cho phân công ID: {assignmentId}");

            var orderedRoutes = routes.OrderBy(r => r.RecordedTime).ToList();
            var startPoint = orderedRoutes.First();
            var endPoint = orderedRoutes.Last();
            var totalDistance = CalculateTotalDistance(orderedRoutes);
            var totalDuration = (endPoint.RecordedTime - startPoint.RecordedTime).TotalHours;

            return new Dictionary<string, object>
            {
                ["TotalPoints"] = routes.Count(),
                ["StartLocation"] = new { Lat = startPoint.Latitude, Lng = startPoint.Longitude },
                ["EndLocation"] = new { Lat = endPoint.Latitude, Lng = endPoint.Longitude },
                ["TotalDistanceKm"] = Math.Round(totalDistance, 2),
                ["TotalDurationHours"] = Math.Round(totalDuration, 2),
                ["AverageSpeedKmh"] = totalDuration > 0 ? Math.Round(totalDistance / totalDuration, 2) : 0,
                ["AssignmentId"] = assignmentId,
                ["RouteDate"] = startPoint.RecordedTime.Date
            };
        }

        private bool IsValidLatitude(decimal? latitude)
        {
            return latitude >= -90m && latitude <= 90m;
        }

        private bool IsValidLongitude(decimal? longitude)
        {
            return longitude >= -180m && longitude <= 180m;
        }

        private double CalculateTotalDistance(List<DeliveryRoute> routes)
        {
            double totalDistance = 0;
            for (int i = 0; i < routes.Count - 1; i++)
            {
                var point1 = routes[i];
                var point2 = routes[i + 1];
                totalDistance += CalculateDistance(
                    (double)point1.Latitude, (double)point1.Longitude,
                    (double)point2.Latitude, (double)point2.Longitude
                );
            }
            return totalDistance;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula to calculate distance between two coordinates
            const double R = 6371; // Earth's radius in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private void ValidateDeliveryRoute(DeliveryRoute route)
        {
            if (route == null) throw new ArgumentNullException(nameof(route));
            if (route.AssignmentId <= 0) throw new ArgumentException("ID phân công không hợp lệ");
            if (!IsValidLatitude(route.Latitude)) throw new ArgumentException("Vĩ độ không hợp lệ");
            if (!IsValidLongitude(route.Longitude)) throw new ArgumentException("Kinh độ không hợp lệ");
            if (route.RecordedTime > DateTime.Now) throw new ArgumentException("Thời gian ghi nhận không được ở tương lai");
        }
    }
}