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
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class InventoryService : BaseService, IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public InventoryService(IUnitOfWork unitOfWork, IProductService productService)
            : base("InventoryService")
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public async Task<Result<Inventory>> GetInventoryByIdAsync(int inventoryId)
        {
            try
            {
                if (inventoryId <= 0)
                    return NotFoundError<Inventory>("Tồn kho", inventoryId);

                var inventory = await _unitOfWork.Inventories.GetByIdAsync(inventoryId);
                if (inventory == null)
                    return NotFoundError<Inventory>("Tồn kho", inventoryId);

                return Result<Inventory>.Ok(inventory);
            }
            catch (Exception ex)
            {
                return HandleException<Inventory>(ex, "lấy thông tin tồn kho");
            }
        }

        public async Task<Result<List<Inventory>>> GetAllAsync()
        {
            try
            {
                var inventories = await _unitOfWork.Inventories.GetAllAsync();
                return Result<List<Inventory>>.Ok(inventories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Inventory>>(ex, "lấy tất cả tồn kho");
            }
        }

        public async Task<Result<List<Inventory>>> GetInventoryByProductAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<List<Inventory>>("Sản phẩm", productId);

                var inventories = await _unitOfWork.Inventories.GetByProductAsync(productId);
                return Result<List<Inventory>>.Ok(inventories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Inventory>>(ex, "lấy tồn kho theo sản phẩm");
            }
        }

        public async Task<Result<List<Inventory>>> GetLowStockItemsAsync()
        {
            try
            {
                var lowStockItems = await _unitOfWork.Inventories.GetLowStockItemsAsync();
                return Result<List<Inventory>>.Ok(lowStockItems.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Inventory>>(ex, "lấy danh sách tồn kho thấp");
            }
        }

        public async Task<Result<List<Inventory>>> GetExpiringItemsAsync(int daysThreshold = 15)
        {
            try
            {
                if (daysThreshold <= 0)
                    return BusinessError<List<Inventory>>("Số ngày cảnh báo phải lớn hơn 0");

                var expiringItems = await _unitOfWork.Inventories.GetExpiringItemsAsync(daysThreshold);
                return Result<List<Inventory>>.Ok(expiringItems.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Inventory>>(ex, "lấy danh sách sản phẩm sắp hết hạn");
            }
        }

        public async Task<Result<decimal>> GetTotalStockQuantityAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<decimal>("Sản phẩm", productId);

                var totalQuantity = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(productId);
                return Result<decimal>.Ok(totalQuantity);
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "lấy tổng số lượng tồn kho");
            }
        }

        public async Task<Result<Inventory>> AddStockAsync(int productId, string batchNo, decimal quantity, DateTime? expiryDate)
        {
            try
            {
                // Validate input
                if (productId <= 0)
                    return NotFoundError<Inventory>("Sản phẩm", productId);

                if (string.IsNullOrWhiteSpace(batchNo))
                    return BusinessError<Inventory>("Số lô không được để trống");

                if (quantity <= 0)
                    return BusinessError<Inventory>("Số lượng phải lớn hơn 0");

                if (expiryDate.HasValue && expiryDate.Value < DateTime.Today)
                    return BusinessError<Inventory>("Hạn sử dụng không được ở trong quá khứ");

                // Kiểm tra sản phẩm tồn tại
                var productResult = await _productService.GetProductByIdAsync(productId);
                if (!productResult.Success)
                    return Result<Inventory>.Fail(productResult.Message);

                // Thêm tồn kho
                var success = await _unitOfWork.Inventories.AddStockAsync(productId, batchNo, quantity, expiryDate);
                if (!success)
                    return BusinessError<Inventory>("Không thể thêm tồn kho");

                // Lấy thông tin inventory vừa tạo
                var inventory = await _unitOfWork.Inventories.GetByProductAndBatchAsync(productId, batchNo);
                return Result<Inventory>.Ok(inventory, "Đã thêm tồn kho thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Inventory>(ex, "thêm tồn kho");
            }
        }

        public async Task<Result<bool>> UpdateStockQuantityAsync(int inventoryId, decimal newQuantity)
        {
            try
            {
                if (inventoryId <= 0)
                    return NotFoundError<bool>("Tồn kho", inventoryId);

                if (newQuantity < 0)
                    return BusinessError<bool>("Số lượng tồn kho không được âm");

                var inventory = await _unitOfWork.Inventories.GetByIdAsync(inventoryId);
                if (inventory == null)
                    return NotFoundError<bool>("Tồn kho", inventoryId);

                var success = await _unitOfWork.Inventories.UpdateStockQuantityAsync(inventoryId, newQuantity);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật số lượng tồn kho");

                return Result<bool>.Ok(true, "Đã cập nhật số lượng tồn kho thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật số lượng tồn kho");
            }
        }

        public async Task<Result<bool>> ReduceStockAsync(int productId, string batchNo, decimal quantity)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                if (string.IsNullOrWhiteSpace(batchNo))
                    return BusinessError<bool>("Số lô không được để trống");

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                // Kiểm tra đủ tồn kho
                var isSufficient = await _unitOfWork.Inventories.IsStockSufficientAsync(productId, quantity);
                if (!isSufficient)
                {
                    var totalStock = await _unitOfWork.Inventories.GetTotalStockQuantityAsync(productId);
                    return BusinessError<bool>($"Không đủ tồn kho. Tồn kho hiện có: {totalStock}, yêu cầu: {quantity}");
                }

                var success = await _unitOfWork.Inventories.ReduceStockAsync(productId, batchNo, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể giảm tồn kho");

                return Result<bool>.Ok(true, "Đã giảm tồn kho thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "giảm tồn kho");
            }
        }

        public async Task<Result<bool>> IsStockSufficientAsync(int productId, decimal requiredQuantity)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                if (requiredQuantity <= 0)
                    return BusinessError<bool>("Số lượng yêu cầu phải lớn hơn 0");

                var isSufficient = await _unitOfWork.Inventories.IsStockSufficientAsync(productId, requiredQuantity);
                return Result<bool>.Ok(isSufficient);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra tồn kho");
            }
        }

        public async Task<Result<List<Inventory>>> GetInventoryHistoryAsync(
            int productId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<List<Inventory>>("Sản phẩm", productId);

                // Validate date range
                if (fromDate.HasValue && toDate.HasValue)
                {
                    var dateErrors = ValidationHelper.ValidateDateRange(fromDate.Value, toDate.Value);
                    if (dateErrors.Any())
                        return ValidationError<List<Inventory>>(dateErrors);
                }

                var history = await _unitOfWork.Inventories.GetInventoryHistoryAsync(productId, fromDate, toDate);
                return Result<List<Inventory>>.Ok(history.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Inventory>>(ex, "lấy lịch sử tồn kho");
            }
        }
    }
}