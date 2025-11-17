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

        public async Task<Result<List<PackagingTransaction>>> GetTransactionsByCustomerAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    return NotFoundError<List<PackagingTransaction>>("Khách hàng", customerId);

                var transactions = await _unitOfWork.PackagingTransactions.GetByCustomerAsync(customerId);
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

                // Tự động gán userId từ context nếu chưa có
                if (!userId.HasValue)
                {
                    userId = GetCurrentUserId();
                }

                // Thực hiện phát hành bao bì
                var success = await _unitOfWork.PackagingTransactions.IssuePackagingAsync(
                    packagingId, customerId, quantity, depositPrice, userId, notes);

                if (!success)
                    return BusinessError<PackagingTransaction>("Không thể phát hành bao bì");

                // Lấy thông tin giao dịch vừa tạo
                var transactions = await _unitOfWork.PackagingTransactions.GetByPackagingAsync(packagingId);
                var latestTransaction = transactions.OrderByDescending(t => t.CreatedDate).FirstOrDefault();

                return Result<PackagingTransaction>.Ok(latestTransaction, $"Đã phát hành {quantity} bao bì thành công");
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

                // Kiểm tra khách hàng có đang giữ đủ số lượng không
                var holdingQuantity = await _unitOfWork.PackagingTransactions.GetCustomerHoldingQuantityAsync(customerId, packagingId);
                if (holdingQuantity < quantity)
                    return BusinessError<PackagingTransaction>($"Khách hàng chỉ đang giữ {holdingQuantity} bao bì, không đủ để trả {quantity}");

                // Tự động gán userId từ context nếu chưa có
                if (!userId.HasValue)
                {
                    userId = GetCurrentUserId();
                }

                // Thực hiện thu hồi bao bì
                var success = await _unitOfWork.PackagingTransactions.ReturnPackagingAsync(
                    packagingId, customerId, quantity, refundAmount, userId, notes);

                if (!success)
                    return BusinessError<PackagingTransaction>("Không thể thu hồi bao bì");

                // Lấy thông tin giao dịch vừa tạo
                var transactions = await _unitOfWork.PackagingTransactions.GetByPackagingAsync(packagingId);
                var latestTransaction = transactions.OrderByDescending(t => t.CreatedDate).FirstOrDefault();

                return Result<PackagingTransaction>.Ok(latestTransaction, $"Đã thu hồi {quantity} bao bì thành công");
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
