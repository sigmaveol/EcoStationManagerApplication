using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.DAL.SqlQueries;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Repositories
{
    public class PackagingInventoryRepository : BaseRepository<PackagingInventory>, IPackagingInventoryRepository
    {
        public PackagingInventoryRepository(IDatabaseHelper databaseHelper)
            : base(databaseHelper, "PackagingInventories", "pk_inv_id")
        {
        }

        public async Task<PackagingInventory> GetByPackagingAsync(int packagingId)
        {
            try
            {
                var result = await _databaseHelper.QueryFirstOrDefaultAsync<PackagingInventory>(
                    PackagingInventoryQueries.GetByPackaging,
                    new { PackagingId = packagingId }
                );

                // Nếu chưa có inventory, tạo mới
                if (result == null)
                {
                    await EnsurePackagingInventoryExistsAsync(packagingId);
                    result = await _databaseHelper.QueryFirstOrDefaultAsync<PackagingInventory>(
                        PackagingInventoryQueries.GetByPackaging,
                        new { PackagingId = packagingId }
                    );
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByPackagingAsync error - PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PackagingInventory>> GetLowStockPackagingAsync()
        {
            try
            {
                return await _databaseHelper.QueryAsync<PackagingInventory>(
                    PackagingInventoryQueries.GetLowStockPackaging
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"GetLowStockPackagingAsync error - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateQuantitiesAsync(int packagingId, PackagingQuantities quantities)
        {
            try
            {
                await EnsurePackagingInventoryExistsAsync(packagingId);

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    PackagingInventoryQueries.UpdateQuantities,
                    new
                    {
                        PackagingId = packagingId,
                        quantities.QtyNew,
                        quantities.QtyInUse,
                        quantities.QtyReturned,
                        quantities.QtyNeedCleaning,
                        quantities.QtyCleaned,
                        quantities.QtyDamaged
                    }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã cập nhật số lượng bao bì PackagingId: {packagingId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"UpdateQuantitiesAsync error - PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TransferToInUseAsync(int packagingId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                await EnsurePackagingInventoryExistsAsync(packagingId);

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    PackagingInventoryQueries.TransferToInUse,
                    new { PackagingId = packagingId, Quantity = quantity }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã chuyển {quantity} bao bì sang đang sử dụng - PackagingId: {packagingId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"TransferToInUseAsync error - PackagingId: {packagingId}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ReturnForCleaningAsync(int packagingId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                await EnsurePackagingInventoryExistsAsync(packagingId);

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    PackagingInventoryQueries.ReturnForCleaning,
                    new { PackagingId = packagingId, Quantity = quantity }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã nhận {quantity} bao bì cần vệ sinh - PackagingId: {packagingId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"ReturnForCleaningAsync error - PackagingId: {packagingId}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CompleteCleaningAsync(int packagingId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                await EnsurePackagingInventoryExistsAsync(packagingId);

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    PackagingInventoryQueries.CompleteCleaning,
                    new { PackagingId = packagingId, Quantity = quantity }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã hoàn thành vệ sinh {quantity} bao bì - PackagingId: {packagingId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"CompleteCleaningAsync error - PackagingId: {packagingId}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> MarkAsDamagedAsync(int packagingId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return false;

                await EnsurePackagingInventoryExistsAsync(packagingId);

                var affectedRows = await _databaseHelper.ExecuteAsync(
                    PackagingInventoryQueries.MarkAsDamaged,
                    new { PackagingId = packagingId, Quantity = quantity }
                );

                if (affectedRows > 0)
                {
                    _logger.Info($"Đã đánh dấu {quantity} bao bì hỏng - PackagingId: {packagingId}");
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.Error($"MarkAsDamagedAsync error - PackagingId: {packagingId}, Quantity: {quantity} - {ex.Message}");
                throw;
            }
        }

        public async Task<PackagingQuantities> GetPackagingQuantitiesAsync(int packagingId)
        {
            try
            {
                await EnsurePackagingInventoryExistsAsync(packagingId);

                var result = await _databaseHelper.QueryFirstOrDefaultAsync<PackagingQuantities>(
                    PackagingInventoryQueries.GetPackagingQuantities,
                    new { PackagingId = packagingId }
                );

                return result ?? new PackagingQuantities();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetPackagingQuantitiesAsync error - PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsNewPackagingSufficientAsync(int packagingId, int requiredQuantity)
        {
            try
            {
                if (requiredQuantity <= 0)
                    return true;

                await EnsurePackagingInventoryExistsAsync(packagingId);

                var result = await _databaseHelper.ExecuteScalarAsync<int?>(
                    PackagingInventoryQueries.IsNewPackagingSufficient,
                    new { PackagingId = packagingId, RequiredQuantity = requiredQuantity }
                );

                return result.HasValue;
            }
            catch (Exception ex)
            {
                _logger.Error($"IsNewPackagingSufficientAsync error - PackagingId: {packagingId}, Required: {requiredQuantity} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Đảm bảo tồn kho bao bì tồn tại, nếu chưa thì tạo mới
        /// </summary>
        private async Task EnsurePackagingInventoryExistsAsync(int packagingId)
        {
            try
            {
                var existing = await _databaseHelper.QueryFirstOrDefaultAsync<PackagingInventory>(
                    PackagingInventoryQueries.GetByPackaging,
                    new { PackagingId = packagingId }
                );

                if (existing == null)
                {
                    await _databaseHelper.ExecuteAsync(
                        PackagingInventoryQueries.CreatePackagingInventory,
                        new { PackagingId = packagingId }
                    );
                    _logger.Info($"Đã tạo mới tồn kho bao bì - PackagingId: {packagingId}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"EnsurePackagingInventoryExistsAsync error - PackagingId: {packagingId} - {ex.Message}");
                throw;
            }
        }
    }
}
