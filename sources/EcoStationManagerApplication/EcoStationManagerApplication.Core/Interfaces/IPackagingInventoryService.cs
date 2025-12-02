using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IPackagingInventoryService
    {
        Task<Result<IEnumerable<PackagingInventory>>> GetAllAsync();
        Task<Result<PackagingInventory>> GetPackagingInventoryAsync(int packagingId);
        Task<Result<List<PackagingInventory>>> GetLowStockPackagingAsync();
        Task<Result<bool>> UpdatePackagingQuantitiesAsync(int packagingId, PackagingQuantities quantities);
        Task<Result<bool>> AddPackagingNewAsync(int packagingId, PackagingQuantities quantities);
        Task<Result<bool>> TransferToInUseAsync(int packagingId, int quantity);
        Task<Result<bool>> ReturnForCleaningAsync(int packagingId, int quantity);
        Task<Result<bool>> MoveReturnedToNeedCleaningAsync(int packagingId, int quantity);
        Task<Result<bool>> CompleteCleaningAsync(int packagingId, int quantity);
        Task<Result<bool>> MarkAsDamagedAsync(int packagingId, int quantity);
        Task<Result<bool>> MarkReturnedAsDamagedAsync(int packagingId, int quantity);
        Task<Result<bool>> MarkNewAsDamagedAsync(int packagingId, int quantity);
        Task<Result<bool>> MarkNeedCleaningAsDamagedAsync(int packagingId, int quantity);
        Task<Result<PackagingQuantities>> GetPackagingQuantitiesAsync(int packagingId);
        Task<Result<bool>> IsNewPackagingSufficientAsync(int packagingId, int requiredQuantity);
    }
}
