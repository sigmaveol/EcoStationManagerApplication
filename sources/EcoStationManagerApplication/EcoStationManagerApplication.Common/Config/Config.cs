using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Common.Config
{
    public class AppSettings
    {
        public DatabaseConfig Database { get; set; }
        public ApplicationConfig Application { get; set; }
        public LoggingConfig Logging { get; set; }
        public UIConfig UI { get; set; }
    }

    public class DatabaseConfig
    {
        public string Server { get; set; } = "localhost";
        public string Database { get; set; } = "EcoStationManager";
        public string UserId { get; set; } = "root";
        public string Password { get; set; } = "123456";
        public int Port { get; set; } = 3306;
        public int ConnectionTimeout { get; set; } = 30;
        public int CommandTimeout { get; set; } = 120;
        public bool AllowZeroDateTime { get; set; } = false;
        public bool ConvertZeroDateTime { get; set; } = true;
        public int MaxPoolSize { get; set; } = 100;
        public int MinPoolSize { get; set; } = 1;
    }

    public class ApplicationConfig
    {
        public string Name { get; set; } = "EcoStation Manager";
        public string Version { get; set; } = "1.0.0";
        public string Environment { get; set; } = "Development";
        public int SessionTimeout { get; set; } = 30;
        public int MaxLoginAttempts { get; set; } = 5;
        public string Theme { get; set; } = "Light";
    }

    public class LoggingConfig
    {
        public string LogLevel { get; set; } = "Information";
        public string LogPath { get; set; } = "logs";
        public int MaxFileSize { get; set; } = 10; // MB
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = true;
    }

    public class UIConfig
    {
        public string PrimaryColor { get; set; } = "#2C3E50";
        public string SecondaryColor { get; set; } = "#3498DB";
        public string SuccessColor { get; set; } = "#27AE60";
        public string WarningColor { get; set; } = "#F39C12";
        public string DangerColor { get; set; } = "#E74C3C";
        public string FontFamily { get; set; } = "Segoe UI";
        public int FontSize { get; set; } = 9;
        public bool EnableAnimations { get; set; } = true;
        public string GunaTheme { get; set; } = "Light";
    }

}
