using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<Category>> GetCategoryByIdAsync(int categoryId);
        Task<Result<List<Category>>> GetAllCategoriesAsync();
        Task<Result<List<Category>>> GetActiveCategoriesAsync();
        Task<Result<List<Category>>> GetCategoriesByTypeAsync(CategoryType? categoryType);
        Task<Result<Category>> CreateCategoryAsync(Category category);
        Task<Result<Category>> UpdateCategoryAsync(Category category);
        Task<Result<bool>> DeleteCategoryAsync(int categoryId);
        Task<Result<bool>> ToggleCategoryStatusAsync(int categoryId, bool isActive);
        Task<Result<List<Category>>> SearchCategoriesAsync(string keyword);
        Task<Result<bool>> ValidateCategoryAsync(Category category);
        Task<Result<bool>> IsCategoryNameExistsAsync(string name, int? excludeCategoryId = null);
    }

    public interface IProductService
    {
        /// <summary>
        /// Lấy sản phẩm theo ID
        /// </summary>
        Task<Result<Product>> GetProductByIdAsync(int productId);

        /// <summary>
        /// Lấy sản phẩm theo SKU
        /// </summary>
        Task<Result<Product>> GetProductBySkuAsync(string sku);

        /// <summary>
        /// Lấy tất cả sản phẩm active
        /// </summary>
        Task<Result<IEnumerable<Product>>> GetAllActiveProductsAsync();

        Task<Result<IEnumerable<Product>>> GetAllProductsAsync();

        /// <summary>
        /// Lấy sản phẩm theo danh mục
        /// </summary>
        Task<Result<IEnumerable<Product>>> GetProductsByCategoryAsync(int categoryId);

        /// <summary>
        /// Lấy sản phẩm theo loại
        /// </summary>
        Task<Result<IEnumerable<Product>>> GetProductsByTypeAsync(string productType);

        /// <summary>
        /// Lấy sản phẩm sắp hết hàng
        /// </summary>
        Task<Result<IEnumerable<Product>>> GetLowStockProductsAsync();

        /// <summary>
        /// Tìm kiếm sản phẩm
        /// </summary>
        Task<Result<IEnumerable<Product>>> SearchProductsAsync(string keyword, string productType = null);

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        Task<Result<int>> CreateProductAsync(Product product);

        /// <summary>
        /// Cập nhật sản phẩm
        /// </summary>
        Task<Result<bool>> UpdateProductAsync(Product product);

        /// <summary>
        /// Xóa mềm sản phẩm
        /// </summary>
        Task<Result<bool>> DeleteProductAsync(int productId);

        /// <summary>
        /// Bật/tắt trạng thái sản phẩm
        /// </summary>
        Task<Result<bool>> ToggleProductStatusAsync(int productId, bool isActive);

        /// <summary>
        /// Cập nhật giá sản phẩm
        /// </summary>
        Task<Result<bool>> UpdateProductPriceAsync(int productId, decimal newPrice);

        /// <summary>
        /// Cập nhật mức tồn kho tối thiểu
        /// </summary>
        Task<Result<bool>> UpdateMinStockLevelAsync(int productId, decimal minStockLevel);

        /// <summary>
        /// Validate sản phẩm trước khi lưu
        /// </summary>
        Task<Result<bool>> ValidateProductAsync(Product product);

        /// <summary>
        /// Kiểm tra SKU đã tồn tại chưa
        /// </summary>
        Task<Result<bool>> IsSkuExistsAsync(string sku, int? excludeProductId = null);
    }

    public interface IPackagingService
    {
        /// <summary>
        /// Lấy bao bì theo ID
        /// </summary>
        Task<Result<Packaging>> GetPackagingByIdAsync(int packagingId);

        /// <summary>
        /// Lấy bao bì theo barcode
        /// </summary>
        Task<Result<Packaging>> GetPackagingByBarcodeAsync(string barcode);

        /// <summary>
        /// Lấy tất cả bao bì
        /// </summary>
        Task<Result<IEnumerable<Packaging>>> GetAllPackagingsAsync();

        /// <summary>
        /// Lấy bao bì theo loại
        /// </summary>
        Task<Result<IEnumerable<Packaging>>> GetPackagingsByTypeAsync(string type);

        /// <summary>
        /// Tìm kiếm bao bì
        /// </summary>
        Task<Result<IEnumerable<Packaging>>> SearchPackagingsAsync(string keyword);

        /// <summary>
        /// Thêm bao bì mới
        /// </summary>
        Task<Result<int>> CreatePackagingAsync(Packaging packaging);

        /// <summary>
        /// Cập nhật bao bì
        /// </summary>
        Task<Result<bool>> UpdatePackagingAsync(Packaging packaging);

        /// <summary>
        /// Xóa bao bì
        /// </summary>
        Task<Result<bool>> DeletePackagingAsync(int packagingId);

        /// <summary>
        /// Cập nhật giá ký quỹ
        /// </summary>
        Task<Result<bool>> UpdateDepositPriceAsync(int packagingId, decimal newPrice);

        /// <summary>
        /// Validate bao bì trước khi lưu
        /// </summary>
        Task<Result<bool>> ValidatePackagingAsync(Packaging packaging);

        /// <summary>
        /// Kiểm tra barcode đã tồn tại chưa
        /// </summary>
        Task<Result<bool>> IsBarcodeExistsAsync(string barcode, int? excludePackagingId = null);
    }

}
