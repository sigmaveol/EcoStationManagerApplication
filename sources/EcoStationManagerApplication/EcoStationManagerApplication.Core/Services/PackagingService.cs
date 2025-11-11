using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class PackagingService : BaseService, IPackagingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackagingService(IUnitOfWork unitOfWork)
            : base("PackagingService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Packaging>> GetPackagingByIdAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<Packaging>("Bao bì", packagingId);

                var packaging = await _unitOfWork.Packaging.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<Packaging>("Bao bì", packagingId);

                return Result<Packaging>.Ok(packaging);
            }
            catch (Exception ex)
            {
                return HandleException<Packaging>(ex, "lấy thông tin bao bì");
            }
        }

        public async Task<Result<Packaging>> GetPackagingByBarcodeAsync(string barcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                    return BusinessError<Packaging>("Barcode không được để trống");

                var packaging = await _unitOfWork.Packaging.GetByBarcodeAsync(barcode);
                if (packaging == null)
                    return NotFoundError<Packaging>($"Không tìm thấy bao bì với barcode: {barcode}");

                return Result<Packaging>.Ok(packaging);
            }
            catch (Exception ex)
            {
                return HandleException<Packaging>(ex, "lấy bao bì theo barcode");
            }
        }

        public async Task<Result<IEnumerable<Packaging>>> GetAllPackagingsAsync()
        {
            try
            {
                var packagings = await _unitOfWork.Packaging.GetAllAsync();
                return Result<IEnumerable<Packaging>>.Ok(packagings);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Packaging>>(ex, "lấy danh sách bao bì");
            }
        }

        public async Task<Result<IEnumerable<Packaging>>> GetPackagingsByTypeAsync(string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type))
                    return BusinessError<IEnumerable<Packaging>>("Loại bao bì không được để trống");

                var packagings = await _unitOfWork.Packaging.GetByTypeAsync(type);
                return Result<IEnumerable<Packaging>>.Ok(packagings);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Packaging>>(ex, "lấy bao bì theo loại");
            }
        }

        public async Task<Result<IEnumerable<Packaging>>> SearchPackagingsAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllPackagingsAsync();

                var packagings = await _unitOfWork.Packaging.SearchAsync(keyword);
                return Result<IEnumerable<Packaging>>.Ok(packagings);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Packaging>>(ex, "tìm kiếm bao bì");
            }
        }

        public async Task<Result<int>> CreatePackagingAsync(Packaging packaging)
        {
            try
            {
                // Validate dữ liệu
                var validationResult = await ValidatePackagingAsync(packaging);
                if (!validationResult.Success)
                    return Result<int>.Fail(validationResult.Message);

                // Kiểm tra barcode trùng
                var barcodeExistsResult = await IsBarcodeExistsAsync(packaging.Barcode);
                if (barcodeExistsResult.Success && barcodeExistsResult.Data)
                    return BusinessError<int>($"Barcode '{packaging.Barcode}' đã tồn tại trong hệ thống");

                // Tạo bao bì mới
                var packagingId = await _unitOfWork.Packaging.AddAsync(packaging);
                if (packagingId <= 0)
                    return BusinessError<int>("Không thể tạo bao bì mới");

                return Result<int>.Ok(packagingId, "Đã tạo bao bì mới thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo bao bì mới");
            }
        }

        public async Task<Result<bool>> UpdatePackagingAsync(Packaging packaging)
        {
            try
            {
                if (packaging == null || packaging.PackagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packaging?.PackagingId ?? 0);

                // Validate dữ liệu
                var validationResult = await ValidatePackagingAsync(packaging);
                if (!validationResult.Success)
                    return Result<bool>.Fail(validationResult.Message);

                // Kiểm tra bao bì tồn tại
                var existingPackaging = await _unitOfWork.Packaging.GetByIdAsync(packaging.PackagingId);
                if (existingPackaging == null)
                    return NotFoundError<bool>("Bao bì", packaging.PackagingId);

                // Kiểm tra barcode trùng (trừ chính nó)
                var barcodeExistsResult = await IsBarcodeExistsAsync(packaging.Barcode, packaging.PackagingId);
                if (barcodeExistsResult.Success && barcodeExistsResult.Data)
                    return BusinessError<bool>($"Barcode '{packaging.Barcode}' đã được sử dụng bởi bao bì khác");

                // Cập nhật bao bì
                var success = await _unitOfWork.Packaging.UpdateAsync(packaging);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật thông tin bao bì");

                return Result<bool>.Ok(true, "Đã cập nhật thông tin bao bì thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật bao bì");
            }
        }

        public async Task<Result<bool>> DeletePackagingAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                // Kiểm tra bao bì tồn tại
                var packaging = await _unitOfWork.Packaging.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<bool>("Bao bì", packagingId);

                // TODO: Kiểm tra xem bao bì có đang được sử dụng không
                // (có thể kiểm tra trong PackagingTransactions, PackagingInventory, etc.)

                // Xóa bao bì
                var success = await _unitOfWork.Packaging.DeleteAsync(packagingId);
                if (!success)
                    return BusinessError<bool>("Không thể xóa bao bì");

                return Result<bool>.Ok(true, "Đã xóa bao bì thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa bao bì");
            }
        }

        public async Task<Result<bool>> UpdateDepositPriceAsync(int packagingId, decimal newPrice)
        {
            try
            {
                if (packagingId <= 0)
                    return NotFoundError<bool>("Bao bì", packagingId);

                // Validate giá
                var priceErrors = ValidationHelper.ValidatePrice(newPrice);
                if (priceErrors.Any())
                    return ValidationError<bool>(priceErrors);

                // Kiểm tra bao bì tồn tại
                var packaging = await _unitOfWork.Packaging.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<bool>("Bao bì", packagingId);

                var success = await _unitOfWork.Packaging.UpdateDepositPriceAsync(packagingId, newPrice);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật giá ký quỹ");

                return Result<bool>.Ok(true, $"Đã cập nhật giá ký quỹ thành {newPrice:N0} VND");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật giá ký quỹ");
            }
        }

        public async Task<Result<bool>> ValidatePackagingAsync(Packaging packaging)
        {
            try
            {
                if (packaging == null)
                    return BusinessError<bool>("Thông tin bao bì không được để trống");

                var validationErrors = ValidationHelper.ValidatePackaging(packaging);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                return Result<bool>.Ok(true, "Bao bì hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate bao bì");
            }
        }

        public async Task<Result<bool>> IsBarcodeExistsAsync(string barcode, int? excludePackagingId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                    return BusinessError<bool>("Barcode không được để trống");

                var exists = await _unitOfWork.Packaging.IsBarcodeExistsAsync(barcode, excludePackagingId);
                return Result<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra barcode tồn tại");
            }
        }
    }
}
