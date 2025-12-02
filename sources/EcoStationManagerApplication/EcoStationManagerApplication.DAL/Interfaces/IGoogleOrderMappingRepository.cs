using EcoStationManagerApplication.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IGoogleOrderMappingRepository : IRepository<GoogleOrderMapping>
    {
        Task<int> AddMappingAsync(GoogleOrderMapping mapping);
        Task<IEnumerable<GoogleOrderMapping>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<GoogleOrderMapping>> GetByConfigIdAsync(int configId);
    }
}
