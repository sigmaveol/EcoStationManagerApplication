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
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
            : base("ProductService")
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Product>> GetProductByIdAsync(int productId)
        {
            try
            {
                if (productId <= 0)
                    return Result<Product>.Fail("ID sản phẩm không hợp lệ");

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<Product>("Sản phẩm", productId);

                return Result<Product>.Ok(product, "Lấy thông tin sản phẩm thành công");
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
                    return Result<Product>.Fail("SKU không được để trống");

                var product = await _productRepository.GetBySkuAsync(sku);
                if (product == null)
                    return NotFoundError<Product>($"Không tìm thấy sản phẩm với SKU: {sku}");

                return Result<Product>.Ok(product, "Lấy thông tin sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<Product>(ex, "lấy thông tin sản phẩm theo SKU");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetAllActiveProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetActiveProductsAsync();
                return Result<IEnumerable<Product>>.Ok(products, "Lấy danh sách sản phẩm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy danh sách sản phẩm");
            }
        }

        public async Task<Result<IEnumerable<Product>>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return Result<IEnumerable<Product>>.Fail("ID danh mục không hợp lệ");

                // Kiểm tra danh mục tồn tại
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                    return NotFoundError<IEnumerable<Product>>("Danh mục", categoryId);

                var products = await _productRepository.GetByCategoryAsync(categoryId);
                return Result<IEnumerable<Product>>.Ok(products, "Lấy sản phẩm theo danh mục thành công");
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
                    return Result<IEnumerable<Product>>.Fail("Loại sản phẩm không được để trống");

                var products = await _productRepository.GetByTypeAsync(productType);
                return Result<IEnumerable<Product>>.Ok(products, "Lấy sản phẩm theo loại thành công");
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
                var products = await _productRepository.GetLowStockProductsAsync();
                var message = products.Any()
                    ? $"Có {products.Count()} sản phẩm sắp hết hàng"
                    : "Không có sản phẩm nào sắp hết hàng";

                return Result<IEnumerable<Product>>.Ok(products, message);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Product>>(ex, "lấy sản phẩm sắp hết hàng");
            }
        }

        public async Task<Result<IEnumerable<Product>>> SearchProductsAsync(string keyword, string productType = null)
        {
            try
            {
                var products = await _productRepository.SearchAsync(keyword, productType);
                var message = products.Any()
                    ? $"Tìm thấy {products.Count()} sản phẩm"
                    : "Không tìm thấy sản phẩm nào";

                return Result<IEnumerable<Product>>.Ok(products, message);
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
                var validationErrors = ValidationHelper.ValidateProduct(product);
                if (validationErrors.Any())
                    return ValidationError<int>(validationErrors);

                // Kiểm tra SKU trùng
                var isSkuExists = await _productRepository.IsSkuExistsAsync(product.SKU);
                if (isSkuExists)
                    return Result<int>.Fail($"SKU '{product.SKU}' đã tồn tại");

                // Kiểm tra danh mục tồn tại
                if (product.CategoryId.HasValue)
                {
                    var category = await _categoryRepository.GetByIdAsync(product.CategoryId.Value);
                    if (category == null)
                        return Result<int>.Fail("Danh mục không tồn tại");
                }

                // Thêm mới
                var productId = await _productRepository.AddAsync(product);
                _logger.Info($"Đã tạo sản phẩm mới: {product.Name} (SKU: {product.SKU}, ID: {productId})");

                return Result<int>.Ok(productId, $"Thêm sản phẩm '{product.Name}' thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "thêm sản phẩm");
            }
        }

        public async Task<Result<bool>> UpdateProductAsync(Product product)
        {
            try
            {
                // Validate dữ liệu
                var validationErrors = ValidationHelper.ValidateProduct(product);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra tồn tại
                var existingProduct = await _productRepository.GetByIdAsync(product.ProductId);
                if (existingProduct == null)
                    return NotFoundError<bool>("Sản phẩm", product.ProductId);

                // Kiểm tra SKU trùng (trừ chính nó)
                var isSkuExists = await _productRepository.IsSkuExistsAsync(product.SKU, product.ProductId);
                if (isSkuExists)
                    return Result<bool>.Fail($"SKU '{product.SKU}' đã tồn tại");

                // Kiểm tra danh mục tồn tại
                if (product.CategoryId.HasValue)
                {
                    var category = await _categoryRepository.GetByIdAsync(product.CategoryId.Value);
                    if (category == null)
                        return Result<bool>.Fail("Danh mục không tồn tại");
                }

                // Cập nhật
                var success = await _productRepository.UpdateAsync(product);
                if (success)
                {
                    _logger.Info($"Đã cập nhật sản phẩm: {product.Name} (SKU: {product.SKU}, ID: {product.ProductId})");
                    return Result<bool>.Ok(true, $"Cập nhật sản phẩm '{product.Name}' thành công");
                }

                return Result<bool>.Fail("Cập nhật sản phẩm thất bại");
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
                    return Result<bool>.Fail("ID sản phẩm không hợp lệ");

                // Kiểm tra tồn tại
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                // Xóa mềm
                var success = await _productRepository.ToggleActiveAsync(productId, false);
                if (success)
                {
                    _logger.Info($"Đã xóa sản phẩm: {product.Name} (SKU: {product.SKU}, ID: {productId})");
                    return Result<bool>.Ok(true, $"Đã xóa sản phẩm '{product.Name}'");
                }

                return Result<bool>.Fail("Xóa sản phẩm thất bại");
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
                    return Result<bool>.Fail("ID sản phẩm không hợp lệ");

                // Kiểm tra tồn tại
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _productRepository.ToggleActiveAsync(productId, isActive);
                var status = isActive ? "kích hoạt" : "vô hiệu hóa";

                if (success)
                {
                    _logger.Info($"Đã {status} sản phẩm: {product.Name} (ID: {productId})");
                    return Result<bool>.Ok(true, $"Đã {status} sản phẩm '{product.Name}'");
                }

                return Result<bool>.Fail($"{status} sản phẩm thất bại");
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
                    return Result<bool>.Fail("ID sản phẩm không hợp lệ");

                if (newPrice < 0)
                    return Result<bool>.Fail("Giá sản phẩm không được âm");

                // Kiểm tra tồn tại
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _productRepository.UpdatePriceAsync(productId, newPrice);
                if (success)
                {
                    _logger.Info($"Đã cập nhật giá sản phẩm: {product.Name} từ {product.Price} thành {newPrice}");
                    return Result<bool>.Ok(true, $"Đã cập nhật giá sản phẩm '{product.Name}' thành {newPrice:N0} VNĐ");
                }

                return Result<bool>.Fail("Cập nhật giá sản phẩm thất bại");
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
                    return Result<bool>.Fail("ID sản phẩm không hợp lệ");

                if (minStockLevel < 0)
                    return Result<bool>.Fail("Mức tồn kho tối thiểu không được âm");

                // Kiểm tra tồn tại
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return NotFoundError<bool>("Sản phẩm", productId);

                var success = await _productRepository.UpdateMinStockLevelAsync(productId, minStockLevel);
                if (success)
                {
                    _logger.Info($"Đã cập nhật mức tồn kho tối thiểu sản phẩm: {product.Name} thành {minStockLevel}");
                    return Result<bool>.Ok(true, $"Đã cập nhật mức tồn kho tối thiểu cho '{product.Name}'");
                }

                return Result<bool>.Fail("Cập nhật mức tồn kho tối thiểu thất bại");
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
                var validationErrors = ValidationHelper.ValidateProduct(product);
                if (validationErrors.Any())
                    return ValidationError<bool>(validationErrors);

                // Kiểm tra SKU trùng
                var isSkuExists = await _productRepository.IsSkuExistsAsync(product.SKU, product.ProductId);
                if (isSkuExists)
                    return Result<bool>.Fail($"SKU '{product.SKU}' đã tồn tại");

                // Kiểm tra danh mục tồn tại
                if (product.CategoryId.HasValue)
                {
                    var category = await _categoryRepository.GetByIdAsync(product.CategoryId.Value);
                    if (category == null)
                        return Result<bool>.Fail("Danh mục không tồn tại");
                }

                return Result<bool>.Ok(true, "Dữ liệu sản phẩm hợp lệ");
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
                if (string.IsNullOrWhiteSpace(sku))
                    return Result<bool>.Fail("SKU không được để trống");

                var exists = await _productRepository.IsSkuExistsAsync(sku, excludeProductId);
                var message = exists ? "SKU đã tồn tại" : "SKU có thể sử dụng";

                return Result<bool>.Ok(exists, message);
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "kiểm tra SKU");
            }
        }
    }
}
