using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IPackagingRepository : IRepository<Packaging>
    {
        Task<Packaging> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Packaging>> GetByTypeAsync(string type);
        Task<bool> UpdateDepositPriceAsync(int packagingId, decimal newPrice);
        Task<(IEnumerable<Packaging> Packagings, int TotalCount)> GetPagedPackagingsAsync(
            int pageNumber,
            int pageSize,
            string search = null,
            string type = null
        );
    }
}
