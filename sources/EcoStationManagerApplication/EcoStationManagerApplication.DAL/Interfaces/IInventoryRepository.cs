using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task<IEnumerable<Inventory>> GetLowStockAsync(decimal threshold);
        Task<IEnumerable<Inventory>> GetExpiringSoonAsync(int days);
        Task<Inventory> GetByProductIdAsync(int productId);
        Task<bool> UpdateQuantityAsync(int productId, decimal newQuantity);
        Task<bool> AdjustQuantityAsync(int productId, decimal delta);
        Task<(IEnumerable<Inventory> Inventories, int TotalCount)> GetPagedInventoriesAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            bool includeExpired = false
        );
    }
}
