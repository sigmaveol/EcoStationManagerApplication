using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public interface INetworkService
    {
        Task<bool> CheckInternetConnectionAsync();
        Task<bool> CheckApiAvailabilityAsync(string url);
    }

    public class NetworkService : INetworkService
    {
        public async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckApiAvailabilityAsync(string url)
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    await client.DownloadStringTaskAsync(url);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}