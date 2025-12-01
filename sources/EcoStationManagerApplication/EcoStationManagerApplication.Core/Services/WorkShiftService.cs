using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class WorkShiftService : BaseService, IWorkShiftService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkShiftService(IUnitOfWork unitOfWork)
            : base("WorkShiftService")
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<WorkShift>>> GetAllAsync()
        {
            try
            {
                var shifts = await _unitOfWork.WorkShifts.GetAllAsync();
                var shiftsList = shifts?.ToList() ?? new List<WorkShift>();
                
                return Result<IEnumerable<WorkShift>>.Ok(
                    shiftsList, 
                    shiftsList.Any() 
                        ? $"Đã tải {shiftsList.Count} ca làm việc" 
                        : "Không có ca làm việc nào");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<WorkShift>>(ex, "lấy danh sách ca làm việc");
            }
        }

        public async Task<Result<WorkShift>> GetByIdAsync(int shiftId)
        {
            try
            {
                if (shiftId <= 0)
                {
                    return ValidationError<WorkShift>(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                var shift = await _unitOfWork.WorkShifts.GetByIdAsync(shiftId);
                if (shift == null)
                {
                    return NotFoundError<WorkShift>("ca làm việc", shiftId);
                }

                return Result<WorkShift>.Ok(shift, "Đã tải thông tin ca làm việc");
            }
            catch (Exception ex)
            {
                return HandleException<WorkShift>(ex, "lấy thông tin ca làm việc");
            }
        }

        public async Task<Result<WorkShift>> GetByUserIdAndDateAsync(int userId, DateTime shiftDate)
        {
            try
            {
                if (userId <= 0)
                {
                    return ValidationError<WorkShift>(new List<string> { "ID người dùng không hợp lệ" });
                }

                var shift = await _unitOfWork.WorkShifts.GetByUserIdAndDateAsync(userId, shiftDate);
                if (shift == null)
                {
                    return NotFoundError<WorkShift>($"Không tìm thấy ca làm việc cho người dùng {userId} vào ngày {shiftDate:dd/MM/yyyy}");
                }

                return Result<WorkShift>.Ok(shift, "Đã tải thông tin ca làm việc");
            }
            catch (Exception ex)
            {
                return HandleException<WorkShift>(ex, "lấy ca làm việc theo người dùng và ngày");
            }
        }

        public async Task<Result<WorkShift>> GetCurrentShiftByUserIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return ValidationError<WorkShift>(new List<string> { "ID người dùng không hợp lệ" });
                }

                var shift = await _unitOfWork.WorkShifts.GetCurrentShiftByUserIdAsync(userId);
                if (shift == null)
                {
                    return NotFoundError<WorkShift>($"Không tìm thấy ca làm việc hiện tại cho người dùng {userId}");
                }

                return Result<WorkShift>.Ok(shift, "Đã tải thông tin ca làm việc hiện tại");
            }
            catch (Exception ex)
            {
                return HandleException<WorkShift>(ex, "lấy ca làm việc hiện tại");
            }
        }

        public async Task<Result<IEnumerable<WorkShift>>> GetByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (userId <= 0)
                {
                    return ValidationError<IEnumerable<WorkShift>>(new List<string> { "ID người dùng không hợp lệ" });
                }

                if (startDate > endDate)
                {
                    return ValidationError<IEnumerable<WorkShift>>(new List<string> { "Ngày bắt đầu không được lớn hơn ngày kết thúc" });
                }

                var shifts = await _unitOfWork.WorkShifts.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
                return Result<IEnumerable<WorkShift>>.Ok(shifts, $"Đã tải {shifts.Count()} ca làm việc");
            }
            catch (Exception ex)
            {
                return HandleException<IEnumerable<WorkShift>>(ex, "lấy ca làm việc theo khoảng thời gian");
            }
        }

        public async Task<Result<int>> CreateAsync(WorkShift workShift)
        {
            try
            {
                var errors = new List<string>();

                if (workShift.UserId <= 0)
                    errors.Add("ID người dùng không hợp lệ");

                if (workShift.ShiftDate == default(DateTime))
                    errors.Add("Ngày ca làm việc không hợp lệ");

                if (errors.Any())
                {
                    return ValidationError<int>(errors);
                }

                // Kiểm tra xem đã có ca làm việc trong ngày chưa
                var exists = await _unitOfWork.WorkShifts.ExistsByUserIdAndDateAsync(workShift.UserId, workShift.ShiftDate);
                if (exists)
                {
                    return BusinessError<int>("Người dùng đã có ca làm việc trong ngày này");
                }

                var shiftId = await _unitOfWork.WorkShifts.AddAsync(workShift);
                _logger.Info($"Đã tạo ca làm việc mới - ShiftId: {shiftId}, UserId: {workShift.UserId}, Date: {workShift.ShiftDate:dd/MM/yyyy}");

                return Result<int>.Ok(shiftId, "Đã tạo ca làm việc thành công");
            }
            catch (Exception ex)
            {
                return HandleException<int>(ex, "tạo ca làm việc");
            }
        }

        public async Task<Result> UpdateAsync(WorkShift workShift)
        {
            try
            {
                if (workShift.ShiftId <= 0)
                {
                    return ValidationError(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                var existing = await _unitOfWork.WorkShifts.GetByIdAsync(workShift.ShiftId);
                if (existing == null)
                {
                    return NotFoundError("ca làm việc", workShift.ShiftId);
                }

                var updated = await _unitOfWork.WorkShifts.UpdateAsync(workShift);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật ca làm việc");
                }

                _logger.Info($"Đã cập nhật ca làm việc - ShiftId: {workShift.ShiftId}");
                return Result.Ok("Đã cập nhật ca làm việc thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật ca làm việc");
            }
        }

        public async Task<Result> UpdateStartTimeAsync(int shiftId, TimeSpan startTime)
        {
            try
            {
                if (shiftId <= 0)
                {
                    return ValidationError(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                var updated = await _unitOfWork.WorkShifts.UpdateStartTimeAsync(shiftId, startTime);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật thời gian bắt đầu ca");
                }

                _logger.Info($"Đã cập nhật thời gian bắt đầu ca - ShiftId: {shiftId}, StartTime: {startTime}");
                return Result.Ok("Đã cập nhật thời gian bắt đầu ca thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật thời gian bắt đầu ca");
            }
        }

        public async Task<Result> UpdateEndTimeAsync(int shiftId, TimeSpan endTime)
        {
            try
            {
                if (shiftId <= 0)
                {
                    return ValidationError(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                var updated = await _unitOfWork.WorkShifts.UpdateEndTimeAsync(shiftId, endTime);
                if (!updated)
                {
                    return BusinessError("Không thể cập nhật thời gian kết thúc ca");
                }

                _logger.Info($"Đã cập nhật thời gian kết thúc ca - ShiftId: {shiftId}, EndTime: {endTime}");
                return Result.Ok("Đã cập nhật thời gian kết thúc ca thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "cập nhật thời gian kết thúc ca");
            }
        }

        public async Task<Result<decimal>> CalculateKPIAsync(int shiftId, int ordersHandled, int targetOrders = 20)
        {
            try
            {
                if (shiftId <= 0)
                {
                    return ValidationError<decimal>(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                if (targetOrders <= 0)
                {
                    targetOrders = 20; // Mặc định 20 đơn/ca
                }

                // Tính KPI: (số đơn xử lý / mục tiêu) * 100, tối đa 100%
                decimal kpiScore = Math.Min(100, (decimal)ordersHandled / targetOrders * 100);

                // Cập nhật KPI vào ca làm việc
                var shift = await _unitOfWork.WorkShifts.GetByIdAsync(shiftId);
                if (shift != null)
                {
                    shift.KpiScore = kpiScore;
                    await _unitOfWork.WorkShifts.UpdateAsync(shift);
                }

                _logger.Info($"Đã tính KPI cho ca làm việc - ShiftId: {shiftId}, OrdersHandled: {ordersHandled}, KPI: {kpiScore:F2}%");
                return Result<decimal>.Ok(kpiScore, $"KPI: {kpiScore:F2}%");
            }
            catch (Exception ex)
            {
                return HandleException<decimal>(ex, "tính KPI cho ca làm việc");
            }
        }

        public async Task<Result> DeleteAsync(int shiftId)
        {
            try
            {
                if (shiftId <= 0)
                {
                    return ValidationError(new List<string> { "ID ca làm việc không hợp lệ" });
                }

                var existing = await _unitOfWork.WorkShifts.GetByIdAsync(shiftId);
                if (existing == null)
                {
                    return NotFoundError("ca làm việc", shiftId);
                }

                var deleted = await _unitOfWork.WorkShifts.DeleteAsync(shiftId);
                if (!deleted)
                {
                    return BusinessError("Không thể xóa ca làm việc");
                }

                _logger.Info($"Đã xóa ca làm việc - ShiftId: {shiftId}");
                return Result.Ok("Đã xóa ca làm việc thành công");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "xóa ca làm việc");
            }
        }
    }
}

