using System;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class CleaningScheduleQueries
    {
        public const string GetByCleaningType = @"
            SELECT * FROM CleaningSchedules 
            WHERE cleaning_type = @CleaningType
            ORDER BY cleaning_date DESC";

        public const string GetByStatus = @"
            SELECT * FROM CleaningSchedules 
            WHERE status = @Status
            ORDER BY cleaning_date DESC";

        public const string GetUpcomingInRange = @"
            SELECT * FROM CleaningSchedules 
            WHERE cleaning_date BETWEEN @FromDate AND @ToDate
            ORDER BY cleaning_date ASC";

        public const string GetOverdue = @"
            SELECT * FROM CleaningSchedules 
            WHERE cleaning_date < NOW()
              AND status IN (0, 2)
            ORDER BY cleaning_date ASC";

        public const string GetByCleaningBy = @"
            SELECT * FROM CleaningSchedules 
            WHERE cleaning_by = @CleaningBy
            ORDER BY cleaning_date DESC";

        public const string GetByDateAndType = @"
            SELECT * FROM CleaningSchedules
            WHERE DATE(cleaning_date) = DATE(@CleaningDate)
              AND cleaning_type = @CleaningType
            LIMIT 1";
    }
}


