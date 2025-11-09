using EcoStationManagerApplication.Models.DTOs;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IPackagingInventoryRepository : IRepository<PackagingInventory>
    {
        /// <summary>
        /// Lấy tồn kho bao bì theo packagingId
        /// </summary>
        Task<PackagingInventory> GetByPackagingAsync(int packagingId);

        /// <summary>
        /// Lấy bao bì sắp hết hàng
        /// </summary>
        Task<IEnumerable<PackagingInventory>> GetLowStockPackagingAsync();

        /// <summary>
        /// Cập nhật số lượng các loại bao bì
        /// </summary>
        Task<bool> UpdateQuantitiesAsync(int packagingId, PackagingQuantities quantities);

        /// <summary>
        /// Chuyển bao bì mới sang đang sử dụng
        /// </summary>
        Task<bool> TransferToInUseAsync(int packagingId, int quantity);

        /// <summary>
        /// Nhận bao bì trả về cần vệ sinh
        /// </summary>
        Task<bool> ReturnForCleaningAsync(int packagingId, int quantity);

        /// <summary>
        /// Hoàn thành vệ sinh bao bì
        /// </summary>
        Task<bool> CompleteCleaningAsync(int packagingId, int quantity);

        /// <summary>
        /// Đánh dấu bao bì hỏng
        /// </summary>
        Task<bool> MarkAsDamagedAsync(int packagingId, int quantity);

        /// <summary>
        /// Lấy tổng số lượng bao bì theo trạng thái
        /// </summary>
        Task<PackagingQuantities> GetPackagingQuantitiesAsync(int packagingId);

        /// <summary>
        /// Kiểm tra số lượng bao bì mới có đủ không
        /// </summary>
        Task<bool> IsNewPackagingSufficientAsync(int packagingId, int requiredQuantity);

    }

}
