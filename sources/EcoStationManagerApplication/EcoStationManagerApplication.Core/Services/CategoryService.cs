using EcoStationManagerApplication.Common.Logging;
using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
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
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
            : base("CategoryService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Category>> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return NotFoundError<Category>("Danh mục", categoryId);

                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<Category>("Danh mục", categoryId);

                return Result<Category>.Ok(category);
            }
            catch (Exception ex)
            {
                return HandleException<Category>(ex, "lấy thông tin danh mục");
            }
        }

        public async Task<Result<List<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                return Result<List<Category>>.Ok(categories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Category>>(ex, "lấy danh sách danh mục");
            }
        }

        public async Task<Result<List<Category>>> GetActiveCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
                return Result<List<Category>>.Ok(categories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Category>>(ex, "lấy danh sách danh mục đang hoạt động");
            }
        }

        public async Task<Result<List<Category>>> GetCategoriesByTypeAsync(CategoryType? categoryType)
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetByTypeAsync(categoryType);
                return Result<List<Category>>.Ok(categories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Category>>(ex, "lấy danh mục theo loại");
            }
        }

        public async Task<Result<Category>> CreateCategoryAsync(Category category)
        {
            try
            {
                // Validate dữ liệu
                var validationResult = await ValidateCategoryAsync(category);
                if (!validationResult.Success)
                    return Result<Category>.Fail(validationResult.Message);

                // Kiểm tra tên danh mục trùng
                var nameExistsResult = await IsCategoryNameExistsAsync(category.Name);
                if (nameExistsResult.Success && nameExistsResult.Data)
                    return BusinessError<Category>($"Tên danh mục '{category.Name}' đã tồn tại");

                // Set default values
                category.IsActive = ActiveStatus.ACTIVE;
                category.CreatedDate = DateTime.Now;

                // Tạo danh mục mới
                var categoryId = await _unitOfWork.Categories.AddAsync(category);
                if (categoryId <= 0)
                    return BusinessError<Category>("Không thể tạo danh mục mới");

                // Lấy thông tin danh mục vừa tạo
                var createdCategory = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                return Result<Category>.Ok(createdCategory, "Đã tạo danh mục mới thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Category>(ex, "tạo danh mục mới");
            }
        }

        public async Task<Result<Category>> UpdateCategoryAsync(Category category)
        {
            try
            {
                if (category == null || category.CategoryId <= 0)
                    return NotFoundError<Category>("Danh mục", category?.CategoryId ?? 0);

                // Validate dữ liệu
                var validationResult = await ValidateCategoryAsync(category);
                if (!validationResult.Success)
                    return Result<Category>.Fail(validationResult.Message);

                // Kiểm tra danh mục tồn tại
                var existingCategory = await _unitOfWork.Categories.GetByIdAsync(category.CategoryId);
                if (existingCategory == null)
                    return NotFoundError<Category>("Danh mục", category.CategoryId);

                // Kiểm tra tên danh mục trùng (trừ chính nó)
                var nameExistsResult = await IsCategoryNameExistsAsync(category.Name, category.CategoryId);
                if (nameExistsResult.Success && nameExistsResult.Data)
                    return BusinessError<Category>($"Tên danh mục '{category.Name}' đã được sử dụng bởi danh mục khác");

                // Cập nhật danh mục
                var success = await _unitOfWork.Categories.UpdateAsync(category);
                if (!success)
                    return BusinessError<Category>("Không thể cập nhật thông tin danh mục");

                // Lấy thông tin danh mục đã cập nhật
                var updatedCategory = await _unitOfWork.Categories.GetByIdAsync(category.CategoryId);
                return Result<Category>.Ok(updatedCategory, "Đã cập nhật thông tin danh mục thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Category>(ex, "cập nhật danh mục");
            }
        }

        public async Task<Result<bool>> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return NotFoundError<bool>("Danh mục", categoryId);

                // Kiểm tra danh mục tồn tại
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<bool>("Danh mục", categoryId);

                // Kiểm tra xem danh mục có sản phẩm không
                var productCount = await _unitOfWork.Categories.CountProductsInCategoryAsync(categoryId);
                if (productCount > 0)
                    return BusinessError<bool>($"Không thể xóa danh mục vì có {productCount} sản phẩm đang sử dụng");

                // Xóa mềm danh mục
                var success = await _unitOfWork.Categories.ToggleActiveAsync(categoryId, false);
                if (!success)
                    return BusinessError<bool>("Không thể xóa danh mục");

                return Result<bool>.Ok(true, "Đã xóa danh mục thành công");
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
                    return NotFoundError<bool>("Danh mục", categoryId);

                // Kiểm tra danh mục tồn tại
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<bool>("Danh mục", categoryId);

                var success = await _unitOfWork.Categories.ToggleActiveAsync(categoryId, isActive);
                if (!success)
                    return BusinessError<bool>("Không thể thay đổi trạng thái danh mục");

                var statusText = isActive ? "kích hoạt" : "vô hiệu hóa";
                return Result<bool>.Ok(true, $"Đã {statusText} danh mục thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái danh mục");
            }
        }

        public async Task<Result<List<Category>>> SearchCategoriesAsync(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetActiveCategoriesAsync();

                var categories = await _unitOfWork.Categories.SearchAsync(keyword);
                return Result<List<Category>>.Ok(categories.ToList());
            }
            catch (Exception ex)
            {
                return HandleException<List<Category>>(ex, "tìm kiếm danh mục");
            }
        }

        public async Task<Result<bool>> ValidateCategoryAsync(Category category)
        {
            try
            {
                if (category == null)
                    return BusinessError<bool>("Thông tin danh mục không được để trống");

                var validationErrors = ValidationHelper.ValidateCategory(category);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra loại danh mục hợp lệ
                if (!Enum.IsDefined(typeof(CategoryType), category.CategoryType))
                    return BusinessError<bool>("Loại danh mục không hợp lệ");

                return Result<bool>.Ok(true, "Danh mục hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate danh mục");
            }
        }

        public async Task<Result<bool>> IsCategoryNameExistsAsync(string name, int? excludeCategoryId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BusinessError<bool>("Tên danh mục không được để trống");

                var exists = await _unitOfWork.Categories.IsNameExistsAsync(name, excludeCategoryId);
                return Result<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra tên danh mục tồn tại");
            }
        }
    }

}
