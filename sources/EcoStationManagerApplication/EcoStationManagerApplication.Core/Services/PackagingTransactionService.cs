using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class PackagingTransactionService : BaseService, IPackagingTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPackagingService _packagingService;
        private readonly ICustomerService _customerService;
        private readonly IPackagingInventoryService _packagingInventoryService;

        public PackagingTransactionService(IUnitOfWork unitOfWork, IPackagingService packagingService, ICustomerService customerService, IPackagingInventoryService packagingInventoryService)
            : base("PackagingTransactionService")
        {
            _unitOfWork = unitOfWork;
            _packagingService = packagingService;
            _customerService = customerService;
            _packagingInventoryService = packagingInventoryService;
        }

        public async Task<Result<PackagingTransaction>> GetTransactionByIdAsync(int transactionId)
        {
            try
            {
                if (transactionId <= 0)
                    return NotFoundError<PackagingTransaction>("Giao dịch bao bì", transactionId);

                var transaction = await _unitOfWork.PackagingTransactions.GetByIdAsync(transactionId);
                if (transaction == null)
                    return NotFoundError<PackagingTransaction>("Giao dịch bao bì", transactionId);

                return Result<PackagingTransaction>.Ok(transaction);
            }
            catch (Exception ex)
            {
                return HandleException<PackagingTransaction>(ex, "lấy thông tin giao dịch bao bì");
            }
        }

        public async Task<Result<List<PackagingTransaction>>> GetTransactionsByCustomerAsync(int customerId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<List<PackagingTransaction>>("Khách hàng", customerId);

                var transactions = await _unitOfWork.PackagingTransactions.GetByCustomerAsync(customerId, fromDate, toDate);
                return Result<List<PackagingTransaction>>.Ok(transactions.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<PackagingTransaction>>(ex, "lấy giao dịch theo khách hàng");
            }
        }

        public async Task<Result<List<PackagingTransaction>>> GetTransactionsByPackagingAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<List<PackagingTransaction>>("Bao bì", packagingId);

                var transactions = await _unitOfWork.PackagingTransactions.GetByPackagingAsync(packagingId);
                return Result<List<PackagingTransaction>>.Ok(transactions.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<PackagingTransaction>>(ex, "lấy giao dịch theo bao bì");
            }
        }

        public async Task<Result<PackagingTransaction>> IssuePackagingAsync(int packagingId, int? customerId, int quantity, decimal depositPrice, int? userId, string notes = null)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<PackagingTransaction>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<PackagingTransaction>("Số lượng phải lớn hơn 0");

                if (depositPrice < 0)
                    return BusinessError<PackagingTransaction>("Giá ký quỹ không được âm");

                // Kiểm tra bao bì tồn tại
                var packagingResult = await _packagingService.GetPackagingByIdAsync(packagingId);
                if (!packagingResult.Success)
                    return Result<PackagingTransaction>.Fail(packagingResult.Message);

                // Kiểm tra khách hàng tồn tại (nếu có)
                if (customerId.HasValue)
                {
                    var customerResult = await _customerService.GetCustomerByIdAsync(customerId.Value);
                    if (!customerResult.Success)
                        return Result<PackagingTransaction>.Fail(customerResult.Message);
                }

                // Kiểm tra tồn kho khả dụng (ưu tiên dùng Đã vệ sinh, sau đó Mới)
                var currentInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (currentInventory == null)
                    return BusinessError<PackagingTransaction>("Không tìm thấy tồn kho bao bì");

                var available = (currentInventory.QtyCleaned) + (currentInventory.QtyNew);
                if (available < quantity)
                    return BusinessError<PackagingTransaction>($"Không đủ bao bì khả dụng. Hiện có: {available}, yêu cầu: {quantity}");

                // Tính phân bổ: lấy từ đã vệ sinh trước, sau đó từ mới
                var takeFromCleaned = Math.Min(quantity, currentInventory.QtyCleaned);
                var remaining = quantity - takeFromCleaned;
                var takeFromNew = Math.Min(remaining, currentInventory.QtyNew);

                // Bắt đầu transaction để đảm bảo atomic
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // Ghi giao dịch phát hành
                    var success = await _unitOfWork.PackagingTransactions.IssuePackagingAsync(
                        packagingId, customerId, quantity, depositPrice, userId, notes);

                    if (!success)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return BusinessError<PackagingTransaction>("Không thể phát hành bao bì");
                    }

                    // Cập nhật tồn kho: giảm cleaned/new, tăng in_use
                    var newQuantities = new PackagingQuantities
                    {
                        QtyNew = currentInventory.QtyNew - takeFromNew,
                        QtyInUse = currentInventory.QtyInUse + quantity,
                        QtyReturned = currentInventory.QtyReturned,
                        QtyNeedCleaning = currentInventory.QtyNeedCleaning,
                        QtyCleaned = currentInventory.QtyCleaned - takeFromCleaned,
                        QtyDamaged = currentInventory.QtyDamaged
                    };

                    var updateSuccess = await _unitOfWork.PackagingInventories.UpdateQuantitiesAsync(packagingId, newQuantities);
                    if (!updateSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return BusinessError<PackagingTransaction>("Không thể cập nhật tồn kho bao bì khi phát hành");
                    }

                    await _unitOfWork.CommitTransactionAsync();

                    // Trả về giao dịch mới nhất
                    var transactions = await _unitOfWork.PackagingTransactions.GetByPackagingAsync(packagingId);
                    var latestTransaction = transactions.OrderByDescending(t => t.CreatedDate).FirstOrDefault();
                    return Result<PackagingTransaction>.Ok(latestTransaction, $"Đã phát hành {quantity} bao bì thành công");
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return HandleException<PackagingTransaction>(ex, "phát hành bao bì");
            }
        }

        public async Task<Result<PackagingTransaction>> ReturnPackagingAsync(int packagingId, int customerId, int quantity, decimal refundAmount, int? userId, string notes = null)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<PackagingTransaction>("Bao bì", packagingId);

                if (customerId <= 0)
                    return NotFoundError<PackagingTransaction>("Khách hàng", customerId);

                if (quantity <= 0)
                    return BusinessError<PackagingTransaction>("Số lượng phải lớn hơn 0");

                if (refundAmount < 0)
                    return BusinessError<PackagingTransaction>("Số tiền hoàn trả không được âm");

                // Kiểm tra bao bì tồn tại
                var packagingResult = await _packagingService.GetPackagingByIdAsync(packagingId);
                if (!packagingResult.Success)
                    return Result<PackagingTransaction>.Fail(packagingResult.Message);

                // Kiểm tra khách hàng tồn tại
                var customerResult = await _customerService.GetCustomerByIdAsync(customerId);
                if (!customerResult.Success)
                    return Result<PackagingTransaction>.Fail(customerResult.Message);

                //// Kiểm tra khách hàng có đang giữ đủ số lượng không
                //var holdingQuantity = await _unitOfWork.PackagingTransactions.GetCustomerHoldingQuantityAsync(customerId, packagingId);
                //if (holdingQuantity < quantity)
                //    return BusinessError<PackagingTransaction>($"Khách hàng chỉ đang giữ {holdingQuantity} bao bì, không đủ để trả {quantity}");

                // Thực hiện thu hồi bao bì và chuyển sang trạng thái cần vệ sinh trong một transaction
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var success = await _unitOfWork.PackagingTransactions.ReturnPackagingAsync(
                        packagingId, customerId, quantity, refundAmount, userId, notes);

                    if (!success)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return BusinessError<PackagingTransaction>("Không thể thu hồi bao bì");
                    }

                    // Cập nhật tồn kho: giảm đang dùng, tăng cần vệ sinh
                    var invUpdateSuccess = await _unitOfWork.PackagingInventories.ReturnForCleaningAsync(packagingId, quantity);
                    if (!invUpdateSuccess)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return BusinessError<PackagingTransaction>("Không thể chuyển bao bì sang trạng thái cần vệ sinh");
                    }

                    await _unitOfWork.CommitTransactionAsync();

                    var transactions = await _unitOfWork.PackagingTransactions.GetByPackagingAsync(packagingId);
                    var latestTransaction = transactions.OrderByDescending(t => t.CreatedDate).FirstOrDefault();
                    return Result<PackagingTransaction>.Ok(latestTransaction, $"Đã thu hồi {quantity} bao bì thành công");
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return HandleException<PackagingTransaction>(ex, "thu hồi bao bì");
            }
        }

        public async Task<Result<int>> GetCustomerHoldingQuantityAsync(int customerId, int packagingId)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<int>("Khách hàng", customerId);

                if (packagingId <= 0)
                    return NotFoundError<int>("Bao bì", packagingId);

                var holdingQuantity = await _unitOfWork.PackagingTransactions.GetCustomerHoldingQuantityAsync(customerId, packagingId);
                return Result<int>.Ok(holdingQuantity);
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "lấy số lượng bao bì khách hàng đang giữ");
            }
        }

        public async Task<Result<List<CustomerHoldingPackaging>>> GetCustomerHoldingsAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<List<CustomerHoldingPackaging>>("Khách hàng", customerId);

                var holdings = await _unitOfWork.PackagingTransactions.GetCustomerHoldingsAsync(customerId);
                return Result<List<CustomerHoldingPackaging>>.Ok(holdings.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<CustomerHoldingPackaging>>(ex, "lấy danh sách bao bì khách hàng đang giữ");
            }
        }

        public async Task<Result<decimal>> GetTotalDepositAmountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Validate date range
                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(startDate.Value, endDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<decimal>(dateErrors);
                }

                var totalDeposit = await _unitOfWork.PackagingTransactions.GetTotalDepositAmountAsync(startDate, endDate);
                return Result<decimal>.Ok(totalDeposit);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng tiền ký quỹ");
            }
        }

        public async Task<Result<decimal>> GetTotalRefundAmountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // Validate date range
                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(startDate.Value, endDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<decimal>(dateErrors);
                }

                var totalRefund = await _unitOfWork.PackagingTransactions.GetTotalRefundAmountAsync(startDate, endDate);
                return Result<decimal>.Ok(totalRefund);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng tiền hoàn trả");
            }
        }
    }
}
