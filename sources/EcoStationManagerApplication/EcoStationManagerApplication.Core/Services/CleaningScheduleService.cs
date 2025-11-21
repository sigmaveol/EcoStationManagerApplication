using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class CleaningScheduleService : BaseService, ICleaningScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CleaningScheduleService(IUnitOfWork unitOfWork)
            : base("CleaningScheduleService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<CleaningSchedule>>> GetAllAsync()
        {
            try
            {
                var schedules = await _unitOfWork.CleaningSchedules.GetAllAsync();
                var ordered = schedules?
                    .OrderByDescending(s => s.CleaningDate)
                    .ToList() ?? new List<CleaningSchedule>();

                return Result<IEnumerable<CleaningSchedule>>.Ok(
                    ordered,
                    $"Đã tải {ordered.Count} lịch vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CleaningSchedule>>(ex, "lấy danh sách lịch vệ sinh");
            }
        }

        public async Task<Result<CleaningSchedule>> GetByIdAsync(int cleaningScheduleId)
        {
            try
            {
                if (cleaningScheduleId <= 0)
                    return ValidationError<CleaningSchedule>(new List<string> { "ID lịch vệ sinh không hợp lệ" });

                var schedule = await _unitOfWork.CleaningSchedules.GetByIdAsync(cleaningScheduleId);
                if (schedule == null)
                    return NotFoundError<CleaningSchedule>("Lịch vệ sinh", cleaningScheduleId);

                return Result<CleaningSchedule>.Ok(schedule, "Đã tải thông tin lịch vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<CleaningSchedule>(ex, "lấy thông tin lịch vệ sinh");
            }
        }

        public async Task<Result<IEnumerable<CleaningSchedule>>> GetByStationIdAsync(int stationId)
        {
            try
            {
                // V0 không hỗ trợ đa trạm nên trả về toàn bộ danh sách
                return await GetAllAsync();
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CleaningSchedule>>(ex, "lấy lịch vệ sinh");
            }
        }

        public async Task<Result<IEnumerable<CleaningSchedule>>> GetUpcomingSchedulesAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var from = (fromDate ?? DateTime.Now).Date;
                var to = (toDate ?? DateTime.Now.AddDays(7)).Date.AddDays(1).AddTicks(-1);

                if (from > to)
                {
                    return ValidationError<IEnumerable<CleaningSchedule>>(new List<string>
                    {
                        "Ngày bắt đầu không được lớn hơn ngày kết thúc"
                    });
                }

                var schedules = await _unitOfWork.CleaningSchedules.GetUpcomingSchedulesAsync(from, to);
                return Result<IEnumerable<CleaningSchedule>>.Ok(
                    schedules,
                    $"Đã tải {schedules?.Count() ?? 0} lịch vệ sinh trong khoảng thời gian");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CleaningSchedule>>(ex, "lấy lịch vệ sinh sắp đến hạn");
            }
        }

        public async Task<Result<IEnumerable<CleaningSchedule>>> GetOverdueSchedulesAsync()
        {
            try
            {
                var schedules = await _unitOfWork.CleaningSchedules.GetOverdueSchedulesAsync();
                return Result<IEnumerable<CleaningSchedule>>.Ok(
                    schedules,
                    $"Đã tải {schedules?.Count() ?? 0} lịch vệ sinh quá hạn");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<CleaningSchedule>>(ex, "lấy lịch vệ sinh quá hạn");
            }
        }

        public async Task<Result<int>> CreateAsync(CleaningSchedule cleaningSchedule)
        {
            try
            {
                var errors = new List<string>();

                if (cleaningSchedule == null)
                    return Result<int>.Fail("Thông tin lịch vệ sinh không được để trống");

                if (cleaningSchedule.CleaningDate == default)
                    errors.Add("Ngày vệ sinh không hợp lệ");

                if (cleaningSchedule.CleaningDate.Date < DateTime.Now.Date)
                    errors.Add("Ngày vệ sinh không được trong quá khứ");

                if (errors.Any())
                    return ValidationError<int>(errors);

                cleaningSchedule.Status = CleaningStatus.SCHEDULED;
                cleaningSchedule.CreatedDate = DateTime.Now;

                var scheduleId = await _unitOfWork.CleaningSchedules.AddAsync(cleaningSchedule);
                _logger.Info($"Đã tạo lịch vệ sinh mới - Id: {scheduleId}, Type: {cleaningSchedule.CleaningType}, Date: {cleaningSchedule.CleaningDate:dd/MM/yyyy HH:mm}");

                return Result<int>.Ok(scheduleId, "Đã tạo lịch vệ sinh thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo lịch vệ sinh");
            }
        }

        public async Task<Result<bool>> UpdateAsync(CleaningSchedule cleaningSchedule)
        {
            try
            {
                if (cleaningSchedule == null || cleaningSchedule.CsId <= 0)
                    return Result<bool>.Fail("Thông tin lịch vệ sinh không hợp lệ");

                var existing = await _unitOfWork.CleaningSchedules.GetByIdAsync(cleaningSchedule.CsId);
                if (existing == null)
                    return NotFoundError<bool>("Lịch vệ sinh", cleaningSchedule.CsId);

                existing.CleaningType = cleaningSchedule.CleaningType;
                existing.CleaningDate = cleaningSchedule.CleaningDate;
                existing.CleaningBy = cleaningSchedule.CleaningBy;
                existing.Status = cleaningSchedule.Status;
                existing.Notes = cleaningSchedule.Notes;

                var updated = await _unitOfWork.CleaningSchedules.UpdateAsync(existing);
                if (!updated)
                    return BusinessError<bool>("Không thể cập nhật lịch vệ sinh");

                return Result<bool>.Ok(true, "Đã cập nhật lịch vệ sinh thành công");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "cập nhật lịch vệ sinh");
            }
        }

        public async Task<Result<bool>> MarkAsCompletedAsync(int cleaningScheduleId, int? cleanedBy = null, string notes = null)
        {
            try
            {
                if (cleaningScheduleId <= 0)
                    return Result<bool>.Fail("ID lịch vệ sinh không hợp lệ");

                var schedule = await _unitOfWork.CleaningSchedules.GetByIdAsync(cleaningScheduleId);
                if (schedule == null)
                    return NotFoundError<bool>("Lịch vệ sinh", cleaningScheduleId);

                if (schedule.Status == CleaningStatus.CANCELLED)
                    return BusinessError<bool>("Không thể hoàn thành lịch đã bị hủy");

                schedule.Status = CleaningStatus.COMPLETED;
                schedule.CleaningBy = cleanedBy ?? schedule.CleaningBy;
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    schedule.Notes = string.IsNullOrWhiteSpace(schedule.Notes)
                        ? notes
                        : $"{schedule.Notes}\n{notes}";
                }

                var updated = await _unitOfWork.CleaningSchedules.UpdateAsync(schedule);
                if (!updated)
                    return BusinessError<bool>("Không thể cập nhật trạng thái lịch vệ sinh");

                return Result<bool>.Ok(true, "Đã đánh dấu hoàn thành lịch vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "đánh dấu hoàn thành vệ sinh");
            }
        }

        public async Task<Result<bool>> CancelAsync(int cleaningScheduleId, string reason = null)
        {
            try
            {
                if (cleaningScheduleId <= 0)
                    return Result<bool>.Fail("ID lịch vệ sinh không hợp lệ");

                var schedule = await _unitOfWork.CleaningSchedules.GetByIdAsync(cleaningScheduleId);
                if (schedule == null)
                    return NotFoundError<bool>("Lịch vệ sinh", cleaningScheduleId);

                if (schedule.Status == CleaningStatus.COMPLETED)
                    return BusinessError<bool>("Không thể hủy lịch đã hoàn thành");

                schedule.Status = CleaningStatus.CANCELLED;
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    schedule.Notes = string.IsNullOrWhiteSpace(schedule.Notes)
                        ? $"Lý do hủy: {reason}"
                        : $"{schedule.Notes}\nLý do hủy: {reason}";
                }

                var updated = await _unitOfWork.CleaningSchedules.UpdateAsync(schedule);
                if (!updated)
                    return BusinessError<bool>("Không thể hủy lịch vệ sinh");

                return Result<bool>.Ok(true, "Đã hủy lịch vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "hủy lịch vệ sinh");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int cleaningScheduleId)
        {
            try
            {
                if (cleaningScheduleId <= 0)
                    return Result<bool>.Fail("ID lịch vệ sinh không hợp lệ");

                var schedule = await _unitOfWork.CleaningSchedules.GetByIdAsync(cleaningScheduleId);
                if (schedule == null)
                    return NotFoundError<bool>("Lịch vệ sinh", cleaningScheduleId);

                var deleted = await _unitOfWork.CleaningSchedules.DeleteAsync(cleaningScheduleId);
                if (!deleted)
                    return BusinessError<bool>("Không thể xóa lịch vệ sinh");

                return Result<bool>.Ok(true, "Đã xóa lịch vệ sinh");
            }
            catch (Exception ex)
            {
                return HandleException<bool>(ex, "xóa lịch vệ sinh");
            }
        }
    }
}

