using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Lấy danh mục theo loại (PRODUCT, SERVICE, OTHER)
        /// </summary>
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType? categoryType);

        /// <summary>
        /// Lấy tất cả danh mục đang active
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        /// <summary>
        /// Kiểm tra tên danh mục đã tồn tại chưa
        /// </summary>
        Task<bool> IsNameExistsAsync(string name, int? excludeCategoryId = null);

        /// <summary>
        /// Bật/tắt trạng thái active của danh mục
        /// </summary>
        Task<bool> ToggleActiveAsync(int categoryId, bool isActive);

        /// <summary>
        /// Đếm số sản phẩm trong danh mục
        /// </summary>
        Task<int> CountProductsInCategoryAsync(int categoryId);

        /// <summary>
        /// Tìm kiếm danh mục theo tên hoặc loại danh mục 
        /// </summary>
        Task<IEnumerable<Category>> SearchAsync(string keyword);
    }

    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Lấy sản phẩm theo SKU
        /// </summary>
        Task<Product> GetBySkuAsync(string sku);

        /// <summary>
        /// Lấy sản phẩm theo danh mục
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        /// <summary>
        /// Lấy sản phẩm theo loại (PACKED, REFILLED, OTHER)
        /// </summary>
        Task<IEnumerable<Product>> GetByTypeAsync(ProductType? productType);

        /// <summary>
        /// Lấy tất cả sản phẩm đang active
        /// </summary>
        Task<IEnumerable<Product>> GetActiveProductsAsync();

        /// <summary>
        /// Lấy sản phẩm sắp hết hàng (tồn kho <= mức tối thiểu)
        /// </summary>
        Task<IEnumerable<Product>> GetLowStockProductsAsync();

        /// <summary>
        /// Tìm kiếm sản phẩm theo tên hoặc SKU
        /// </summary>
        Task<IEnumerable<Product>> SearchAsync(string keyword, ProductType? productType = null);

        /// <summary>
        /// Kiểm tra SKU đã tồn tại chưa
        /// </summary>
        Task<bool> IsSkuExistsAsync(string sku, int? excludeProductId = null);

        /// <summary>
        /// Cập nhật giá bán sản phẩm
        /// </summary>
        Task<bool> UpdatePriceAsync(int productId, decimal newPrice);

        /// <summary>
        /// Cập nhật đơn vị sản phẩm
        /// </summary>
        Task<bool> UpdateUnitAsync(int productId, string  unit);

        /// <summary>
        /// Cập nhật mức tồn kho tối thiểu
        /// </summary>
        Task<bool> UpdateMinStockLevelAsync(int productId, decimal minStockLevel);

        /// <summary>
        /// Bật/tắt trạng thái active của sản phẩm
        /// </summary>
        Task<bool> ToggleActiveAsync(int productId, bool isActive);

        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedProductsAsync(
            int pageNumber,
            int pageSize,
            string searchKeyword = null,
            int? categoryId = null,
            string productType = null,
            bool? isActive = null
        );
    }

    public interface IPackagingRepository : IRepository<Packaging>
    { 
        /// <summary>
        /// Lấy bao bì theo barcode
        /// </summary>
        Task<Packaging> GetByBarcodeAsync(string barcode);

        /// <summary>
        /// Lấy bao bì theo loại (bottle, box, container...)
        /// </summary>
        Task<IEnumerable<Packaging>> GetByTypeAsync(string type);

        /// <summary>
        /// Tìm kiếm bao bì theo tên hoặc barcode
        /// </summary>
        Task<IEnumerable<Packaging>> SearchAsync(string keyword);

        /// <summary>
        /// Kiểm tra barcode đã tồn tại chưa
        /// </summary>
        Task<bool> IsBarcodeExistsAsync(string barcode, int? excludePackagingId = null);

        /// <summary>
        /// Cập nhật giá ký quỹ
        /// </summary>
        Task<bool> UpdateDepositPriceAsync(int packagingId, decimal newPrice);

        /// <summary>
        /// Lấy tổng số bao bì
        /// </summary>
        Task<int> GetTotalPackagingCountAsync();

        Task<(IEnumerable<Packaging> Packagings, int TotalCount)> GetPagedPackagingsAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            string type = null
        );
    }

}
