using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class StockOutService : BaseService, IStockOutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryService _inventoryService;
        private readonly IPackagingInventoryService _packagingInventoryService;

        public StockOutService(IUnitOfWork unitOfWork, IInventoryService inventoryService, IPackagingInventoryService packagingInventoryService)
            : base("StockOutService")
        {
            _unitOfWork = unitOfWork;
            _inventoryService = inventoryService;
            _packagingInventoryService = packagingInventoryService;
        }

        public async Task<Result<StockOut>> GetStockOutByIdAsync(int stockOutId)
        {
            try
            {
                if (stockOutId <= 0)
                    return NotFoundError<StockOut>("Phiếu xuất kho", stockOutId);

                var stockOut = await _unitOfWork.StockOut.GetByIdAsync(stockOutId);
                if (stockOut == null)
                    return NotFoundError<StockOut>("Phiếu xuất kho", stockOutId);

                return Result<StockOut>.Ok(stockOut);
            }
            catch (Exception ex)
            {
                return HandleException<StockOut>(ex, "lấy thông tin phiếu xuất kho");
            }
        }

        public async Task<Result<List<StockOut>>> GetStockOutByProductAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<List<StockOut>>("Sản phẩm", productId);

                var stockOuts = await _unitOfWork.StockOut.GetByProductAsync(productId);
                return Result<List<StockOut>>.Ok(stockOuts.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockOut>>(ex, "lấy phiếu xuất kho theo sản phẩm");
            }
        }

        public async Task<Result<List<StockOut>>> GetStockOutByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate date range
                var dateErrors = ValidationHelper.ValidateDateRange(startDate, endDate);
                if (dateErrors.Any())
                    return ValidationError<List<StockOut>>(dateErrors);

                var stockOuts = await _unitOfWork.StockOut.GetByDateRangeAsync(startDate, endDate);
                return Result<List<StockOut>>.Ok(stockOuts.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockOut>>(ex, "lấy phiếu xuất kho theo khoảng thời gian");
            }
        }

        public async Task<Result<List<StockOutDetail>>> GetStockOutDetailsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate date range
                var dateErrors = ValidationHelper.ValidateDateRange(startDate, endDate);
                if (dateErrors.Any())
                    return ValidationError<List<StockOutDetail>>(dateErrors);

                var stockOutDetails = await _unitOfWork.StockOut.GetStockOutDetailsAsync(startDate, endDate);
                return Result<List<StockOutDetail>>.Ok(stockOutDetails.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockOutDetail>>(ex, "lấy chi tiết phiếu xuất kho theo khoảng thời gian");
            }
        }

        public async Task<Result<StockOut>> CreateStockOutAsync(StockOut stockOut)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateStockOut(stockOut);
                if (validationErrors.Any())
                    return ValidationError<StockOut>(validationErrors);

                // Kiểm tra tồn kho
                if (stockOut.RefType == RefType.PRODUCT)
                {
                    var isSufficient = await _inventoryService.IsStockSufficientAsync(stockOut.RefId, stockOut.Quantity);
                    if (!isSufficient.Data)
                    {
                        var totalStock = await _inventoryService.GetTotalStockQuantityAsync(stockOut.RefId);
                        return BusinessError<StockOut>($"Không đủ tồn kho. Tồn kho hiện có: {totalStock.Data}, yêu cầu: {stockOut.Quantity}");
                    }
                }
                else if (stockOut.RefType == RefType.PACKAGING)
                {
                    var isSufficient = await _packagingInventoryService.IsNewPackagingSufficientAsync(stockOut.RefId, (int)stockOut.Quantity);
                    if (!isSufficient.Data)
                    {
                        var packagingInventory = await _packagingInventoryService.GetPackagingInventoryAsync(stockOut.RefId);
                        return BusinessError<StockOut>($"Không đủ bao bì mới. Số lượng hiện có: {packagingInventory.Data?.QtyNew ?? 0}, yêu cầu: {stockOut.Quantity}");
                        }
                }

                // Thực hiện xuất kho
                await _unitOfWork.BeginTransactionAsync();
                {
                    try
                    {
                        // Thêm phiếu xuất kho
                        var stockOutId = await _unitOfWork.StockOut.AddAsync(stockOut);
                        if (stockOutId <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return BusinessError<StockOut>("Không thể tạo phiếu xuất kho");
                        }

                        // Cập nhật tồn kho tương ứng
                        if (stockOut.RefType == RefType.PRODUCT)
                        {
                            var reduceResult = await _inventoryService.ReduceStockAsync(
                                stockOut.RefId, stockOut.BatchNo, stockOut.Quantity);

                            if (!reduceResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<StockOut>.Fail(reduceResult.Message);
                            }
                        }
                        else if (stockOut.RefType == RefType.PACKAGING)
                        {
                            var transferResult = await _packagingInventoryService.TransferToInUseAsync(
                                stockOut.RefId, (int)stockOut.Quantity);

                            if (!transferResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<StockOut>.Fail(transferResult.Message);
                            }
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        // Lấy thông tin phiếu xuất kho vừa tạo
                        var createdStockOut = await _unitOfWork.StockOut.GetByIdAsync(stockOutId);
                        return Result<StockOut>.Ok(createdStockOut, "Đã xuất kho thành công");
                    }
                    catch (Exception)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return HandleException<StockOut>(ex, "tạo phiếu xuất kho");
            }
        }

        public async Task<Result<List<StockOut>>> CreateMultipleStockOutsAsync(List<StockOut> stockOuts)
        {
            try
            {
                if (stockOuts == null || !stockOuts.Any())
                    return BusinessError<List<StockOut>>("Danh sách phiếu xuất kho không được để trống");

                // Validate tất cả các phiếu
                var allErrors = new List<string>();
                foreach (var stockOut in stockOuts)
                {
                    var validationErrors = ValidationHelper.ValidateStockOut(stockOut);
                    if (validationErrors.Any())
                    {
                        allErrors.AddRange(validationErrors);
                    }
                }

                if (allErrors.Any())
                    return ValidationError<List<StockOut>>(allErrors);

                // Kiểm tra tồn kho cho tất cả các sản phẩm trước
                var insufficientItems = new List<string>();
                foreach (var stockOut in stockOuts)
                {
                    if (stockOut.RefType == RefType.PRODUCT)
                    {
                        var isSufficient = await _inventoryService.IsStockSufficientAsync(stockOut.RefId, stockOut.Quantity);
                        if (!isSufficient.Data)
                        {
                            var totalStock = await _inventoryService.GetTotalStockQuantityAsync(stockOut.RefId);
                            insufficientItems.Add($"Sản phẩm ID {stockOut.RefId}: Tồn kho hiện có: {totalStock.Data}, yêu cầu: {stockOut.Quantity}");
                        }
                    }
                    else if (stockOut.RefType == RefType.PACKAGING)
                    {
                        var isSufficient = await _packagingInventoryService.IsNewPackagingSufficientAsync(stockOut.RefId, (int)stockOut.Quantity);
                        if (!isSufficient.Data)
                        {
                            var packagingInventory = await _packagingInventoryService.GetPackagingInventoryAsync(stockOut.RefId);
                            insufficientItems.Add($"Bao bì ID {stockOut.RefId}: Số lượng hiện có: {packagingInventory.Data?.QtyNew ?? 0}, yêu cầu: {stockOut.Quantity}");
                        }
                    }
                }

                if (insufficientItems.Any())
                {
                    return BusinessError<List<StockOut>>($"Không đủ tồn kho:\n{string.Join("\n", insufficientItems)}");
                }

                // Thực hiện xuất kho cho tất cả các phiếu
                await _unitOfWork.BeginTransactionAsync();
                var createdStockOuts = new List<StockOut>();
                try
                {
                    foreach (var stockOut in stockOuts)
                    {
                        // Thêm phiếu xuất kho
                        var stockOutId = await _unitOfWork.StockOut.AddAsync(stockOut);
                        if (stockOutId <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return BusinessError<List<StockOut>>("Không thể tạo phiếu xuất kho");
                        }

                        // Cập nhật tồn kho tương ứng
                        if (stockOut.RefType == RefType.PRODUCT)
                        {
                            var reduceResult = await _inventoryService.ReduceStockAsync(
                                stockOut.RefId, stockOut.BatchNo, stockOut.Quantity);

                            if (!reduceResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<List<StockOut>>.Fail(reduceResult.Message);
                            }
                        }
                        else if (stockOut.RefType == RefType.PACKAGING)
                        {
                            var transferResult = await _packagingInventoryService.TransferToInUseAsync(
                                stockOut.RefId, (int)stockOut.Quantity);

                            if (!transferResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<List<StockOut>>.Fail(transferResult.Message);
                            }
                        }

                        // Lấy thông tin phiếu xuất kho vừa tạo
                        var createdStockOut = await _unitOfWork.StockOut.GetByIdAsync(stockOutId);
                        createdStockOuts.Add(createdStockOut);
                    }

                    await _unitOfWork.CommitTransactionAsync();
                    return Result<List<StockOut>>.Ok(createdStockOuts, $"Đã xuất kho thành công {createdStockOuts.Count} sản phẩm");
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return HandleException<List<StockOut>>(ex, "tạo nhiều phiếu xuất kho");
            }
        }

        public async Task<Result<bool>> StockOutForOrderAsync(int productId, string batchNo, decimal quantity, int orderId, int userId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                if (string.IsNullOrWhiteSpace(batchNo))
                    return BusinessError<bool>("Số lô không được để trống");

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                if (orderId <= 0)
                    return NotFoundError<bool>("Đơn hàng", orderId);

                if (userId <= 0)
                    return NotFoundError<bool>("Người dùng", userId);

                var success = await _unitOfWork.StockOut.StockOutForOrderAsync(productId, batchNo, quantity, orderId, userId);
                if (!success)
                    return BusinessError<bool>("Không thể xuất kho cho đơn hàng");

                return Result<bool>.Ok(true, "Đã xuất kho cho đơn hàng thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xuất kho cho đơn hàng");
            }
        }

        public async Task<Result<decimal>> GetTotalStockOutValueAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var totalValue = await _unitOfWork.StockOut.GetTotalStockOutValueAsync(startDate, endDate);
                return Result<decimal>.Ok(totalValue);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng giá trị xuất kho");
            }
        }

        public async Task<Result<List<StockOutSummary>>> GetTopStockOutProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                    return BusinessError<List<StockOutSummary>>("Số lượng bản ghi phải từ 1 đến 100");

                // Validate date range
                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(startDate.Value, endDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<List<StockOutSummary>>(dateErrors);
                }

                var topProducts = await _unitOfWork.StockOut.GetTopStockOutProductsAsync(limit, startDate, endDate);
                return Result<List<StockOutSummary>>.Ok(topProducts.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockOutSummary>>(ex, "lấy top sản phẩm xuất kho");
            }
        }
    }
}
