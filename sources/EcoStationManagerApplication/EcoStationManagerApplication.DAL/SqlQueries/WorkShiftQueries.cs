using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class WorkShiftQueries
    {
        // Lấy WorkShift theo user_id và ngày
        public const string GetByUserIdAndDate = @"
            SELECT * FROM WorkShifts 
            WHERE user_id = @UserId AND shift_date = @ShiftDate";

        // Lấy WorkShift hiện tại của user (hôm nay)
        public const string GetCurrentShiftByUserId = @"
            SELECT * FROM WorkShifts 
            WHERE user_id = @UserId 
            AND shift_date = CURDATE()";

        // Lấy tất cả WorkShift của user trong khoảng thời gian
        public const string GetByUserIdAndDateRange = @"
            SELECT * FROM WorkShifts 
            WHERE user_id = @UserId 
            AND shift_date BETWEEN @StartDate AND @EndDate
            ORDER BY shift_date DESC";

        // Tạo WorkShift mới
        public const string Insert = @"
            INSERT INTO WorkShifts (user_id, shift_date, start_time, end_time, kpi_score, notes)
            VALUES (@UserId, @ShiftDate, @StartTime, @EndTime, @KpiScore, @Notes);
            SELECT LAST_INSERT_ID();";

        // Cập nhật WorkShift
        public const string Update = @"
            UPDATE WorkShifts 
            SET start_time = @StartTime, 
                end_time = @EndTime, 
                kpi_score = @KpiScore, 
                notes = @Notes,
                updated_date = CURRENT_TIMESTAMP
            WHERE shift_id = @ShiftId";

        // Cập nhật start_time khi bắt đầu ca
        public const string UpdateStartTime = @"
            UPDATE WorkShifts 
            SET start_time = @StartTime,
                updated_date = CURRENT_TIMESTAMP
            WHERE shift_id = @ShiftId";

        // Cập nhật end_time khi kết thúc ca
        public const string UpdateEndTime = @"
            UPDATE WorkShifts 
            SET end_time = @EndTime,
                updated_date = CURRENT_TIMESTAMP
            WHERE shift_id = @ShiftId";

        // Kiểm tra xem user đã có ca làm việc trong ngày chưa
        public const string ExistsByUserIdAndDate = @"
            SELECT COUNT(1) FROM WorkShifts 
            WHERE user_id = @UserId AND shift_date = @ShiftDate";
    }
}

