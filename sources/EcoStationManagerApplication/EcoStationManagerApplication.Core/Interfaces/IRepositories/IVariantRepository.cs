using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoStationManagerApplication.Models;

namespace EcoStationManagerApplication.Core.Interfaces.IRepositories
{
    public interface IVariantRepository : IRepository<Variant>
    {
        Task<Variant> GetByBarcodeAsync(string barcode);
        Task<Variant> GetBySKUAsync(string sku);
        Task<IEnumerable<Variant>> GetByProductIdAsync(int productId);
        Task<IEnumerable<Variant>> GetActiveVariantsAsync();
        Task<bool> UpdateStockAsync(int variantId, decimal newStock);
        Task<IEnumerable<Variant>> GetLowStockVariantsAsync(decimal threshold);
        Task<bool> DeactivateVariantAsync(int variantId);
    }
}
