using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.Interfaces
{
    public interface IStationRepository : IRepository<Station>
    {
        /// <summary>
        /// Lấy tất cả trạm đang active
        /// </summary>
        Task<IEnumerable<Station>> GetActiveStationsAsync();

        /// <summary>
        /// Lấy trạm theo manager (quản lý trạm)
        /// </summary>
        Task<IEnumerable<Station>> GetByManagerAsync(int managerId);

        /// <summary>
        /// Lấy trạm theo tên
        /// </summary>
        Task<Station> GetByNameAsync(string name);

        /// <summary>
        /// Tìm kiếm trạm theo keyword
        /// </summary>
        Task<IEnumerable<Station>> SearchStationsAsync(string keyword);

        /// <summary>
        /// Bật/tắt trạng thái active của trạm
        /// </summary>
        Task<bool> ToggleStatusAsync(int stationId, bool isActive);
    }
}

