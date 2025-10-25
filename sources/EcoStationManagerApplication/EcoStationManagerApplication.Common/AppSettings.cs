using System;
using System.Configuration;

namespace EcoStationManagerApplication.Common
{
    public class AppSettings
    {
        public static string DatabaseConnectionString =>
            ConfigurationManager.ConnectionStrings["EcoStationDB"]?.ConnectionString ??
            throw new InvalidOperationException("Database connection string not found");

        public static string ApplicationName => "EcoStation Manager";
        public static string Version => "1.0.0";

        // Google Services Settings
        public static string GoogleClientId =>
            ConfigurationManager.AppSettings["GoogleClientId"] ?? "";

        public static string GoogleClientSecret =>
            ConfigurationManager.AppSettings["GoogleClientSecret"] ?? "";

        public static string GoogleSpreadsheetId =>
            ConfigurationManager.AppSettings["GoogleSpreadsheetId"] ?? "";

        public static string GoogleDriveFolderId =>
            ConfigurationManager.AppSettings["GoogleDriveFolderId"] ?? "";

        public static string GoogleMapsApiKey =>
            ConfigurationManager.AppSettings["GoogleMapsApiKey"] ?? "";

        // Application Settings
        public static int CommandTimeout =>
            int.Parse(ConfigurationManager.AppSettings["CommandTimeout"] ?? "30");

        public static bool EnableLogging =>
            bool.Parse(ConfigurationManager.AppSettings["EnableLogging"] ?? "true");

        public static string LogLevel =>
            ConfigurationManager.AppSettings["LogLevel"] ?? "Information";

        public static int SessionTimeout =>
            int.Parse(ConfigurationManager.AppSettings["SessionTimeout"] ?? "30");

        // Business Settings
        public static decimal DefaultTaxRate =>
            decimal.Parse(ConfigurationManager.AppSettings["DefaultTaxRate"] ?? "0.1");

        public static int LowStockThreshold =>
            int.Parse(ConfigurationManager.AppSettings["LowStockThreshold"] ?? "10");

        public static int ExpiryWarningDays =>
            int.Parse(ConfigurationManager.AppSettings["ExpiryWarningDays"] ?? "15");

        public static bool AutoBackup =>
            bool.Parse(ConfigurationManager.AppSettings["AutoBackup"] ?? "true");

        public static int BackupIntervalHours =>
            int.Parse(ConfigurationManager.AppSettings["BackupIntervalHours"] ?? "24");
    }
}
