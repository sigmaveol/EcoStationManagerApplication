using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== PRODUCT SERVICE INTERFACE ====================
    public interface IProductService : IService<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> GetProductWithVariantsAsync(int productId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<Product> GetProductByCodeAsync(string code);
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);
        Task<bool> UpdateProductStatusAsync(int productId, bool isActive);
        Task<IEnumerable<Product>> GetProductsLowInStockAsync(int stationId);
        Task<Dictionary<string, object>> GetProductStatisticsAsync(int productId);
        Task<Product> CreateProductWithVariantsAsync(Product product, List<Variant> variants);
    }

    // ==================== CATEGORY SERVICE INTERFACE ====================
    public interface ICategoryService : IService<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string categoryType);
        Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isActive);
        Task<Category> CreateCategoryWithChildrenAsync(Category category, List<Category> children);
        Task<Dictionary<string, object>> GetCategoryStatisticsAsync(int categoryId);
    }

    // ==================== VARIANT SERVICE INTERFACE ====================
    public interface IVariantService : IService<Variant>
    {
        Task<IEnumerable<Variant>> GetVariantsByProductAsync(int productId);
        Task<Variant> GetVariantBySKUAsync(string sku);
        Task<Variant> GetVariantByBarcodeAsync(string barcode);
        Task<Variant> GetVariantWithProductAsync(int variantId);
        Task<IEnumerable<Variant>> GetActiveVariantsAsync();
        Task<IEnumerable<Variant>> GetVariantsLowInStockAsync(int stationId);
        Task<bool> UpdateVariantPriceAsync(int variantId, decimal newPrice);
        Task<Variant> CreateVariantAsync(Variant variant);
        Task<Dictionary<string, object>> GetVariantPerformanceAsync(int variantId);
    }

    // ==================== VARIANT TYPE SERVICE INTERFACE ====================
    public interface IVariantTypeService : IService<VariantType>
    {
        Task<IEnumerable<VariantType>> GetActiveVariantTypesAsync();
        Task<VariantType> GetVariantTypeByNameAsync(string name);
        Task<bool> UpdateVariantTypeStatusAsync(int typeId, bool isActive);
        Task<Dictionary<string, object>> GetVariantTypeUsageAsync(int typeId);
    }

    // ==================== COMBO SERVICE INTERFACE ====================
    public interface IComboService : IService<Combo>
    {
        Task<Combo> GetComboWithItemsAsync(int comboId);
        Task<IEnumerable<Combo>> GetActiveCombosAsync();
        Task<Combo> GetComboByCodeAsync(string code);
        Task<IEnumerable<Combo>> GetCombosByCategoryAsync(int categoryId);
        Task<Combo> CreateComboWithItemsAsync(Combo combo, List<ComboItem> items);
        Task<bool> UpdateComboPriceAsync(int comboId, decimal newPrice);
        Task<bool> UpdateComboItemsAsync(int comboId, List<ComboItem> items);
        Task<Dictionary<string, object>> GetComboPerformanceAsync(int comboId);
    }

    // ==================== COMBO ITEM SERVICE INTERFACE ====================
    public interface IComboItemService : IService<ComboItem>
    {
        Task<IEnumerable<ComboItem>> GetItemsByComboAsync(int comboId);
        Task<bool> DeleteItemsByComboAsync(int comboId);
        Task<bool> UpdateComboItemQuantityAsync(int comboId, int variantId, int quantity);
        Task<IEnumerable<ComboItem>> GetCombosContainingVariantAsync(int variantId);
        Task<bool> AddItemsToComboAsync(int comboId, List<ComboItem> items);
        Task<Dictionary<string, object>> GetComboItemAnalysisAsync(int comboId);
    }
}