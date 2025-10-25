using EcoStationManagerApplication.Core.Exceptions;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Models;
using EcoStationManagerApplication.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class StationService : BaseService<Station>, IStationService
    {
        private readonly IStationRepository _stationRepository;

        public StationService(IStationRepository stationRepository) : base(stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<IEnumerable<Station>> GetActiveStationsAsync()
        {
            return await _stationRepository.GetActiveStationsAsync();
        }

        public async Task<IEnumerable<Station>> GetStationsByTypeAsync(string stationType)
        {
            if (string.IsNullOrWhiteSpace(stationType))
                throw new ArgumentException("Loại trạm không được để trống", nameof(stationType));

            return await _stationRepository.GetStationsByTypeAsync(stationType);
        }

        public async Task<IEnumerable<Station>> GetChildStationsAsync(int parentStationId)
        {
            if (parentStationId <= 0)
                throw new ArgumentException("Parent Station ID phải lớn hơn 0", nameof(parentStationId));

            return await _stationRepository.GetChildStationsAsync(parentStationId);
        }

        public async Task<bool> UpdateStationStatusAsync(int stationId, bool isActive)
        {
            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            return await _stationRepository.UpdateStationStatusAsync(stationId, isActive);
        }

        protected override async Task ValidateEntityAsync(Station station)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            if (string.IsNullOrWhiteSpace(station.Name))
                throw new ValidationException("Tên trạm là bắt buộc");

            if (string.IsNullOrWhiteSpace(station.Address))
                throw new ValidationException("Địa chỉ trạm là bắt buộc");

            if (string.IsNullOrWhiteSpace(station.StationType.ToString()))
                throw new ValidationException("Loại trạm là bắt buộc");

            // Kiểm tra parent station tồn tại nếu có
            if (station.ParentStationId.HasValue)
            {
                var parentStation = await _stationRepository.GetByIdAsync(station.ParentStationId.Value);
                if (parentStation == null)
                    throw new ValidationException($"Trạm cha với ID {station.ParentStationId} không tồn tại");
            }
        }
    }

    public class TankService : BaseService<Tank>, ITankService
    {
        private readonly ITankRepository _tankRepository;
        private readonly IStationRepository _stationRepository;

        public TankService(ITankRepository tankRepository, IStationRepository stationRepository)
            : base(tankRepository)
        {
            _tankRepository = tankRepository;
            _stationRepository = stationRepository;
        }

        public async Task<IEnumerable<Tank>> GetTanksByStationAsync(int stationId)
        {
            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            return await _tankRepository.GetTanksByStationAsync(stationId);
        }

        public async Task<bool> UpdateTankLevelAsync(int tankId, decimal currentLevel)
        {
            if (tankId <= 0)
                throw new ArgumentException("Tank ID phải lớn hơn 0", nameof(tankId));

            if (currentLevel < 0)
                throw new ArgumentException("Mức nước hiện tại không được âm", nameof(currentLevel));

            var tank = await _tankRepository.GetByIdAsync(tankId);
            if (tank == null)
                throw new NotFoundException($"Bồn chứa với ID {tankId} không tồn tại");

            if (currentLevel > tank.Capacity)
                throw new ValidationException($"Mức nước không thể vượt quá dung tích ({tank.Capacity} {tank.Unit})");

            return await _tankRepository.UpdateTankLevelAsync(tankId, currentLevel);
        }

        public async Task<IEnumerable<Tank>> GetTanksNeedCleaningAsync()
        {
            return await _tankRepository.GetTanksNeedCleaningAsync();
        }

        public async Task<bool> ScheduleTankCleaningAsync(int tankId, DateTime cleaningDate)
        {
            if (tankId <= 0)
                throw new ArgumentException("Tank ID phải lớn hơn 0", nameof(tankId));

            if (cleaningDate <= DateTime.Now)
                throw new ArgumentException("Ngày vệ sinh phải trong tương lai", nameof(cleaningDate));

            return await _tankRepository.UpdateTankCleaningDateAsync(tankId, cleaningDate);
        }

        protected override async Task ValidateEntityAsync(Tank tank)
        {
            if (tank == null)
                throw new ArgumentNullException(nameof(tank));

            if (string.IsNullOrWhiteSpace(tank.Name))
                throw new ValidationException("Tên bồn chứa là bắt buộc");

            if (tank.Capacity <= 0)
                throw new ValidationException("Dung tích phải lớn hơn 0");

            if (tank.CurrentLevel < 0)
                throw new ValidationException("Mức nước hiện tại không được âm");

            if (tank.CurrentLevel > tank.Capacity)
                throw new ValidationException("Mức nước hiện tại không thể vượt quá dung tích");

            if (tank.StationId <= 0)
                throw new ValidationException("Station ID không hợp lệ");

            // Kiểm tra station tồn tại
            var station = await _stationRepository.GetByIdAsync(tank.StationId);
            if (station == null)
                throw new ValidationException($"Trạm với ID {tank.StationId} không tồn tại");
        }
    }

    public class CleaningService : BaseService<CleaningSchedule>, ICleaningService
    {
        private readonly ICleaningScheduleRepository _cleaningScheduleRepository;
        private readonly IStationRepository _stationRepository;

        public CleaningService(
            ICleaningScheduleRepository cleaningScheduleRepository,
            IStationRepository stationRepository)
            : base(cleaningScheduleRepository)
        {
            _cleaningScheduleRepository = cleaningScheduleRepository;
            _stationRepository = stationRepository;
        }

        public async Task<IEnumerable<CleaningSchedule>> GetCleaningSchedulesByStationAsync(int stationId)
        {
            if (stationId <= 0)
                throw new ArgumentException("Station ID phải lớn hơn 0", nameof(stationId));

            return await _cleaningScheduleRepository.GetCleaningSchedulesByStationAsync(stationId);
        }

        public async Task<IEnumerable<CleaningSchedule>> GetOverdueCleaningSchedulesAsync()
        {
            return await _cleaningScheduleRepository.GetOverdueCleaningSchedulesAsync();
        }

        public async Task<bool> CompleteCleaningAsync(int cleaningScheduleId, int cleanedBy)
        {
            if (cleaningScheduleId <= 0)
                throw new ArgumentException("Cleaning Schedule ID phải lớn hơn 0", nameof(cleaningScheduleId));

            if (cleanedBy <= 0)
                throw new ArgumentException("User ID phải lớn hơn 0", nameof(cleanedBy));

            return await _cleaningScheduleRepository.CompleteCleaningAsync(cleaningScheduleId, cleanedBy);
        }

        public async Task<bool> ScheduleCleaningAsync(CleaningSchedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            // Validate schedule
            await ValidateCleaningScheduleAsync(schedule);

            return await _cleaningScheduleRepository.CreateAsync(schedule) != null;
        }

        private async Task ValidateCleaningScheduleAsync(CleaningSchedule schedule)
        {
            if (schedule.StationId <= 0)
                throw new ValidationException("Station ID không hợp lệ");

            if (schedule.CleaningDate <= DateTime.Now)
                throw new ValidationException("Ngày vệ sinh phải trong tương lai");

            if (string.IsNullOrWhiteSpace(schedule.CleaningType))
                throw new ValidationException("Loại vệ sinh là bắt buộc");

            // Kiểm tra station tồn tại
            var station = await _stationRepository.GetByIdAsync(schedule.StationId);
            if (station == null)
                throw new ValidationException($"Trạm với ID {schedule.StationId} không tồn tại");
        }

        protected override async Task ValidateEntityAsync(CleaningSchedule schedule)
        {
            await ValidateCleaningScheduleAsync(schedule);
        }
    }
}