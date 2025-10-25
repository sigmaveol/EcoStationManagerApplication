using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    // ==================== PRODUCT SERVICE IMPLEMENTATION ====================
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductService(IProductRepository productRepository, IVariantRepository variantRepository, IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
            var product = await _productRepository.GetByIdAsync(id);
            return product ?? throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {id}");
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetActiveProductsAsync();
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            ValidateProduct(entity);

            // Check duplicate code
            if (!string.IsNullOrWhiteSpace(entity.Code))
            {
                var existingByCode = await _productRepository.GetProductByCodeAsync(entity.Code);
                if (existingByCode != null)
                    throw new InvalidOperationException($"Mã sản phẩm {entity.Code} đã tồn tại");
            }

            // Set default values
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _productRepository.CreateAsync(entity);
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            if (entity.ProductId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
            ValidateProduct(entity);

            var existing = await _productRepository.GetByIdAsync(entity.ProductId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {entity.ProductId}");

            // Check code uniqueness if changed
            if (!string.IsNullOrWhiteSpace(entity.Code) && existing.Code != entity.Code)
            {
                var productWithCode = await _productRepository.GetProductByCodeAsync(entity.Code);
                if (productWithCode != null)
                    throw new InvalidOperationException($"Mã sản phẩm {entity.Code} đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _productRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {id}");

            // Check if product has variants
            var variants = await _variantRepository.GetVariantsByProductAsync(id);
            if (variants.Any())
                throw new InvalidOperationException("Không thể xóa sản phẩm đã có biến thể");

            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {id}");

            var updatedProduct = new Product
            {
                ProductId = product.ProductId,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                UnitMeasure = product.UnitMeasure,
                ProductType = product.ProductType,
                CategoryId = product.CategoryId,
                IsActive = false,
                CreatedDate = product.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _productRepository.UpdateAsync(updatedProduct);
            return true;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ");
            return await _productRepository.GetProductsByCategoryAsync(categoryId);
        }

        public async Task<Product> GetProductWithVariantsAsync(int productId)
        {
            if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
            return await _productRepository.GetProductWithVariantsAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _productRepository.GetActiveProductsAsync();
        }

        public async Task<Product> GetProductByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Mã sản phẩm không được trống");
            return await _productRepository.GetProductByCodeAsync(code);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                throw new ArgumentException("Từ khóa phải có ít nhất 2 ký tự");
            return await _productRepository.SearchProductsAsync(keyword);
        }

        public async Task<bool> UpdateProductStatusAsync(int productId, bool isActive)
        {
            if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {productId}");

            var updatedProduct = new Product
            {
                ProductId = product.ProductId,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                UnitMeasure = product.UnitMeasure,
                ProductType = product.ProductType,
                CategoryId = product.CategoryId,
                IsActive = isActive,
                CreatedDate = product.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _productRepository.UpdateAsync(updatedProduct);
            return true;
        }

        public async Task<IEnumerable<Product>> GetProductsLowInStockAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _productRepository.GetProductsLowInStockAsync(stationId);
        }

        public async Task<Dictionary<string, object>> GetProductStatisticsAsync(int productId)
        {
            if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {productId}");

            var variants = await _variantRepository.GetVariantsByProductAsync(productId);
            var totalStock = 0m;

            foreach (var variant in variants)
            {
                var stock = await _inventoryRepository.GetCurrentStockAsync(variant.VariantId, 1); // Default station
                totalStock += stock;
            }

            return new Dictionary<string, object>
            {
                ["ProductId"] = productId,
                ["Name"] = product.Name,
                ["Code"] = product.Code,
                ["TotalVariants"] = variants.Count(),
                ["TotalStock"] = totalStock,
                ["IsActive"] = product.IsActive,
                ["BasePrice"] = product.BasePrice,
                ["CategoryId"] = product.CategoryId
            };
        }

        public async Task<Product> CreateProductWithVariantsAsync(Product product, List<Variant> variants)
        {
            ValidateProduct(product);

            // Create product
            product.IsActive = true;
            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;

            var createdProduct = await _productRepository.CreateAsync(product);

            // Create variants
            foreach (var variant in variants)
            {
                variant.ProductId = createdProduct.ProductId;
                variant.IsActive = true;
                variant.CreatedDate = DateTime.Now;
                variant.UpdatedDate = DateTime.Now;
                await _variantRepository.CreateAsync(variant);
            }

            return createdProduct;
        }

        private void ValidateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("Tên sản phẩm không được trống");
            if (string.IsNullOrWhiteSpace(product.UnitMeasure)) throw new ArgumentException("Đơn vị tính không được trống");
            if (product.Name.Length > 255) throw new ArgumentException("Tên sản phẩm không được vượt quá 255 ký tự");
            if (!string.IsNullOrWhiteSpace(product.Code) && product.Code.Length > 30)
                throw new ArgumentException("Mã sản phẩm không được vượt quá 30 ký tự");
        }
    }

    // ==================== CATEGORY SERVICE IMPLEMENTATION ====================
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID danh mục không hợp lệ");
            var category = await _categoryRepository.GetByIdAsync(id);
            return category ?? throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {id}");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            ValidateCategory(entity);

            // Check duplicate name
            var existingByName = await _categoryRepository.GetCategoryByNameAsync(entity.Name);
            if (existingByName != null)
                throw new InvalidOperationException($"Tên danh mục {entity.Name} đã tồn tại");

            // Set default values
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _categoryRepository.CreateAsync(entity);
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            if (entity.CategoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ");
            ValidateCategory(entity);

            var existing = await _categoryRepository.GetByIdAsync(entity.CategoryId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {entity.CategoryId}");

            // Check name uniqueness if changed
            if (existing.Name != entity.Name)
            {
                var categoryWithName = await _categoryRepository.GetCategoryByNameAsync(entity.Name);
                if (categoryWithName != null)
                    throw new InvalidOperationException($"Tên danh mục {entity.Name} đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _categoryRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID danh mục không hợp lệ");

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {id}");

            // Check if category has products
            var products = await _productRepository.GetProductsByCategoryAsync(id);
            if (products.Any())
                throw new InvalidOperationException("Không thể xóa danh mục đã có sản phẩm");

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID danh mục không hợp lệ");

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {id}");

            var updatedCategory = new Category
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Image = category.Image,
                Description = category.Description,
                CategoryType = category.CategoryType,
                ParentId = category.ParentId,
                SortOrder = category.SortOrder,
                IsActive = false,
                CreatedDate = category.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _categoryRepository.UpdateAsync(updatedCategory);
            return true;
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string categoryType)
        {
            if (string.IsNullOrWhiteSpace(categoryType)) throw new ArgumentException("Loại danh mục không được trống");
            return await _categoryRepository.GetCategoriesByTypeAsync(categoryType);
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId)
        {
            if (parentId <= 0) throw new ArgumentException("ID danh mục cha không hợp lệ");
            return await _categoryRepository.GetChildCategoriesAsync(parentId);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
        {
            return await _categoryRepository.GetCategoriesWithProductsAsync();
        }

        public async Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isActive)
        {
            if (categoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null) throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {categoryId}");

            var updatedCategory = new Category
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Image = category.Image,
                Description = category.Description,
                CategoryType = category.CategoryType,
                ParentId = category.ParentId,
                SortOrder = category.SortOrder,
                IsActive = isActive,
                CreatedDate = category.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _categoryRepository.UpdateAsync(updatedCategory);
            return true;
        }

        public async Task<Category> CreateCategoryWithChildrenAsync(Category category, List<Category> children)
        {
            ValidateCategory(category);

            // Create parent category
            category.IsActive = true;
            category.CreatedDate = DateTime.Now;
            category.UpdatedDate = DateTime.Now;

            var createdCategory = await _categoryRepository.CreateAsync(category);

            // Create child categories
            foreach (var child in children)
            {
                child.ParentId = createdCategory.CategoryId;
                child.IsActive = true;
                child.CreatedDate = DateTime.Now;
                child.UpdatedDate = DateTime.Now;
                await _categoryRepository.CreateAsync(child);
            }

            return createdCategory;
        }

        public async Task<Dictionary<string, object>> GetCategoryStatisticsAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null) throw new KeyNotFoundException($"Không tìm thấy danh mục với ID: {categoryId}");

            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(categoryId);

            return new Dictionary<string, object>
            {
                ["CategoryId"] = categoryId,
                ["Name"] = category.Name,
                ["TotalProducts"] = products.Count(),
                ["TotalChildCategories"] = childCategories.Count(),
                ["IsActive"] = category.IsActive,
                ["CategoryType"] = category.CategoryType,
                ["HasParent"] = category.ParentId.HasValue
            };
        }

        private void ValidateCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrWhiteSpace(category.Name)) throw new ArgumentException("Tên danh mục không được trống");
            if (category.Name.Length > 255) throw new ArgumentException("Tên danh mục không được vượt quá 255 ký tự");
        }
    }

    // ==================== VARIANT SERVICE IMPLEMENTATION ====================
    public class VariantService : IVariantService
    {
        private readonly IVariantRepository _variantRepository;
        private readonly IProductRepository _productRepository;

        public VariantService(IVariantRepository variantRepository, IProductRepository productRepository)
        {
            _variantRepository = variantRepository;
            _productRepository = productRepository;
        }

        public async Task<Variant> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            var variant = await _variantRepository.GetByIdAsync(id);
            return variant ?? throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {id}");
        }

        public async Task<IEnumerable<Variant>> GetAllAsync()
        {
            return await _variantRepository.GetActiveVariantsAsync();
        }

        public async Task<Variant> CreateAsync(Variant entity)
        {
            ValidateVariant(entity);

            // Check if product exists
            var product = await _productRepository.GetByIdAsync(entity.ProductId);
            if (product == null) throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {entity.ProductId}");

            // Check duplicate SKU
            if (!string.IsNullOrWhiteSpace(entity.SKU))
            {
                var existingBySKU = await _variantRepository.GetVariantBySKUAsync(entity.SKU);
                if (existingBySKU != null)
                    throw new InvalidOperationException($"SKU {entity.SKU} đã tồn tại");
            }

            // Check duplicate barcode
            if (!string.IsNullOrWhiteSpace(entity.Barcode))
            {
                var existingByBarcode = await _variantRepository.GetVariantByBarcodeAsync(entity.Barcode);
                if (existingByBarcode != null)
                    throw new InvalidOperationException($"Barcode {entity.Barcode} đã tồn tại");
            }

            // Set default values
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _variantRepository.CreateAsync(entity);
        }

        public async Task<Variant> UpdateAsync(Variant entity)
        {
            if (entity.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            ValidateVariant(entity);

            var existing = await _variantRepository.GetByIdAsync(entity.VariantId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {entity.VariantId}");

            // Check SKU uniqueness if changed
            if (!string.IsNullOrWhiteSpace(entity.SKU) && existing.SKU != entity.SKU)
            {
                var variantWithSKU = await _variantRepository.GetVariantBySKUAsync(entity.SKU);
                if (variantWithSKU != null)
                    throw new InvalidOperationException($"SKU {entity.SKU} đã được sử dụng");
            }

            // Check barcode uniqueness if changed
            if (!string.IsNullOrWhiteSpace(entity.Barcode) && existing.Barcode != entity.Barcode)
            {
                var variantWithBarcode = await _variantRepository.GetVariantByBarcodeAsync(entity.Barcode);
                if (variantWithBarcode != null)
                    throw new InvalidOperationException($"Barcode {entity.Barcode} đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _variantRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            return await _variantRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID biến thể không hợp lệ");

            var variant = await _variantRepository.GetByIdAsync(id);
            if (variant == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {id}");

            var updatedVariant = new Variant
            {
                VariantId = variant.VariantId,
                SKU = variant.SKU,
                Barcode = variant.Barcode,
                Name = variant.Name,
                Unit = variant.Unit,
                Price = variant.Price,
                QrCode = variant.QrCode,
                ProductId = variant.ProductId,
                TypeId = variant.TypeId,
                IsActive = false,
                CreatedDate = variant.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _variantRepository.UpdateAsync(updatedVariant);
            return true;
        }

        public async Task<IEnumerable<Variant>> GetVariantsByProductAsync(int productId)
        {
            if (productId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
            return await _variantRepository.GetVariantsByProductAsync(productId);
        }

        public async Task<Variant> GetVariantBySKUAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU không được trống");
            return await _variantRepository.GetVariantBySKUAsync(sku);
        }

        public async Task<Variant> GetVariantByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) throw new ArgumentException("Barcode không được trống");
            return await _variantRepository.GetVariantByBarcodeAsync(barcode);
        }

        public async Task<Variant> GetVariantWithProductAsync(int variantId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            return await _variantRepository.GetVariantWithProductAsync(variantId);
        }

        public async Task<IEnumerable<Variant>> GetActiveVariantsAsync()
        {
            return await _variantRepository.GetActiveVariantsAsync();
        }

        public async Task<IEnumerable<Variant>> GetVariantsLowInStockAsync(int stationId)
        {
            if (stationId <= 0) throw new ArgumentException("ID trạm không hợp lệ");
            return await _variantRepository.GetVariantsLowInStockAsync(stationId);
        }

        public async Task<bool> UpdateVariantPriceAsync(int variantId, decimal newPrice)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (newPrice < 0) throw new ArgumentException("Giá không được âm");

            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {variantId}");

            var updatedVariant = new Variant
            {
                VariantId = variant.VariantId,
                SKU = variant.SKU,
                Barcode = variant.Barcode,
                Name = variant.Name,
                Unit = variant.Unit,
                Price = newPrice,
                QrCode = variant.QrCode,
                ProductId = variant.ProductId,
                TypeId = variant.TypeId,
                IsActive = variant.IsActive,
                CreatedDate = variant.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _variantRepository.UpdateAsync(updatedVariant);
            return true;
        }

        public async Task<Variant> CreateVariantAsync(Variant variant)
        {
            return await CreateAsync(variant);
        }

        public async Task<Dictionary<string, object>> GetVariantPerformanceAsync(int variantId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");

            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {variantId}");

            var product = await _productRepository.GetByIdAsync(variant.ProductId);
            var stock = await _variantRepository.GetVariantsLowInStockAsync(1); // Default station

            return new Dictionary<string, object>
            {
                ["VariantId"] = variantId,
                ["Name"] = variant.Name,
                ["SKU"] = variant.SKU,
                ["Price"] = variant.Price,
                ["ProductName"] = product?.Name,
                ["IsLowStock"] = stock.Any(v => v.VariantId == variantId),
                ["IsActive"] = variant.IsActive,
                ["HasBarcode"] = !string.IsNullOrWhiteSpace(variant.Barcode)
            };
        }

        private void ValidateVariant(Variant variant)
        {
            if (variant == null) throw new ArgumentNullException(nameof(variant));
            if (string.IsNullOrWhiteSpace(variant.Name)) throw new ArgumentException("Tên biến thể không được trống");
            if (string.IsNullOrWhiteSpace(variant.Unit)) throw new ArgumentException("Đơn vị tính không được trống");
            if (variant.ProductId <= 0) throw new ArgumentException("ID sản phẩm không hợp lệ");
            if (variant.TypeId <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");
            if (variant.Price < 0) throw new ArgumentException("Giá không được âm");
            if (variant.Name.Length > 255) throw new ArgumentException("Tên biến thể không được vượt quá 255 ký tự");
            if (!string.IsNullOrWhiteSpace(variant.SKU) && variant.SKU.Length > 50)
                throw new ArgumentException("SKU không được vượt quá 50 ký tự");
        }
    }

    // ==================== VARIANT TYPE SERVICE IMPLEMENTATION ====================
    public class VariantTypeService : IVariantTypeService
    {
        private readonly IVariantTypeRepository _variantTypeRepository;
        private readonly IVariantRepository _variantRepository;

        public VariantTypeService(IVariantTypeRepository variantTypeRepository, IVariantRepository variantRepository)
        {
            _variantTypeRepository = variantTypeRepository;
            _variantRepository = variantRepository;
        }

        public async Task<VariantType> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");
            var variantType = await _variantTypeRepository.GetByIdAsync(id);
            return variantType ?? throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {id}");
        }

        public async Task<IEnumerable<VariantType>> GetAllAsync()
        {
            return await _variantTypeRepository.GetActiveVariantTypesAsync();
        }

        public async Task<VariantType> CreateAsync(VariantType entity)
        {
            ValidateVariantType(entity);

            // Check duplicate name
            var existingByName = await _variantTypeRepository.GetVariantTypeByNameAsync(entity.Name);
            if (existingByName != null)
                throw new InvalidOperationException($"Tên loại biến thể {entity.Name} đã tồn tại");

            // Set default values
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _variantTypeRepository.CreateAsync(entity);
        }

        public async Task<VariantType> UpdateAsync(VariantType entity)
        {
            if (entity.TypeId <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");
            ValidateVariantType(entity);

            var existing = await _variantTypeRepository.GetByIdAsync(entity.TypeId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {entity.TypeId}");

            // Check name uniqueness if changed
            if (existing.Name != entity.Name)
            {
                var variantTypeWithName = await _variantTypeRepository.GetVariantTypeByNameAsync(entity.Name);
                if (variantTypeWithName != null)
                    throw new InvalidOperationException($"Tên loại biến thể {entity.Name} đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _variantTypeRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");

            var variantType = await _variantTypeRepository.GetByIdAsync(id);
            if (variantType == null) throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {id}");

            // Check if variant type is used by variants
            var variants = await _variantRepository.GetActiveVariantsAsync();
            if (variants.Any(v => v.TypeId == id))
                throw new InvalidOperationException("Không thể xóa loại biến thể đang được sử dụng");

            return await _variantTypeRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");

            var variantType = await _variantTypeRepository.GetByIdAsync(id);
            if (variantType == null) throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {id}");

            var updatedVariantType = new VariantType
            {
                TypeId = variantType.TypeId,
                Name = variantType.Name,
                Description = variantType.Description,
                IsActive = false,
                CreatedDate = variantType.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _variantTypeRepository.UpdateAsync(updatedVariantType);
            return true;
        }

        public async Task<IEnumerable<VariantType>> GetActiveVariantTypesAsync()
        {
            return await _variantTypeRepository.GetActiveVariantTypesAsync();
        }

        public async Task<VariantType> GetVariantTypeByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên loại biến thể không được trống");
            return await _variantTypeRepository.GetVariantTypeByNameAsync(name);
        }

        public async Task<bool> UpdateVariantTypeStatusAsync(int typeId, bool isActive)
        {
            if (typeId <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");

            var variantType = await _variantTypeRepository.GetByIdAsync(typeId);
            if (variantType == null) throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {typeId}");

            var updatedVariantType = new VariantType
            {
                TypeId = variantType.TypeId,
                Name = variantType.Name,
                Description = variantType.Description,
                IsActive = isActive,
                CreatedDate = variantType.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _variantTypeRepository.UpdateAsync(updatedVariantType);
            return true;
        }

        public async Task<Dictionary<string, object>> GetVariantTypeUsageAsync(int typeId)
        {
            if (typeId <= 0) throw new ArgumentException("ID loại biến thể không hợp lệ");

            var variantType = await _variantTypeRepository.GetByIdAsync(typeId);
            if (variantType == null) throw new KeyNotFoundException($"Không tìm thấy loại biến thể với ID: {typeId}");

            var variants = await _variantRepository.GetActiveVariantsAsync();
            var variantsUsingType = variants.Where(v => v.TypeId == typeId);

            return new Dictionary<string, object>
            {
                ["TypeId"] = typeId,
                ["Name"] = variantType.Name,
                ["TotalVariantsUsing"] = variantsUsingType.Count(),
                ["IsActive"] = variantType.IsActive,
                ["UsagePercentage"] = variants.Any() ? (variantsUsingType.Count() * 100.0 / variants.Count()) : 0
            };
        }

        private void ValidateVariantType(VariantType variantType)
        {
            if (variantType == null) throw new ArgumentNullException(nameof(variantType));
            if (string.IsNullOrWhiteSpace(variantType.Name)) throw new ArgumentException("Tên loại biến thể không được trống");
            if (variantType.Name.Length > 100) throw new ArgumentException("Tên loại biến thể không được vượt quá 100 ký tự");
        }
    }

    // ==================== COMBO SERVICE IMPLEMENTATION ====================
    public class ComboService : IComboService
    {
        private readonly IComboRepository _comboRepository;
        private readonly IComboItemRepository _comboItemRepository;
        private readonly IVariantRepository _variantRepository;

        public ComboService(IComboRepository comboRepository, IComboItemRepository comboItemRepository, IVariantRepository variantRepository)
        {
            _comboRepository = comboRepository;
            _comboItemRepository = comboItemRepository;
            _variantRepository = variantRepository;
        }

        public async Task<Combo> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID combo không hợp lệ");
            var combo = await _comboRepository.GetByIdAsync(id);
            return combo ?? throw new KeyNotFoundException($"Không tìm thấy combo với ID: {id}");
        }

        public async Task<IEnumerable<Combo>> GetAllAsync()
        {
            return await _comboRepository.GetActiveCombosAsync();
        }

        public async Task<Combo> CreateAsync(Combo entity)
        {
            ValidateCombo(entity);

            // Check duplicate code
            if (!string.IsNullOrWhiteSpace(entity.Code))
            {
                var existingByCode = await _comboRepository.GetComboByCodeAsync(entity.Code);
                if (existingByCode != null)
                    throw new InvalidOperationException($"Mã combo {entity.Code} đã tồn tại");
            }

            // Validate total price
            if (entity.TotalPrice <= 0)
                throw new ArgumentException("Tổng giá combo phải lớn hơn 0");

            // Set default values
            entity.IsActive = true;
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;

            return await _comboRepository.CreateAsync(entity);
        }

        public async Task<Combo> UpdateAsync(Combo entity)
        {
            if (entity.ComboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            ValidateCombo(entity);

            var existing = await _comboRepository.GetByIdAsync(entity.ComboId);
            if (existing == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {entity.ComboId}");

            // Check code uniqueness if changed
            if (!string.IsNullOrWhiteSpace(entity.Code) && existing.Code != entity.Code)
            {
                var comboWithCode = await _comboRepository.GetComboByCodeAsync(entity.Code);
                if (comboWithCode != null)
                    throw new InvalidOperationException($"Mã combo {entity.Code} đã được sử dụng");
            }

            entity.UpdatedDate = DateTime.Now;
            return await _comboRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {id}");

            // Delete combo items first
            await _comboItemRepository.DeleteItemsByComboAsync(id);

            return await _comboRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var combo = await _comboRepository.GetByIdAsync(id);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {id}");

            var updatedCombo = new Combo
            {
                ComboId = combo.ComboId,
                Code = combo.Code,
                Name = combo.Name,
                Image = combo.Image,
                Description = combo.Description,
                TotalPrice = combo.TotalPrice,
                IsActive = false,
                CreatedDate = combo.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _comboRepository.UpdateAsync(updatedCombo);
            return true;
        }

        public async Task<Combo> GetComboWithItemsAsync(int comboId)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            return await _comboRepository.GetComboWithItemsAsync(comboId);
        }

        public async Task<IEnumerable<Combo>> GetActiveCombosAsync()
        {
            return await _comboRepository.GetActiveCombosAsync();
        }

        public async Task<Combo> GetComboByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Mã combo không được trống");
            return await _comboRepository.GetComboByCodeAsync(code);
        }

        public async Task<IEnumerable<Combo>> GetCombosByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) throw new ArgumentException("ID danh mục không hợp lệ");
            return await _comboRepository.GetCombosByCategoryAsync(categoryId);
        }

        public async Task<Combo> CreateComboWithItemsAsync(Combo combo, List<ComboItem> items)
        {
            ValidateCombo(combo);

            // Create combo
            combo.IsActive = true;
            combo.CreatedDate = DateTime.Now;
            combo.UpdatedDate = DateTime.Now;

            var createdCombo = await _comboRepository.CreateAsync(combo);

            // Validate and create combo items
            foreach (var item in items)
            {
                // Check if variant exists
                var variant = await _variantRepository.GetByIdAsync(item.VariantId);
                if (variant == null)
                    throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {item.VariantId}");

                item.ComboId = createdCombo.ComboId;
                await _comboItemRepository.CreateAsync(item);
            }

            return createdCombo;
        }

        public async Task<bool> UpdateComboPriceAsync(int comboId, decimal newPrice)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            if (newPrice <= 0) throw new ArgumentException("Giá mới phải lớn hơn 0");

            var combo = await _comboRepository.GetByIdAsync(comboId);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {comboId}");

            var updatedCombo = new Combo
            {
                ComboId = combo.ComboId,
                Code = combo.Code,
                Name = combo.Name,
                Image = combo.Image,
                Description = combo.Description,
                TotalPrice = newPrice,
                IsActive = combo.IsActive,
                CreatedDate = combo.CreatedDate,
                UpdatedDate = DateTime.Now
            };

            await _comboRepository.UpdateAsync(updatedCombo);
            return true;
        }

        public async Task<bool> UpdateComboItemsAsync(int comboId, List<ComboItem> items)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var combo = await _comboRepository.GetByIdAsync(comboId);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {comboId}");

            // Delete existing items
            await _comboItemRepository.DeleteItemsByComboAsync(comboId);

            // Add new items
            foreach (var item in items)
            {
                // Check if variant exists
                var variant = await _variantRepository.GetByIdAsync(item.VariantId);
                if (variant == null)
                    throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {item.VariantId}");

                item.ComboId = comboId;
                await _comboItemRepository.CreateAsync(item);
            }

            return true;
        }

        public async Task<Dictionary<string, object>> GetComboPerformanceAsync(int comboId)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var combo = await _comboRepository.GetByIdAsync(comboId);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {comboId}");

            var items = await _comboItemRepository.GetItemsByComboAsync(comboId);
            var totalComponentValue = 0m;

            foreach (var item in items)
            {
                var variant = await _variantRepository.GetByIdAsync(item.VariantId);
                if (variant != null)
                {
                    totalComponentValue += variant.Price * item.Quantity;
                }
            }

            var savings = totalComponentValue - combo.TotalPrice;

            return new Dictionary<string, object>
            {
                ["ComboId"] = comboId,
                ["Name"] = combo.Name,
                ["TotalPrice"] = combo.TotalPrice,
                ["TotalItems"] = items.Count(),
                ["TotalComponentValue"] = totalComponentValue,
                ["CustomerSavings"] = savings,
                ["SavingsPercentage"] = totalComponentValue > 0 ? (savings * 100 / totalComponentValue) : 0,
                ["IsActive"] = combo.IsActive
            };
        }

        private void ValidateCombo(Combo combo)
        {
            if (combo == null) throw new ArgumentNullException(nameof(combo));
            if (string.IsNullOrWhiteSpace(combo.Name)) throw new ArgumentException("Tên combo không được trống");
            if (combo.TotalPrice <= 0) throw new ArgumentException("Tổng giá combo phải lớn hơn 0");
            if (combo.Name.Length > 255) throw new ArgumentException("Tên combo không được vượt quá 255 ký tự");
            if (!string.IsNullOrWhiteSpace(combo.Code) && combo.Code.Length > 30)
                throw new ArgumentException("Mã combo không được vượt quá 30 ký tự");
        }
    }

    // ==================== COMBO ITEM SERVICE IMPLEMENTATION ====================
    public class ComboItemService : IComboItemService
    {
        private readonly IComboItemRepository _comboItemRepository;
        private readonly IComboRepository _comboRepository;
        private readonly IVariantRepository _variantRepository;

        public ComboItemService(IComboItemRepository comboItemRepository, IComboRepository comboRepository, IVariantRepository variantRepository)
        {
            _comboItemRepository = comboItemRepository;
            _comboRepository = comboRepository;
            _variantRepository = variantRepository;
        }

        public async Task<ComboItem> GetByIdAsync(int id)
        {
            throw new NotSupportedException("ComboItem không hỗ trợ GetByIdAsync do composite key");
        }

        public async Task<IEnumerable<ComboItem>> GetAllAsync()
        {
            throw new NotSupportedException("ComboItem không hỗ trợ GetAllAsync");
        }

        public async Task<ComboItem> CreateAsync(ComboItem entity)
        {
            ValidateComboItem(entity);

            // Check if combo exists
            var combo = await _comboRepository.GetByIdAsync(entity.ComboId);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {entity.ComboId}");

            // Check if variant exists
            var variant = await _variantRepository.GetByIdAsync(entity.VariantId);
            if (variant == null) throw new KeyNotFoundException($"Không tìm thấy biến thể với ID: {entity.VariantId}");

            // Check if item already exists in combo
            var existingItems = await _comboItemRepository.GetItemsByComboAsync(entity.ComboId);
            if (existingItems.Any(item => item.VariantId == entity.VariantId))
                throw new InvalidOperationException("Biến thể đã tồn tại trong combo");

            return await _comboItemRepository.CreateAsync(entity);
        }

        public async Task<ComboItem> UpdateAsync(ComboItem entity)
        {
            throw new NotSupportedException("ComboItem không hỗ trợ UpdateAsync do composite key");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotSupportedException("ComboItem không hỗ trợ DeleteAsync do composite key");
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            throw new NotSupportedException("ComboItem không hỗ trợ SoftDeleteAsync");
        }

        public async Task<IEnumerable<ComboItem>> GetItemsByComboAsync(int comboId)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            return await _comboItemRepository.GetItemsByComboAsync(comboId);
        }

        public async Task<bool> DeleteItemsByComboAsync(int comboId)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            return await _comboItemRepository.DeleteItemsByComboAsync(comboId);
        }

        public async Task<bool> UpdateComboItemQuantityAsync(int comboId, int variantId, int quantity)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");

            return await _comboItemRepository.UpdateComboItemQuantityAsync(comboId, variantId, quantity);
        }

        public async Task<IEnumerable<ComboItem>> GetCombosContainingVariantAsync(int variantId)
        {
            if (variantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            return await _comboItemRepository.GetCombosContainingVariantAsync(variantId);
        }

        public async Task<bool> AddItemsToComboAsync(int comboId, List<ComboItem> items)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var combo = await _comboRepository.GetByIdAsync(comboId);
            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {comboId}");

            foreach (var item in items)
            {
                item.ComboId = comboId;
                await CreateAsync(item);
            }

            return true;
        }

        public async Task<Dictionary<string, object>> GetComboItemAnalysisAsync(int comboId)
        {
            if (comboId <= 0) throw new ArgumentException("ID combo không hợp lệ");

            var items = await _comboItemRepository.GetItemsWithVariantsAsync(comboId);
            var combo = await _comboRepository.GetByIdAsync(comboId);

            if (combo == null) throw new KeyNotFoundException($"Không tìm thấy combo với ID: {comboId}");

            var totalComponentValue = 0m;
            var mostExpensiveItem = items.OrderByDescending(i => i.Quantity).FirstOrDefault();

            foreach (var item in items)
            {
                var variant = await _variantRepository.GetByIdAsync(item.VariantId);
                if (variant != null)
                {
                    totalComponentValue += variant.Price * item.Quantity;
                }
            }

            return new Dictionary<string, object>
            {
                ["ComboId"] = comboId,
                ["ComboName"] = combo.Name,
                ["TotalItems"] = items.Count(),
                ["TotalComponentValue"] = totalComponentValue,
                ["ComboPrice"] = combo.TotalPrice,
                ["Savings"] = totalComponentValue - combo.TotalPrice,
                ["MostUsedItem"] = mostExpensiveItem?.VariantId,
                ["AverageQuantity"] = items.Any() ? items.Average(i => i.Quantity) : 0
            };
        }

        private void ValidateComboItem(ComboItem comboItem)
        {
            if (comboItem == null) throw new ArgumentNullException(nameof(comboItem));
            if (comboItem.ComboId <= 0) throw new ArgumentException("ID combo không hợp lệ");
            if (comboItem.VariantId <= 0) throw new ArgumentException("ID biến thể không hợp lệ");
            if (comboItem.Quantity <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
        }
    }
}