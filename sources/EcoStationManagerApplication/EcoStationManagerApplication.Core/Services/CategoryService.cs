using EcoStationManagerApplication.Common.Logging;
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
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
            : base("CategoryService")
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Category>> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return Result<Category>.Fail("ID danh mục không hợp lệ");

                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<Category>("Danh mục", categoryId);

                return Result<Category>.Ok(category, "Lấy thông tin danh mục thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Category>(ex, "lấy thông tin danh mục");
            }
        }

        public async Task<Result<IEnumerable<Category>>> GetAllActiveCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetActiveCategoriesAsync();
                return Result<IEnumerable<Category>>.Ok(categories, "Lấy danh sách danh mục thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Category>>(ex, "lấy danh sách danh mục");
            }
        }

        public async Task<Result<IEnumerable<Category>>> GetCategoriesByTypeAsync(string categoryType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryType))
                    return Result<IEnumerable<Category>>.Fail("Loại danh mục không được để trống");

                var categories = await _categoryRepository.GetByTypeAsync(categoryType);
                return Result<IEnumerable<Category>>.Ok(categories, "Lấy danh mục theo loại thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Category>>(ex, "lấy danh mục theo loại");
            }
        }

        public async Task<Result<IEnumerable<Category>>> SearchCategoriesAsync(string keyword)
        {
            try
            {
                var categories = await _categoryRepository.SearchAsync(keyword);
                return Result<IEnumerable<Category>>.Ok(categories, "Tìm kiếm danh mục thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Category>>(ex, "tìm kiếm danh mục");
            }
        }

        public async Task<Result<int>> CreateCategoryAsync(Category category)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateCategory(category);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                // Kiểm tra trùng tên
                var isNameExists = await _categoryRepository.IsNameExistsAsync(category.Name);
                if (isNameExists)
                    return Result<int>.Fail($"Tên danh mục '{category.Name}' đã tồn tại");

                // Thêm mới
                var categoryId = await _categoryRepository.AddAsync(category);
                _logger.Info($"Đã tạo danh mục mới: {category.Name} (ID: {categoryId})");

                return Result<int>.Ok(categoryId, $"Thêm danh mục '{category.Name}' thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm danh mục");
            }
        }

        public async Task<Result<bool>> UpdateCategoryAsync(Category category)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateCategory(category);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra tồn tại
                var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);
                if (existingCategory == null)
                    return NotFoundError<bool>("Danh mục", category.CategoryId);

                // Kiểm tra trùng tên (trừ chính nó)
                var isNameExists = await _categoryRepository.IsNameExistsAsync(category.Name, category.CategoryId);
                if (isNameExists)
                    return Result<bool>.Fail($"Tên danh mục '{category.Name}' đã tồn tại");

                // Cập nhật
                var success = await _categoryRepository.UpdateAsync(category);
                if (success)
                {
                    _logger.Info($"Đã cập nhật danh mục: {category.Name} (ID: {category.CategoryId})");
                    return Result<bool>.Ok(true, $"Cập nhật danh mục '{category.Name}' thành công");
                }

                return Result<bool>.Fail("Cập nhật danh mục thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật danh mục");
            }
        }

        public async Task<Result<bool>> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return Result<bool>.Fail("ID danh mục không hợp lệ");

                // Kiểm tra tồn tại
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<bool>("Danh mục", categoryId);

                // Kiểm tra có sản phẩm nào đang sử dụng không
                var productCount = await _categoryRepository.CountProductsInCategoryAsync(categoryId);
                if (productCount > 0)
                    return Result<bool>.Fail($"Không thể xóa danh mục vì có {productCount} sản phẩm đang sử dụng");

                // Xóa mềm
                var success = await _categoryRepository.ToggleActiveAsync(categoryId, false);
                if (success)
                {
                    _logger.Info($"Đã xóa danh mục: {category.Name} (ID: {categoryId})");
                    return Result<bool>.Ok(true, $"Đã xóa danh mục '{category.Name}'");
                }

                return Result<bool>.Fail("Xóa danh mục thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa danh mục");
            }
        }

        public async Task<Result<bool>> ToggleCategoryStatusAsync(int categoryId, bool isActive)
        {
            try
            {
                if (categoryId <= 0)
                    return Result<bool>.Fail("ID danh mục không hợp lệ");

                // Kiểm tra tồn tại
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<bool>("Danh mục", categoryId);

                var success = await _categoryRepository.ToggleActiveAsync(categoryId, isActive);
                var status = isActive ? "kích hoạt" : "vô hiệu hóa";

                if (success)
                {
                    _logger.Info($"Đã {status} danh mục: {category.Name} (ID: {categoryId})");
                    return Result<bool>.Ok(true, $"Đã {status} danh mục '{category.Name}'");
                }

                return Result<bool>.Fail($"{status} danh mục thất bại");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái danh mục");
            }
        }

        public async Task<Result<bool>> CanDeleteCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return Result<bool>.Fail("ID danh mục không hợp lệ");

                var productCount = await _categoryRepository.CountProductsInCategoryAsync(categoryId);
                return Result<bool>.Ok(productCount == 0,
                    productCount == 0 ? "Có thể xóa danh mục" : $"Không thể xóa - có {productCount} sản phẩm");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra khả năng xóa danh mục");
            }
        }

        public async Task<Result<bool>> ValidateCategoryAsync(Category category)
        {
            try
            {
                var validationErrors = ValidationHelper.ValidateCategory(category);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra trùng tên
                var isNameExists = await _categoryRepository.IsNameExistsAsync(category.Name, category.CategoryId);
                if (isNameExists)
                    return Result<bool>.Fail($"Tên danh mục '{category.Name}' đã tồn tại");

                return Result<bool>.Ok(true, "Dữ liệu danh mục hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate danh mục");
            }
        }
    }

}
