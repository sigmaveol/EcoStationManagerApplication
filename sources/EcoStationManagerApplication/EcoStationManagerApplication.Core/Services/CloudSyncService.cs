using EcoStationManagerApplication.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public interface ICloudSyncService
    {
        Task<bool> SyncToCloudAsync();
        Task<bool> SyncFromCloudAsync();
        Task<bool> BackupToCloudAsync();
        Task<List<SyncConflict>> ResolveConflictsAsync();
        CloudSyncStatus GetSyncStatus();
    }

    public class CloudSyncService : ICloudSyncService
    {
        private readonly IDbHelper _dbHelper;
        private readonly INetworkService _networkService;
        private readonly IOrderService _orderService;
        private readonly IInventoryService _inventoryService;

        public CloudSyncService(
            IDbHelper dbHelper,
            INetworkService networkService,
            IOrderService orderService,
            IInventoryService inventoryService)
        {
            _dbHelper = dbHelper;
            _networkService = networkService;
            _orderService = orderService;
            _inventoryService = inventoryService;
        }

        public async Task<bool> SyncToCloudAsync()
        {
            if (!await _networkService.CheckInternetConnectionAsync())
                return false;

            try
            {
                // Sync orders
                var recentOrders = await _orderService.GetOrdersByDateRangeAsync(
                    DateTime.Now.AddDays(-1), DateTime.Now);
                await SyncOrdersToCloud(recentOrders);

                // Sync inventory changes
                var inventoryChanges = await GetRecentInventoryChanges();
                await SyncInventoryToCloud(inventoryChanges);

                // Update sync timestamp
                await UpdateLastSyncTime();

                return true;
            }
            catch (Exception ex)
            {
                throw new CloudSyncException($"Lỗi đồng bộ lên cloud: {ex.Message}", ex);
            }
        }

        public async Task<bool> SyncFromCloudAsync()
        {
            if (!await _networkService.CheckInternetConnectionAsync())
                return false;

            try
            {
                // Download new orders from cloud
                var cloudOrders = await DownloadOrdersFromCloud();
                await ImportCloudOrders(cloudOrders);

                // Download inventory updates
                var cloudInventory = await DownloadInventoryFromCloud();
                await UpdateLocalInventory(cloudInventory);

                return true;
            }
            catch (Exception ex)
            {
                throw new CloudSyncException($"Lỗi đồng bộ từ cloud: {ex.Message}", ex);
            }
        }

        public async Task<bool> BackupToCloudAsync()
        {
            if (!await _networkService.CheckInternetConnectionAsync())
                return false;

            try
            {
                // Create database backup
                var backupFile = await CreateLocalBackup();

                // Upload to cloud storage
                await UploadToCloudStorage(backupFile);

                // Cleanup local backup
                File.Delete(backupFile);

                return true;
            }
            catch (Exception ex)
            {
                throw new CloudSyncException($"Lỗi backup lên cloud: {ex.Message}", ex);
            }
        }

        public async Task<List<SyncConflict>> ResolveConflictsAsync()
        {
            var conflicts = new List<SyncConflict>();

            // Detect conflicts between local and cloud data
            var orderConflicts = await DetectOrderConflicts();
            conflicts.AddRange(orderConflicts);

            var inventoryConflicts = await DetectInventoryConflicts();
            conflicts.AddRange(inventoryConflicts);

            return conflicts;
        }

        public CloudSyncStatus GetSyncStatus()
        {
            return new CloudSyncStatus
            {
                LastSyncTime = GetLastSyncTime(),
                IsConnected = _networkService.CheckInternetConnectionAsync().Result,
                PendingChanges = GetPendingChangeCount()
            };
        }

        private async Task<string> CreateLocalBackup()
        {
            var backupPath = Path.Combine(AppConfigHelper.BackupPath,
                $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql");

            using (var connection = await _dbHelper.GetOpenConnectionAsync())
            {
                // Use mysqldump to create backup
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "mysqldump",
                        Arguments = $"-u {GetDbUser()} -p{GetDbPassword()} {GetDbName()} > \"{backupPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                    throw new Exception("Lỗi tạo backup database");
            }

            return backupPath;
        }
    }

    public class CloudSyncStatus
    {
        public DateTime LastSyncTime { get; set; }
        public bool IsConnected { get; set; }
        public int PendingChanges { get; set; }
        public string StatusMessage => IsConnected ?
            $"Đã đồng bộ: {LastSyncTime:g}" :
            "Chưa kết nối internet";
    }

    public class SyncConflict
    {
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string LocalValue { get; set; }
        public string CloudValue { get; set; }
        public ConflictResolution Resolution { get; set; }
    }

    public enum ConflictResolution
    {
        KeepLocal,
        UseCloud,
        Manual
    }

    public class CloudSyncException : Exception
    {
        public CloudSyncException(string message) : base(message) { }
        public CloudSyncException(string message, Exception innerException) : base(message, innerException) { }
    }
}