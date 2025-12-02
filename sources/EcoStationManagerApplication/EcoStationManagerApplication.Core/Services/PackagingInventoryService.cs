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
    public class PackagingInventoryService : BaseService, IPackagingInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPackagingService _packagingService;

        public PackagingInventoryService(IUnitOfWork unitOfWork, IPackagingService packagingService)
            : base("PackagingInventoryService")
        {
            _unitOfWork = unitOfWork;
            _packagingService = packagingService;
        }

        public async Task<Result<IEnumerable<PackagingInventory>>> GetAllAsync()
        {
            try
            {
                var packagingInventories = await _unitOfWork.PackagingInventories.GetAllAsync();
                
                // Nếu không có dữ liệu, trả về danh sách rỗng thay vì NotFoundError
                if (packagingInventories == null || !packagingInventories.Any())
                {
                    return Result<IEnumerable<PackagingInventory>>.Ok(
                        new List<PackagingInventory>(), 
                        "Không có tồn kho bao bì nào");
                }

                return Result<IEnumerable<PackagingInventory>>.Ok(packagingInventories);
            }
            catch (Exception ex) 
            {
                return HandleException<IEnumerable<PackagingInventory>>(ex, "lấy thông tin tồn kho bao bì");
            }
        }


        public async Task<Result<PackagingInventory>> GetPackagingInventoryAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<PackagingInventory>("Bao bì", packagingId);

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null)
                    return NotFoundError<PackagingInventory>("Tồn kho bao bì", packagingId);

                return Result<PackagingInventory>.Ok(packagingInventory);
            }
            catch (Exception ex)
            {
                return HandleException<PackagingInventory>(ex, "lấy thông tin tồn kho bao bì");
            }
        }

        public async Task<Result<List<PackagingInventory>>> GetLowStockPackagingAsync()
        {
            try
            {
                var lowStockPackaging = await _unitOfWork.PackagingInventories.GetLowStockPackagingAsync();
                return Result<List<PackagingInventory>>.Ok(lowStockPackaging.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<PackagingInventory>>(ex, "lấy danh sách bao bì tồn kho thấp");
            }
        }

        public async Task<Result<bool>> UpdatePackagingQuantitiesAsync(int packagingId, PackagingQuantities quantities)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantities == null)
                    return BusinessError<bool>("Dữ liệu số lượng không hợp lệ");

                // Validate quantities
                if (quantities.QtyNew < 0 || quantities.QtyInUse < 0 || quantities.QtyReturned < 0 ||
                    quantities.QtyNeedCleaning < 0 || quantities.QtyCleaned < 0 || quantities.QtyDamaged < 0)
                    return BusinessError<bool>("Số lượng không được âm");

                // Kiểm tra bao bì tồn tại
                var packagingResult = await _packagingService.GetPackagingByIdAsync(packagingId);
                if (!packagingResult.Success)
                    return Result<bool>.Fail(packagingResult.Message);

                var success = await _unitOfWork.PackagingInventories.UpdateQuantitiesAsync(packagingId, quantities);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật số lượng bao bì");

                return Result<bool>.Ok(true, "Đã cập nhật số lượng bao bì thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật số lượng bao bì");
            }
        }

        public async Task<Result<bool>> AddPackagingNewAsync(int packagingId, PackagingQuantities quantities)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantities == null)
                    return BusinessError<bool>("Dữ liệu số lượng không hợp lệ");

                if (quantities.QtyNew <= 0)
                    return BusinessError<bool>("Số lượng bao bì mới phải lớn hơn 0");

                var packagingResult = await _packagingService.GetPackagingByIdAsync(packagingId);
                if (!packagingResult.Success)
                    return Result<bool>.Fail(packagingResult.Message);

                var currentInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (currentInventory == null)
                    return NotFoundError<bool>("Tồn kho bao bì", packagingId);

                var newQuantities = new PackagingQuantities
                {
                    QtyNew = (currentInventory.QtyNew) + quantities.QtyNew,
                    QtyInUse = currentInventory.QtyInUse,
                    QtyReturned = currentInventory.QtyReturned,
                    QtyNeedCleaning = currentInventory.QtyNeedCleaning,
                    QtyCleaned = currentInventory.QtyCleaned,
                    QtyDamaged = currentInventory.QtyDamaged
                };

                var success = await _unitOfWork.PackagingInventories.UpdateQuantitiesAsync(packagingId, newQuantities);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật số lượng bao bì mới");

                return Result<bool>.Ok(true, $"Đã thêm {quantities.QtyNew} bao bì mới");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thêm bao bì mới");
            }
        }

        public async Task<Result<bool>> TransferToInUseAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                // Kiểm tra đủ bao bì mới
                var isSufficient = await _unitOfWork.PackagingInventories.IsNewPackagingSufficientAsync(packagingId, quantity);
                if (!isSufficient)
                {
                    var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                    return BusinessError<bool>($"Không đủ bao bì mới. Số lượng hiện có: {packagingInventory?.QtyNew ?? 0}, yêu cầu: {quantity}");
                }

                var success = await _unitOfWork.PackagingInventories.TransferToInUseAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể chuyển bao bì sang trạng thái sử dụng");

                return Result<bool>.Ok(true, $"Đã chuyển {quantity} bao bì sang trạng thái sử dụng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "chuyển bao bì sang sử dụng");
            }
        }

        public async Task<Result<bool>> ReturnForCleaningAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                // Kiểm tra đủ bao bì đang sử dụng
                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyInUse < quantity)
                    return BusinessError<bool>($"Không đủ bao bì đang sử dụng để trả về vệ sinh");

                var success = await _unitOfWork.PackagingInventories.ReturnForCleaningAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể nhận bao bì trả về vệ sinh");

                return Result<bool>.Ok(true, $"Đã nhận {quantity} bao bì cần vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "nhận bao bì trả về vệ sinh");
            }
        }

        public async Task<Result<bool>> MoveReturnedToNeedCleaningAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyReturned < quantity)
                    return BusinessError<bool>("Không đủ số lượng bao bì trả về để chuyển sang cần vệ sinh");

                var success = await _unitOfWork.PackagingInventories.MoveReturnedToNeedCleaningAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể chuyển bao bì trả về sang cần vệ sinh");

                return Result<bool>.Ok(true, $"Đã chuyển {quantity} từ trả về sang cần vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "chuyển trả về sang cần vệ sinh");
            }
        }

        public async Task<Result<bool>> CompleteCleaningAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                // Kiểm tra đủ bao bì cần vệ sinh
                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyNeedCleaning < quantity)
                    return BusinessError<bool>($"Không đủ bao bì cần vệ sinh để hoàn thành");

                var success = await _unitOfWork.PackagingInventories.CompleteCleaningAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể hoàn thành vệ sinh bao bì");

                return Result<bool>.Ok(true, $"Đã hoàn thành vệ sinh {quantity} bao bì, cập nhật đã vệ sinh và mới");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "hoàn thành vệ sinh bao bì");
            }
        }

        public async Task<Result<bool>> MarkAsDamagedAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null)
                    return NotFoundError<bool>("Tồn kho bao bì", packagingId);

                if (packagingInventory.QtyCleaned < quantity)
                    return BusinessError<bool>("Không đủ bao bì đã vệ sinh để chuyển sang hỏng");

                var success = await _unitOfWork.PackagingInventories.MarkAsDamagedAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể đánh dấu bao bì hỏng");

                return Result<bool>.Ok(true, $"Đã đánh dấu {quantity} bao bì hỏng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đánh dấu bao bì hỏng");
            }
        }

        public async Task<Result<bool>> MarkReturnedAsDamagedAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyReturned < quantity)
                    return BusinessError<bool>("Không đủ bao bì trả về để chuyển sang hỏng");

                var success = await _unitOfWork.PackagingInventories.MarkReturnedAsDamagedAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể chuyển trả về sang hỏng");

                return Result<bool>.Ok(true, $"Đã chuyển {quantity} trả về sang hỏng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đánh dấu hỏng từ trả về");
            }
        }

        public async Task<Result<bool>> MarkNewAsDamagedAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyNew < quantity)
                    return BusinessError<bool>("Không đủ bao bì mới để chuyển sang hỏng");

                var success = await _unitOfWork.PackagingInventories.MarkNewAsDamagedAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể chuyển mới sang hỏng");

                return Result<bool>.Ok(true, $"Đã chuyển {quantity} mới sang hỏng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đánh dấu hỏng từ mới");
            }
        }

        public async Task<Result<bool>> MarkNeedCleaningAsDamagedAsync(int packagingId, int quantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (quantity <= 0)
                    return BusinessError<bool>("Số lượng phải lớn hơn 0");

                var packagingInventory = await _unitOfWork.PackagingInventories.GetByPackagingAsync(packagingId);
                if (packagingInventory == null || packagingInventory.QtyNeedCleaning < quantity)
                    return BusinessError<bool>("Không đủ bao bì cần vệ sinh để chuyển sang hỏng");

                var success = await _unitOfWork.PackagingInventories.MarkNeedCleaningAsDamagedAsync(packagingId, quantity);
                if (!success)
                    return BusinessError<bool>("Không thể chuyển cần vệ sinh sang hỏng");

                return Result<bool>.Ok(true, $"Đã chuyển {quantity} cần vệ sinh sang hỏng");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đánh dấu hỏng từ cần vệ sinh");
            }
        }

        public async Task<Result<PackagingQuantities>> GetPackagingQuantitiesAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<PackagingQuantities>("Bao bì", packagingId);

                var quantities = await _unitOfWork.PackagingInventories.GetPackagingQuantitiesAsync(packagingId);
                return Result<PackagingQuantities>.Ok(quantities);
            }
            catch (Exception ex)
            {
                return HandleException<PackagingQuantities>(ex, "lấy số lượng bao bì");
            }
        }

        public async Task<Result<bool>> IsNewPackagingSufficientAsync(int packagingId, int requiredQuantity)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                if (requiredQuantity <= 0)
                    return BusinessError<bool>("Số lượng yêu cầu phải lớn hơn 0");

                var isSufficient = await _unitOfWork.PackagingInventories.IsNewPackagingSufficientAsync(packagingId, requiredQuantity);
                return Result<bool>.Ok(isSufficient);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra bao bì mới");
            }
        }
    }
}
