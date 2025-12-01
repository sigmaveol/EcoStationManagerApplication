using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.DAL.SqlQueries
{
    public static class StationQueries
    {
        public const string GetById = "SELECT * FROM Stations WHERE station_id = @StationId";
        public const string GetAll = "SELECT * FROM Stations ORDER BY name";
        public const string GetActive = "SELECT * FROM Stations WHERE is_active = TRUE ORDER BY name";
        public const string Add = @"
            INSERT INTO Stations (name, address, phone, station_type, manager, is_active, created_date, updated_date)
            VALUES (@Name, @Address, @Phone, @StationType, @Manager, @IsActive, @CreatedDate, @UpdatedDate);
            SELECT LAST_INSERT_ID();";
        public const string Update = @"
            UPDATE Stations
            SET name = @Name, address = @Address, phone = @Phone, station_type = @StationType,
                manager = @Manager, is_active = @IsActive, updated_date = @UpdatedDate
            WHERE station_id = @StationId;";
        public const string Delete = "DELETE FROM Stations WHERE station_id = @StationId";
        public const string Exists = "SELECT COUNT(1) FROM Stations WHERE station_id = @StationId";

        public const string GetByManager = @"
            SELECT * FROM Stations
            WHERE manager = @ManagerId AND is_active = TRUE
            ORDER BY name";

        public const string GetByName = @"
            SELECT * FROM Stations
            WHERE name = @Name";

        public const string Search = @"
            SELECT * FROM Stations
            WHERE (name LIKE @Keyword OR address LIKE @Keyword OR phone LIKE @Keyword)
            ORDER BY name";

        public const string ToggleStatus = @"
            UPDATE Stations
            SET is_active = @IsActive, updated_date = @UpdatedDate
            WHERE station_id = @StationId";
    }
}

