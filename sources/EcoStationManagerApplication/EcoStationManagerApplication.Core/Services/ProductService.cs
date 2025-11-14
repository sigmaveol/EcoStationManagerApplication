using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.UnitOfWork;
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
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;

        public ProductService(IUnitOfWork unitOfWork, ICategoryService categoryService)
            : base("ProductService")
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public async Task<Result<Product>> GetProductByIdAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<Product>("Sản phẩm", productId);

                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<Product>("Sản phẩm", productId);

                return Result<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                return HandleException<Product>(ex, "lấy thông tin sản phẩm");
            }
        }

        public async Task<Result<Product>> GetProductBySkuAsync(string sku)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sku))
                    return BusinessError<Product>("SKU không được để trống");

                var product = await _unitOfWork.Products.GetBySkuAsync(sku);
                if (product == null)
                    return NotFoundError<Product>($"Không tìm thấy sản phẩm với SKU: {sku}");

                return Result<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                return HandleException<Product>(ex, "lấy sản phẩm theo SKU");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllActiveProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetActiveProductsAsync();
                return Result<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy danh sách sản phẩm đang hoạt động");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllAsync();
                return Result<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy tất cả danh sách sản phẩm");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return NotFoundError<IEnumerable<Product>>("Danh mục", categoryId);

                // Kiểm tra danh mục tồn tại
                var categoryResult = await _categoryService.GetCategoryByIdAsync(categoryId);
                if (!categoryResult.Success)
                    return Result<IEnumerable<Product>>.Fail(categoryResult.Message);

                var products = await _unitOfWork.Products.GetByCategoryAsync(categoryId);
                return Result<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy sản phẩm theo danh mục");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetProductsByTypeAsync(string productType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productType))
                    return BusinessError<IEnumerable<Product>>("Loại sản phẩm không được để trống");

                // Validate product type
                if (!Enum.TryParse<ProductType>(productType, true, out var parsedType))
                    return BusinessError<IEnumerable<Product>>($"Loại sản phẩm không hợp lệ: {productType}");

                var products = await _unitOfWork.Products.GetByTypeAsync(parsedType);
                return Result<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy sản phẩm theo loại");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetLowStockProductsAsync()
        {
            try
            {
                var lowStockProducts = await _unitOfWork.Products.GetLowStockProductsAsync();
                return Result<IEnumerable<Product>>.Ok(lowStockProducts);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy danh sách sản phẩm sắp hết hàng");
            }
        }

        public async Task<Result<IEnumerable<Product>>> SearchProductsAsync(string keyword, string productType = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return await GetAllActiveProductsAsync();

                ProductType? parsedType = null;
                if (!string.IsNullOrWhiteSpace(productType))
                {
                    if (!Enum.TryParse<ProductType>(productType, true, out var tempType))
                        return BusinessError<IEnumerable<Product>>($"Loại sản phẩm không hợp lệ: {productType}");

                    parsedType = tempType;
                }

                var products = await _unitOfWork.Products.SearchAsync(keyword, parsedType);
                return Result<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "tìm kiếm sản phẩm");
            }
        }

        public async Task<Result<int>> CreateProductAsync(Product product)
        {
            try
            {
                // Validate dữ liệu
                var validationResult = await ValidateProductAsync(product);
                if (!validationResult.Success)
                    return Result<int>.Fail(validationResult.Message);

                // Kiểm tra SKU trùng (chỉ khi SKU không null)
                if (!string.IsNullOrWhiteSpace(product.Sku))
                {
                    var skuExistsResult = await IsSkuExistsAsync(product.Sku);
                    if (skuExistsResult.Success && skuExistsResult.Data)
                        return BusinessError<int>($"SKU '{product.Sku}' đã tồn tại trong hệ thống");
                }

                // Kiểm tra danh mục tồn tại (nếu có)
                if (product.CategoryId.HasValue)
                {
                    var categoryResult = await _categoryService.GetCategoryByIdAsync(product.CategoryId.Value);
                    if (!categoryResult.Success)
                        return Result<int>.Fail($"Danh mục không tồn tại: {categoryResult.Message}");
                }

                // Set default values
                product.IsActive = ActiveStatus.ACTIVE;
                product.CreatedDate = DateTime.Now;

                // Tạo sản phẩm mới
                var productId = await _unitOfWork.Products.AddAsync(product);
                if (productId <= 0)
                    return BusinessError<int>("Không thể tạo sản phẩm mới");

                return Result<int>.Ok(productId, "Đã tạo sản phẩm mới thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo sản phẩm mới");
            }
        }

        public async Task<Result<bool>> UpdateProductAsync(Product product)
        {
            try
            {
                if (product == null || product.ProductId <= 0)
                    return NotFoundError<bool>("Sản phẩm", product?.ProductId ?? 0);

                // Validate dữ liệu
                var validationResult = await ValidateProductAsync(product);
                if (!validationResult.Success)
                    return Result<bool>.Fail(validationResult.Message);

                // Kiểm tra sản phẩm tồn tại
                var existingProduct = await _unitOfWork.Products.GetByIdAsync(product.ProductId);
                if (existingProduct == null)
                    return NotFoundError<bool>("Sản phẩm", product.ProductId);

                // Kiểm tra SKU trùng (trừ chính nó, chỉ khi SKU không null)
                if (!string.IsNullOrWhiteSpace(product.Sku))
                {
                    var skuExistsResult = await IsSkuExistsAsync(product.Sku, product.ProductId);
                    if (skuExistsResult.Success && skuExistsResult.Data)
                        return BusinessError<bool>($"SKU '{product.Sku}' đã được sử dụng bởi sản phẩm khác");
                }

                // Kiểm tra danh mục tồn tại (nếu có)
                if (product.CategoryId.HasValue)
                {
                    var categoryResult = await _categoryService.GetCategoryByIdAsync(product.CategoryId.Value);
                    if (!categoryResult.Success)
                        return Result<bool>.Fail($"Danh mục không tồn tại: {categoryResult.Message}");
                }

                // Cập nhật sản phẩm
                var success = await _unitOfWork.Products.UpdateAsync(product);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật thông tin sản phẩm");

                return Result<bool>.Ok(true, "Đã cập nhật thông tin sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật sản phẩm");
            }
        }

        public async Task<Result<bool>> DeleteProductAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                // Kiểm tra sản phẩm tồn tại
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                // Xóa mềm sản phẩm (set IsActive = false)
                var success = await _unitOfWork.Products.ToggleActiveAsync(productId, false);
                if (!success)
                    return BusinessError<bool>("Không thể xóa sản phẩm");

                return Result<bool>.Ok(true, "Đã xóa sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa sản phẩm");
            }
        }

        public async Task<Result<bool>> ToggleProductStatusAsync(int productId, bool isActive)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                // Kiểm tra sản phẩm tồn tại
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _unitOfWork.Products.ToggleActiveAsync(productId, isActive);
                if (!success)
                    return BusinessError<bool>("Không thể thay đổi trạng thái sản phẩm");

                var statusText = isActive ? "kích hoạt" : "vô hiệu hóa";
                return Result<bool>.Ok(true, $"Đã {statusText} sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái sản phẩm");
            }
        }

        public async Task<Result<bool>> UpdateProductPriceAsync(int productId, decimal newPrice)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                // Validate giá
                var priceErrors = ValidationHelper.ValidatePrice(newPrice);
                if (priceErrors.Any())
                    return ValidationError<bool>(priceErrors);

                // Kiểm tra sản phẩm tồn tại
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _unitOfWork.Products.UpdatePriceAsync(productId, newPrice);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật giá sản phẩm");

                return Result<bool>.Ok(true, $"Đã cập nhật giá sản phẩm thành {newPrice:N0} VND");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật giá sản phẩm");
            }
        }

        public async Task<Result<bool>> UpdateMinStockLevelAsync(int productId, decimal minStockLevel)
        {
            try
            {
                if (productId <= 0)
                    return NotFoundError<bool>("Sản phẩm", productId);

                if (minStockLevel < 0)
                    return BusinessError<bool>("Mức tồn kho tối thiểu không được âm");

                // Kiểm tra sản phẩm tồn tại
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _unitOfWork.Products.UpdateMinStockLevelAsync(productId, minStockLevel);
                if (!success)
                    return BusinessError<bool>("Không thể cập nhật mức tồn kho tối thiểu");

                return Result<bool>.Ok(true, $"Đã cập nhật mức tồn kho tối thiểu thành {minStockLevel}");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật mức tồn kho tối thiểu");
            }
        }

        public async Task<Result<bool>> ValidateProductAsync(Product product)
        {
            try
            {
                if (product == null)
                    return BusinessError<bool>("Thông tin sản phẩm không được để trống");

                var validationErrors = ValidationHelper.ValidateProduct(product);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra loại sản phẩm hợp lệ
                if (!Enum.IsDefined(typeof(ProductType), product.ProductType))
                    return BusinessError<bool>("Loại sản phẩm không hợp lệ");

                return Result<bool>.Ok(true, "Sản phẩm hợp lệ");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "validate sản phẩm");
            }
        }

        public async Task<Result<bool>> IsSkuExistsAsync(string sku, int? excludeProductId = null)
        {
            try
            {
                // Nếu SKU null hoặc empty, trả về false (không tồn tại)
                if (string.IsNullOrWhiteSpace(sku))
                    return Result<bool>.Ok(false);

                var exists = await _unitOfWork.Products.IsSkuExistsAsync(sku, excludeProductId);
                return Result<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra SKU tồn tại");
            }
        }
    }
}
