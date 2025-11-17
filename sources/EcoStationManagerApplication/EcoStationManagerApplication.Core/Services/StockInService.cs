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
    public class StockInService : BaseService, IStockInService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInventoryService _inventoryService;
        private readonly IPackagingInventoryService _packagingInventoryService;

        public StockInService(IUnitOfWork unitOfWork, IInventoryService inventoryService, IPackagingInventoryService packagingInventoryService)
            : base("StockInService")
        {
            _unitOfWork = unitOfWork;
            _inventoryService = inventoryService;
            _packagingInventoryService = packagingInventoryService;
        }

        public async Task<Result<StockIn>> GetStockInByIdAsync(int stockInId)
        {
            try
            {
                if (stockInId <= 0)
                    return NotFoundError<StockIn>("Phiếu nhập kho", stockInId);

                var stockIn = await _unitOfWork.StockIn.GetByIdAsync(stockInId);
                if (stockIn == null)
                    return NotFoundError<StockIn>("Phiếu nhập kho", stockInId);

                return Result<StockIn>.Ok(stockIn);
            }
            catch (Exception ex)
            {
                return HandleException<StockIn>(ex, "lấy thông tin phiếu nhập kho");
            }
        }

        public async Task<Result<List<StockIn>>> GetStockInByProductAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<List<StockIn>>("Sản phẩm", productId);

                var stockIns = await _unitOfWork.StockIn.GetByProductAsync(productId);
                return Result<List<StockIn>>.Ok(stockIns.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockIn>>(ex, "lấy phiếu nhập kho theo sản phẩm");
            }
        }

        public async Task<Result<List<StockIn>>> GetStockInByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate date range
                var dateErrors = ValidationHelper.ValidateDateRange(startDate, endDate);
                if (dateErrors.Any())
                    return ValidationError<List<StockIn>>(dateErrors);

                var stockIns = await _unitOfWork.StockIn.GetByDateRangeAsync(startDate, endDate);
                return Result<List<StockIn>>.Ok(stockIns.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockIn>>(ex, "lấy phiếu nhập kho theo khoảng thời gian");
            }
        }

        public async Task<Result<List<StockInDetail>>> GetStockInDetailsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Validate date range
                var dateErrors = ValidationHelper.ValidateDateRange(startDate, endDate);
                if (dateErrors.Any())
                    return ValidationError<List<StockInDetail>>(dateErrors);

                var stockInDetails = await _unitOfWork.StockIn.GetStockInDetailsAsync(startDate, endDate);
                return Result<List<StockInDetail>>.Ok(stockInDetails.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockInDetail>>(ex, "lấy chi tiết phiếu nhập kho theo khoảng thời gian");
            }
        }

        public async Task<Result<List<StockInDetail>>> GetStockInDetailsByBatchAsync(string batchNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(batchNo))
                    return ValidationError<List<StockInDetail>>(new List<string> { "Mã lô không được để trống" });

                var stockInDetails = await _unitOfWork.StockIn.GetStockInDetailsByBatchAsync(batchNo);
                return Result<List<StockInDetail>>.Ok(stockInDetails.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockInDetail>>(ex, "lấy chi tiết phiếu nhập kho theo lô hàng");
            }
        }

        public async Task<Result<StockIn>> CreateStockInAsync(StockIn stockIn)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateStockIn(stockIn);
                if (validationErrors.Any())
                    return ValidationError<StockIn>(validationErrors);

                // Kiểm tra số lô trùng
                if (!string.IsNullOrWhiteSpace(stockIn.BatchNo))
                {
                    var isBatchExists = await _unitOfWork.StockIn.IsBatchExistsAsync(
                        stockIn.BatchNo, stockIn.RefType, stockIn.RefId);
                    if (isBatchExists)
                        return BusinessError<StockIn>("Số lô đã tồn tại cho sản phẩm/bao bì này");
                }

                // Tự động gán created_by từ context (người nhập kho)
                var currentUserId = GetCurrentUserId();
                if (currentUserId.HasValue && stockIn.CreatedBy <= 0)
                {
                    stockIn.CreatedBy = currentUserId.Value;
                }

                // Thực hiện nhập kho
                await _unitOfWork.BeginTransactionAsync();
                {
                    try
                    {
                        // Thêm phiếu nhập kho
                        var stockInId = await _unitOfWork.StockIn.AddAsync(stockIn);
                        if (stockInId <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return BusinessError<StockIn>("Không thể tạo phiếu nhập kho");
                        }

                        // Cập nhật tồn kho tương ứng
                        if (stockIn.RefType == RefType.PRODUCT)
                        {
                            var addStockResult = await _inventoryService.AddStockAsync(
                                stockIn.RefId, stockIn.BatchNo, stockIn.Quantity, stockIn.ExpiryDate);

                            if (!addStockResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<StockIn>.Fail(addStockResult.Message);
                            }
                        }
                        else if (stockIn.RefType == RefType.PACKAGING)
                        {
                            var quantities = new PackagingQuantities
                            {
                                QtyNew = (int)stockIn.Quantity
                            };

                            var updateResult = await _packagingInventoryService.UpdatePackagingQuantitiesAsync(
                                stockIn.RefId, quantities);

                            if (!updateResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<StockIn>.Fail(updateResult.Message);
                            }
                        }

                        await _unitOfWork.CommitTransactionAsync();

                        // Lấy thông tin phiếu nhập kho vừa tạo
                        var createdStockIn = await _unitOfWork.StockIn.GetByIdAsync(stockInId);
                        return Result<StockIn>.Ok(createdStockIn, "Đã nhập kho thành công");
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
                return HandleException<StockIn>(ex, "tạo phiếu nhập kho");
            }
        }

        public async Task<Result<List<StockIn>>> CreateMultipleStockInsAsync(List<StockIn> stockIns)
        {
            try
            {
                if (stockIns == null || !stockIns.Any())
                    return BusinessError<List<StockIn>>("Danh sách phiếu nhập kho không được để trống");

                // Validate tất cả các phiếu
                var allErrors = new List<string>();
                foreach (var stockIn in stockIns)
                {
                    var validationErrors = ValidationHelper.ValidateStockIn(stockIn);
                    if (validationErrors.Any())
                    {
                        allErrors.AddRange(validationErrors);
                    }
                }

                if (allErrors.Any())
                    return ValidationError<List<StockIn>>(allErrors);

                // Kiểm tra số lô trùng
                foreach (var stockIn in stockIns)
                {
                    if (!string.IsNullOrWhiteSpace(stockIn.BatchNo))
                    {
                        var isBatchExists = await _unitOfWork.StockIn.IsBatchExistsAsync(
                            stockIn.BatchNo, stockIn.RefType, stockIn.RefId);
                        if (isBatchExists)
                            return BusinessError<List<StockIn>>($"Số lô '{stockIn.BatchNo}' đã tồn tại cho sản phẩm/bao bì này");
                    }
                }

                // Thực hiện nhập kho cho tất cả các phiếu
                await _unitOfWork.BeginTransactionAsync();
                var createdStockIns = new List<StockIn>();
                var currentUserId = GetCurrentUserId();
                try
                {
                    foreach (var stockIn in stockIns)
                    {
                        // Tự động gán created_by từ context (người nhập kho)
                        if (currentUserId.HasValue && stockIn.CreatedBy <= 0)
                        {
                            stockIn.CreatedBy = currentUserId.Value;
                        }

                        // Thêm phiếu nhập kho
                        var stockInId = await _unitOfWork.StockIn.AddAsync(stockIn);
                        if (stockInId <= 0)
                        {
                            await _unitOfWork.RollbackTransactionAsync();
                            return BusinessError<List<StockIn>>("Không thể tạo phiếu nhập kho");
                        }

                        // Cập nhật tồn kho tương ứng
                        if (stockIn.RefType == RefType.PRODUCT)
                        {
                            var addStockResult = await _inventoryService.AddStockAsync(
                                stockIn.RefId, stockIn.BatchNo, stockIn.Quantity, stockIn.ExpiryDate);

                            if (!addStockResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<List<StockIn>>.Fail(addStockResult.Message);
                            }
                        }
                        else if (stockIn.RefType == RefType.PACKAGING)
                        {
                            var quantities = new PackagingQuantities
                            {
                                QtyNew = (int)stockIn.Quantity
                            };

                            var updateResult = await _packagingInventoryService.UpdatePackagingQuantitiesAsync(
                                stockIn.RefId, quantities);

                            if (!updateResult.Success)
                            {
                                await _unitOfWork.RollbackTransactionAsync();
                                return Result<List<StockIn>>.Fail(updateResult.Message);
                            }
                        }

                        // Lấy thông tin phiếu nhập kho vừa tạo
                        var createdStockIn = await _unitOfWork.StockIn.GetByIdAsync(stockInId);
                        createdStockIns.Add(createdStockIn);
                    }

                    await _unitOfWork.CommitTransactionAsync();
                    return Result<List<StockIn>>.Ok(createdStockIns, $"Đã nhập kho thành công {createdStockIns.Count} sản phẩm");
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return HandleException<List<StockIn>>(ex, "tạo nhiều phiếu nhập kho");
            }
        }

        public async Task<Result<decimal>> GetTotalStockInValueAsync(DateTime? startDate = null, DateTime? endDate = null)
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

                var totalValue = await _unitOfWork.StockIn.GetTotalStockInValueAsync(startDate, endDate);
                return Result<decimal>.Ok(totalValue);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng giá trị nhập kho");
            }
        }

        public async Task<Result<List<StockInSummary>>> GetTopStockInProductsAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                if (limit <= 0 || limit > 100)
                    return BusinessError<List<StockInSummary>>("Số lượng bản ghi phải từ 1 đến 100");

                // Validate date range
                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(startDate.Value, endDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<List<StockInSummary>>(dateErrors);
                }

                var topProducts = await _unitOfWork.StockIn.GetTopStockInProductsAsync(limit, startDate, endDate);
                return Result<List<StockInSummary>>.Ok(topProducts.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<StockInSummary>>(ex, "lấy top sản phẩm nhập kho");
            }
        }
    }
}
