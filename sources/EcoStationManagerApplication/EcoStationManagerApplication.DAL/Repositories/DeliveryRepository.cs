using Dapper;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class DeliveryRepository : BaseRepository<DeliveryAssignment>, IDeliveryRepository
    {
        public DeliveryRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "DeliveryAssignments", "assignment_id")
        { }

        public async Task<IEnumerable<DeliveryAssignment>> GetByDriverAsync(int driverId)
        {
            try
            {
                if (driverId <= 0)
                    return new List<DeliveryAssignment>();

                return await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetByDriver,
                    new { DriverId = driverId }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDriverAsync error - DriverId: {driverId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetByStatusAsync(DeliveryStatus status)
        {
            try
            {
                // Với TINYINT, cần pass số nguyên thay vì string
                return await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetByStatus,
                    new { Status = (int)status }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStatusAsync error - Status: {status} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetByDateRange,
                    new { StartDate = startDate.Date, EndDate = endDate.Date }
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByDateRangeAsync error - StartDate: {startDate}, EndDate: {endDate} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetPendingDeliveriesAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetPendingDeliveries
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPendingDeliveriesAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetCompletedDeliveriesAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetCompletedDeliveries
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetCompletedDeliveriesAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<decimal> GetTotalCODByDriverAsync(int driverId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (driverId <= 0)
                    return 0;

                var result = await _databaseHelper.QueryFirstOrDefaultAsync<dynamic>(
                    DeliveryQueries.GetTotalCODByDriver,
                    new
                    {
                        DriverId = driverId,
                        StartDate = startDate?.Date,
                        EndDate = endDate?.Date
                    }
                );

                return result?.total_cod ?? 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetTotalCODByDriverAsync error - DriverId: {driverId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateStatusAsync(int assignmentId, DeliveryStatus newStatus)
        {
            try
            {
                if (assignmentId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    DeliveryQueries.UpdateStatus,
                    new { AssignmentId = assignmentId, Status = (int)newStatus } // Với TINYINT, pass số nguyên
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật status cho delivery assignment - AssignmentId: {assignmentId}, Status: {newStatus}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateStatusAsync error - AssignmentId: {assignmentId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int assignmentId, DeliveryPaymentStatus newStatus)
        {
            try
            {
                if (assignmentId <= 0)
                    return false;

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    DeliveryQueries.UpdatePaymentStatus,
                    new { AssignmentId = assignmentId, PaymentStatus = (int)newStatus } // Với TINYINT, pass số nguyên
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật payment_status cho delivery assignment - AssignmentId: {assignmentId}, PaymentStatus: {newStatus}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdatePaymentStatusAsync error - AssignmentId: {assignmentId} - {ex.Message}");
                throw;
            }
        }

        public async Task<(IEnumerable<DeliveryAssignment> Deliveries, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            DeliveryStatus? status = null,
            DeliveryPaymentStatus? paymentStatus = null)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                int offset = (pageNumber - 1) * pageSize;

                var deliveries = await _databaseHelper.QueryAsync<DeliveryAssignment>(
                    DeliveryQueries.GetPaged,
                    new
                    {
                        Search = search,
                        Status = status.HasValue ? (int?)status.Value : null, // Với TINYINT, pass số nguyên
                        PaymentStatus = paymentStatus.HasValue ? (int?)paymentStatus.Value : null, // Với TINYINT, pass số nguyên
                        PageSize = pageSize,
                        Offset = offset
                    }
                );

                var totalCount = await _databaseHelper.ExecuteScalarAsync<int>(
                    DeliveryQueries.GetPagedCount,
                    new
                    {
                        Search = search,
                        Status = status.HasValue ? (int?)status.Value : null, // Với TINYINT, pass số nguyên
                        PaymentStatus = paymentStatus.HasValue ? (int?)paymentStatus.Value : null // Với TINYINT, pass số nguyên
                    }
                );

                return (deliveries, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPagedAsync error - PageNumber: {pageNumber}, PageSize: {pageSize} - {ex.Message}");
                throw;
            }
        }
    }
}

