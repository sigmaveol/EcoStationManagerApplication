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
        private readonly IPackagingRepository _packagingRepository;

        public PackagingService(IPackagingRepository packagingRepository)
            : base("PackagingService")
        {
            _packagingRepository = packagingRepository;
        }

        public async Task<Result<Packaging>> GetPackagingByIdAsync(int packagingId)
        {
            try
            {
                if (packagingId <= 0)
                    return Result<Packaging>.Fail("ID bao bì không hợp lệ");

                var packaging = await _packagingRepository.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<Packaging>("Bao bì", packagingId);

                return Result<Packaging>.Ok(packaging, "Lấy thông tin bao bì thành công");
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
                    return Result<Packaging>.Fail("Barcode không được để trống");

                var packaging = await _packagingRepository.GetByBarcodeAsync(barcode);
                if (packaging == null)
                    return NotFoundError<Packaging>($"Không tìm thấy bao bì với barcode: {barcode}");

                return Result<Packaging>.Ok(packaging, "Lấy thông tin bao bì thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Packaging>(ex, "lấy thông tin bao bì theo barcode");
            }
        }

        public async Task<Result<IEnumerable<Packaging>>> GetAllPackagingsAsync()
        {
            try
            {
                var packagings = await _packagingRepository.GetAllAsync();
                return Result<IEnumerable<Packaging>>.Ok(packagings, "Lấy danh sách bao bì thành công");
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
                    return Result<IEnumerable<Packaging>>.Fail("Loại bao bì không được để trống");

                var packagings = await _packagingRepository.GetByTypeAsync(type);
                return Result<IEnumerable<Packaging>>.Ok(packagings, "Lấy bao bì theo loại thành công");
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
                var packagings = await _packagingRepository.SearchAsync(keyword);
                var message = packagings.Any()
                    ? $"Tìm thấy {packagings.Count()} bao bì"
                    : "Không tìm thấy bao bì nào";

                return Result<IEnumerable<Packaging>>.Ok(packagings, message);
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
                var validationErrors = ValidationHelper.ValidatePackaging(packaging);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                // Kiểm tra barcode trùng
                var isBarcodeExists = await _packagingRepository.IsBarcodeExistsAsync(packaging.Barcode);
                if (isBarcodeExists)
                    return Result<int>.Fail($"Barcode '{packaging.Barcode}' đã tồn tại");

                // Thêm mới
                var packagingId = await _packagingRepository.AddAsync(packaging);
                _logger.Info($"Đã tạo bao bì mới: {packaging.Name} (Barcode: {packaging.Barcode}, ID: {packagingId})");

                return Result<int>.Ok(packagingId, $"Thêm bao bì '{packaging.Name}' thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm bao bì");
            }
        }

        public async Task<Result<bool>> UpdatePackagingAsync(Packaging packaging)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidatePackaging(packaging);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra tồn tại
                var existingPackaging = await _packagingRepository.GetByIdAsync(packaging.PackagingId);
                if (existingPackaging == null)
                    return NotFoundError<bool>("Bao bì", packaging.PackagingId);

                // Kiểm tra barcode trùng (trừ chính nó)
                var isBarcodeExists = await _packagingRepository.IsBarcodeExistsAsync(packaging.Barcode, packaging.PackagingId);
                if (isBarcodeExists)
                    return Result<bool>.Fail($"Barcode '{packaging.Barcode}' đã tồn tại");

                // Cập nhật
                var success = await _packagingRepository.UpdateAsync(packaging);
                if (success)
                {
                    _logger.Info($"Đã cập nhật bao bì: {packaging.Name} (Barcode: {packaging.Barcode}, ID: {packaging.PackagingId})");
                    return Result<bool>.Ok(true, $"Cập nhật bao bì '{packaging.Name}' thành công");
                }

                return Result<bool>.Fail("Cập nhật bao bì thất bại");
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
                    return Result<bool>.Fail("ID bao bì không hợp lệ");

                // Kiểm tra tồn tại
                var packaging = await _packagingRepository.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<bool>("Bao bì", packagingId);

                // TODO: Kiểm tra có đang được sử dụng không (trong inventory, transactions)

                // Xóa cứng (vì không có soft delete)
                var success = await _packagingRepository.DeleteAsync(packagingId);
                if (success)
                {
                    _logger.Info($"Đã xóa bao bì: {packaging.Name} (Barcode: {packaging.Barcode}, ID: {packagingId})");
                    return Result<bool>.Ok(true, $"Đã xóa bao bì '{packaging.Name}'");
                }

                return Result<bool>.Fail("Xóa bao bì thất bại");
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
                    return Result<bool>.Fail("ID bao bì không hợp lệ");

                if (newPrice < 0)
                    return Result<bool>.Fail("Giá ký quỹ không được âm");

                // Kiểm tra tồn tại
                var packaging = await _packagingRepository.GetByIdAsync(packagingId);
                if (packaging == null)
                    return NotFoundError<bool>("Bao bì", packagingId);

                var success = await _packagingRepository.UpdateDepositPriceAsync(packagingId, newPrice);
                if (success)
                {
                    _logger.Info($"Đã cập nhật giá ký quỹ bao bì: {packaging.Name} từ {packaging.DepositPrice} thành {newPrice}");
                    return Result<bool>.Ok(true, $"Đã cập nhật giá ký quỹ bao bì '{packaging.Name}' thành {newPrice:N0} VNĐ");
                }

                return Result<bool>.Fail("Cập nhật giá ký quỹ thất bại");
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
                var validationErrors = ValidationHelper.ValidatePackaging(packaging);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra barcode trùng
                var isBarcodeExists = await _packagingRepository.IsBarcodeExistsAsync(packaging.Barcode, packaging.PackagingId);
                if (isBarcodeExists)
                    return Result<bool>.Fail($"Barcode '{packaging.Barcode}' đã tồn tại");

                return Result<bool>.Ok(true, "Dữ liệu bao bì hợp lệ");
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
                    return Result<bool>.Fail("Barcode không được để trống");

                var exists = await _packagingRepository.IsBarcodeExistsAsync(barcode, excludePackagingId);
                var message = exists ? "Barcode đã tồn tại" : "Barcode có thể sử dụng";

                return Result<bool>.Ok(exists, message);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra barcode");
            }
        }
    }
}
