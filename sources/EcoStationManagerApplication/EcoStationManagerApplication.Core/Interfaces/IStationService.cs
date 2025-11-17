using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IStationService
    {
        /// <summary>
        /// Lấy thông tin trạm theo ID
        /// </summary>
        Task<Result<Station>> GetStationByIdAsync(int stationId);

        /// <summary>
        /// Lấy tất cả trạm
        /// </summary>
        Task<Result<IEnumerable<Station>>> GetAllStationsAsync();

        /// <summary>
        /// Lấy tất cả trạm đang active
        /// </summary>
        Task<Result<IEnumerable<Station>>> GetActiveStationsAsync();

        /// <summary>
        /// Lấy trạm theo manager
        /// </summary>
        Task<Result<IEnumerable<Station>>> GetStationsByManagerAsync(int managerId);

        /// <summary>
        /// Tìm kiếm trạm
        /// </summary>
        Task<Result<IEnumerable<Station>>> SearchStationsAsync(string keyword);

        /// <summary>
        /// Tạo trạm mới
        /// </summary>
        Task<Result<int>> CreateStationAsync(Station station);

        /// <summary>
        /// Cập nhật thông tin trạm
        /// </summary>
        Task<Result<bool>> UpdateStationAsync(Station station);

        /// <summary>
        /// Xóa trạm
        /// </summary>
        Task<Result<bool>> DeleteStationAsync(int stationId);

        /// <summary>
        /// Bật/tắt trạng thái trạm
        /// </summary>
        Task<Result<bool>> ToggleStationStatusAsync(int stationId, bool isActive);
    }
}

