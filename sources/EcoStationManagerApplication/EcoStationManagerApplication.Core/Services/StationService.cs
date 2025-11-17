using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class StationService : BaseService, IStationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StationService(IUnitOfWork unitOfWork)
            : base("StationService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Station>> GetStationByIdAsync(int stationId)
        {
            try
            {
                if (stationId <= 0)
                    return NotFoundError<Station>("Trạm", stationId);

                var station = await _unitOfWork.Stations.GetByIdAsync(stationId);
                if (station == null)
                    return NotFoundError<Station>("Trạm", stationId);

                return Result<Station>.Ok(station);
            }
            catch (Exception ex)
            {
                return HandleException<Station>(ex, "lấy thông tin trạm");
            }
        }

        public async Task<Result<IEnumerable<Station>>> GetAllStationsAsync()
        {
            try
            {
                var stations = await _unitOfWork.Stations.GetAllAsync();
                return Result<IEnumerable<Station>>.Ok(stations);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Station>>(ex, "lấy danh sách trạm");
            }
        }

        public async Task<Result<IEnumerable<Station>>> GetActiveStationsAsync()
        {
            try
            {
                var stations = await _unitOfWork.Stations.GetActiveStationsAsync();
                return Result<IEnumerable<Station>>.Ok(stations);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Station>>(ex, "lấy danh sách trạm đang hoạt động");
            }
        }

        public async Task<Result<IEnumerable<Station>>> GetStationsByManagerAsync(int managerId)
        {
            try
            {
                if (managerId <= 0)
                    return Result<IEnumerable<Station>>.Fail("Manager ID không hợp lệ");

                var stations = await _unitOfWork.Stations.GetByManagerAsync(managerId);
                return Result<IEnumerable<Station>>.Ok(stations);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Station>>(ex, "lấy danh sách trạm theo quản lý");
            }
        }

        public async Task<Result<IEnumerable<Station>>> SearchStationsAsync(string keyword)
        {
            try
            {
                var stations = await _unitOfWork.Stations.SearchStationsAsync(keyword);
                return Result<IEnumerable<Station>>.Ok(stations);
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<Station>>(ex, "tìm kiếm trạm");
            }
        }

        public async Task<Result<int>> CreateStationAsync(Station station)
        {
            try
            {
                if (station == null)
                    return Result<int>.Fail("Thông tin trạm không được để trống");

                // Validation
                if (string.IsNullOrWhiteSpace(station.Name))
                    return Result<int>.Fail("Tên trạm không được để trống");

                if (string.IsNullOrWhiteSpace(station.Address))
                    return Result<int>.Fail("Địa chỉ trạm không được để trống");

                // Kiểm tra tên trạm đã tồn tại chưa
                var existingStation = await _unitOfWork.Stations.GetByNameAsync(station.Name);
                if (existingStation != null)
                    return Result<int>.Fail($"Trạm với tên '{station.Name}' đã tồn tại");

                // Validate và set default station_type
                var validStationTypes = new[] { "warehouse", "refill", "hybrid", "other" };
                if (string.IsNullOrWhiteSpace(station.StationType) || !validStationTypes.Contains(station.StationType.ToLower()))
                    station.StationType = "refill";
                else
                    station.StationType = station.StationType.ToLower();

                station.IsActive = true;
                station.CreatedDate = DateTime.Now;
                station.UpdatedDate = DateTime.Now;

                // Kiểm tra manager nếu có
                if (station.Manager.HasValue && station.Manager.Value > 0)
                {
                    var manager = await _unitOfWork.Users.GetByIdAsync(station.Manager.Value);
                    if (manager == null)
                        return Result<int>.Fail($"Không tìm thấy người quản lý với ID: {station.Manager.Value}");
                }

                var stationId = await _unitOfWork.Stations.AddAsync(station);
                _logger.Info($"Đã tạo trạm mới - StationId: {stationId}, Name: {station.Name}");

                return Result<int>.Ok(stationId, "Tạo trạm thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo trạm");
            }
        }

        public async Task<Result<bool>> UpdateStationAsync(Station station)
        {
            try
            {
                if (station == null || station.StationId <= 0)
                    return Result<bool>.Fail("Thông tin trạm không hợp lệ");

                // Validation - Name và Address có thể NULL trong DB nhưng vẫn validate khi update
                if (string.IsNullOrWhiteSpace(station.Name))
                    return Result<bool>.Fail("Tên trạm không được để trống");

                if (string.IsNullOrWhiteSpace(station.Address))
                    return Result<bool>.Fail("Địa chỉ trạm không được để trống");

                // Validate station_type
                var validStationTypes = new[] { "warehouse", "refill", "hybrid", "other" };
                if (!string.IsNullOrWhiteSpace(station.StationType) && !validStationTypes.Contains(station.StationType.ToLower()))
                    return Result<bool>.Fail($"Loại trạm không hợp lệ. Chỉ chấp nhận: {string.Join(", ", validStationTypes)}");
                
                if (string.IsNullOrWhiteSpace(station.StationType))
                    station.StationType = "refill";
                else
                    station.StationType = station.StationType.ToLower();

                // Kiểm tra trạm có tồn tại không
                var existingStation = await _unitOfWork.Stations.GetByIdAsync(station.StationId);
                if (existingStation == null)
                    return NotFoundError<bool>("Trạm", station.StationId);

                // Kiểm tra tên trạm đã tồn tại ở trạm khác chưa
                var stationWithSameName = await _unitOfWork.Stations.GetByNameAsync(station.Name);
                if (stationWithSameName != null && stationWithSameName.StationId != station.StationId)
                    return Result<bool>.Fail($"Trạm với tên '{station.Name}' đã tồn tại");

                // Kiểm tra manager nếu có
                if (station.Manager.HasValue && station.Manager.Value > 0)
                {
                    var manager = await _unitOfWork.Users.GetByIdAsync(station.Manager.Value);
                    if (manager == null)
                        return Result<bool>.Fail($"Không tìm thấy người quản lý với ID: {station.Manager.Value}");
                }

                station.UpdatedDate = DateTime.Now;
                var success = await _unitOfWork.Stations.UpdateAsync(station);

                if (success)
                {
                    _logger.Info($"Đã cập nhật trạm - StationId: {station.StationId}, Name: {station.Name}");
                    return Result<bool>.Ok(true, "Cập nhật trạm thành công");
                }

                return Result<bool>.Fail("Không thể cập nhật trạm");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật trạm");
            }
        }

        public async Task<Result<bool>> DeleteStationAsync(int stationId)
        {
            try
            {
                if (stationId <= 0)
                    return Result<bool>.Fail("ID trạm không hợp lệ");

                var station = await _unitOfWork.Stations.GetByIdAsync(stationId);
                if (station == null)
                    return NotFoundError<bool>("Trạm", stationId);

                // TODO: Kiểm tra xem trạm có đang được sử dụng trong Orders, StockIn, StockOut không
                // Nếu có thì không cho xóa, chỉ cho vô hiệu hóa

                var success = await _unitOfWork.Stations.DeleteAsync(stationId);
                if (success)
                {
                    _logger.Info($"Đã xóa trạm - StationId: {stationId}");
                    return Result<bool>.Ok(true, "Xóa trạm thành công");
                }

                return Result<bool>.Fail("Không thể xóa trạm");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa trạm");
            }
        }

        public async Task<Result<bool>> ToggleStationStatusAsync(int stationId, bool isActive)
        {
            try
            {
                if (stationId <= 0)
                    return Result<bool>.Fail("ID trạm không hợp lệ");

                var station = await _unitOfWork.Stations.GetByIdAsync(stationId);
                if (station == null)
                    return NotFoundError<bool>("Trạm", stationId);

                var success = await _unitOfWork.Stations.ToggleStatusAsync(stationId, isActive);
                if (success)
                {
                    var status = isActive ? "kích hoạt" : "vô hiệu hóa";
                    _logger.Info($"Đã {status} trạm - StationId: {stationId}");
                    return Result<bool>.Ok(true, $"Đã {status} trạm thành công");
                }

                return Result<bool>.Fail("Không thể thay đổi trạng thái trạm");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "thay đổi trạng thái trạm");
            }
        }
    }
}

