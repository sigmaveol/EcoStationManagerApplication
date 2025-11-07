using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetBySkuAsync(string sku);

        Task<IEnumerable<Product>> SearchByNameAsync(string keyword);

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetByTypeAsync(string productType);

        Task<bool> UpdatePriceAsync(int productId, decimal newPrice);

        Task<IEnumerable<Product>> GetLowStockProductsAsync();

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
}
