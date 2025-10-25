using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public interface IAutoUpdateService
    {
        Task<UpdateInfo> CheckForUpdates();
        Task<bool> DownloadUpdate();
        Task<bool> InstallUpdate();
        Version GetCurrentVersion();
    }

    public class AutoUpdateService : IAutoUpdateService
    {
        private readonly INetworkService _networkService;
        private readonly string updateUrl = "https://api.ecostation.com/updates";

        public AutoUpdateService(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public async Task<UpdateInfo> CheckForUpdates()
        {
            if (!await _networkService.CheckInternetConnectionAsync())
                return new UpdateInfo { UpdateAvailable = false };

            try
            {
                var currentVersion = GetCurrentVersion();
                var latestVersion = await GetLatestVersionFromServer();

                return new UpdateInfo
                {
                    UpdateAvailable = latestVersion > currentVersion,
                    CurrentVersion = currentVersion,
                    LatestVersion = latestVersion,
                    ReleaseNotes = await GetReleaseNotes(latestVersion),
                    DownloadSize = await GetDownloadSize(latestVersion)
                };
            }
            catch
            {
                return new UpdateInfo { UpdateAvailable = false };
            }
        }

        public async Task<bool> DownloadUpdate()
        {
            var updateInfo = await CheckForUpdates();
            if (!updateInfo.UpdateAvailable)
                return false;

            try
            {
                var tempPath = Path.GetTempPath();
                var updateFile = Path.Combine(tempPath, $"EcoStation_Update_{updateInfo.LatestVersion}.msi");

                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(updateInfo.DownloadUrl, updateFile);
                }

                return File.Exists(updateFile);
            }
            catch (Exception ex)
            {
                throw new UpdateException($"Lỗi tải bản cập nhật: {ex.Message}", ex);
            }
        }

        public async Task<bool> InstallUpdate()
        {
            var updateInfo = await CheckForUpdates();
            if (!updateInfo.UpdateAvailable)
                return false;

            try
            {
                var tempPath = Path.GetTempPath();
                var updateFile = Path.Combine(tempPath, $"EcoStation_Update_{updateInfo.LatestVersion}.msi");

                if (!File.Exists(updateFile))
                {
                    await DownloadUpdate();
                }

                // Launch installer
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "msiexec",
                        Arguments = $"/i \"{updateFile}\" /quiet /norestart",
                        UseShellExecute = true
                    }
                };

                process.Start();
                return true;
            }
            catch (Exception ex)
            {
                throw new UpdateException($"Lỗi cài đặt bản cập nhật: {ex.Message}", ex);
            }
        }

        public Version GetCurrentVersion()
        {
            return Version.Parse(AppConfigHelper.Version);
        }

        private async Task<Version> GetLatestVersionFromServer()
        {
            // Simulate API call - in real implementation, this would call your update server
            await Task.Delay(1000);
            return new Version("1.1.0");
        }

        private async Task<string> GetReleaseNotes(Version version)
        {
            // Simulate API call
            await Task.Delay(500);
            return $"Phiên bản {version} - Cải thiện hiệu năng và sửa lỗi";
        }

        private async Task<long> GetDownloadSize(Version version)
        {
            // Simulate API call
            await Task.Delay(500);
            return 50 * 1024 * 1024; // 50MB
        }
    }

    public class UpdateInfo
    {
        public bool UpdateAvailable { get; set; }
        public Version CurrentVersion { get; set; }
        public Version LatestVersion { get; set; }
        public string ReleaseNotes { get; set; }
        public long DownloadSize { get; set; }
        public string DownloadUrl => $"https://downloads.ecostation.com/EcoStation_{LatestVersion}.msi";
    }

    public class UpdateException : Exception
    {
        public UpdateException(string message) : base(message) { }
        public UpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}