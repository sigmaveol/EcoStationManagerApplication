using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces
{
    /// <summary>
    /// Product repository interface extending the generic IRepository.
    /// Provides extra methods for product and inventory operations.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Get a product by its unique code.
        /// </summary>
        Task<Product> GetByCodeAsync(string productCode);

        /// <summary>
        /// Get all products belonging to a specific category.
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        /// <summary>
        /// Search products by keyword (name, code, etc.).
        /// </summary>
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        /// <summary>
        /// Update the stock quantity of a product.
        /// </summary>
        //Task<bool> UpdateStockAsync(int productId, decimal newStock);

        /// <summary>
        /// Get products with stock below a given threshold.
        /// </summary>
        //Task<IEnumerable<Product>> GetLowStockProductsAsync(decimal threshold);

        /// <summary>
        /// Get all active products including their variants.
        /// </summary>
        Task<IEnumerable<Product>> GetActiveProductsWithVariantsAsync();
    }
}
