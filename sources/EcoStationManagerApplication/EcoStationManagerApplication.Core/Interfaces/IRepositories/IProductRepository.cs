using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    // ==================== PRODUCT REPOSITORY INTERFACE ====================
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> GetProductWithVariantsAsync(int productId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<Product> GetProductByCodeAsync(string code);
        Task<IEnumerable<Product>> SearchProductsAsync(string keyword);
        Task<IEnumerable<Product>> GetProductsWithInventoryAsync(int stationId);
        Task<IEnumerable<Product>> GetProductsLowInStockAsync(int stationId);
    }

    // ==================== CATEGORY REPOSITORY INTERFACE ====================
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(string categoryType);
        Task<IEnumerable<Category>> GetChildCategoriesAsync(int parentId);
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<Category> GetCategoryByNameAsync(string name);
    }

    // ==================== VARIANT REPOSITORY INTERFACE ====================
    public interface IVariantRepository : IRepository<Variant>
    {
        Task<IEnumerable<Variant>> GetVariantsByProductAsync(int productId);
        Task<Variant> GetVariantBySKUAsync(string sku);
        Task<Variant> GetVariantByBarcodeAsync(string barcode);
        Task<Variant> GetVariantWithProductAsync(int variantId);
        Task<IEnumerable<Variant>> GetActiveVariantsAsync();
        Task<IEnumerable<Variant>> GetVariantsLowInStockAsync(int stationId);
        Task<IEnumerable<Variant>> GetVariantsByCategoryAsync(int categoryId);
    }

    // ==================== VARIANT TYPE REPOSITORY INTERFACE ====================
    public interface IVariantTypeRepository : IRepository<VariantType>
    {
        Task<IEnumerable<VariantType>> GetActiveVariantTypesAsync();
        Task<VariantType> GetVariantTypeByNameAsync(string name);
    }

    // ==================== COMBO REPOSITORY INTERFACE ====================
    public interface IComboRepository : IRepository<Combo>
    {
        Task<Combo> GetComboWithItemsAsync(int comboId);
        Task<IEnumerable<Combo>> GetActiveCombosAsync();
        Task<Combo> GetComboByCodeAsync(string code);
        Task<IEnumerable<Combo>> GetCombosByCategoryAsync(int categoryId);
        Task<IEnumerable<Combo>> GetCombosWithItemsAsync();
    }

    // ==================== COMBO ITEM REPOSITORY INTERFACE ====================
    public interface IComboItemRepository : IRepository<ComboItem>
    {
        Task<IEnumerable<ComboItem>> GetItemsByComboAsync(int comboId);
        Task<bool> DeleteItemsByComboAsync(int comboId);
        Task<bool> UpdateComboItemQuantityAsync(int comboId, int variantId, int quantity);
        Task<IEnumerable<ComboItem>> GetCombosContainingVariantAsync(int variantId);
        Task<IEnumerable<ComboItem>> GetItemsWithVariantsAsync(int comboId);
    }
}