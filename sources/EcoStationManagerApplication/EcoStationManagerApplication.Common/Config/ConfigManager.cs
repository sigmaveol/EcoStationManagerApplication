using EcoStationManagerApplication.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EcoStationManagerApplication.Common.Config
{
    public static class ConfigManager
    {
        private static AppSettings _appSettings;
        private static readonly object _lock = new object();
        private static readonly string _configFilePath;

        static ConfigManager()
        {
            // Tìm file config trong các thư mục có thể
            _configFilePath = FindConfigFile();
            LoadConfiguration();
        }

        /// <summary>
        /// Tìm file config trong thư mục hiện tại hoặc parent directories
        /// </summary>
        private static string FindConfigFile()
        {
            var possiblePaths = new[]
            {
                "appsettings.json",
                "config/appsettings.json",
                "../appsettings.json",
                "../../appsettings.json",
                "EcoStationManagerApplication.Common/appsettings.json"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    return Path.GetFullPath(path);
                }
            }

            // Nếu không tìm thấy, tạo file mặc định
            var defaultPath = "appsettings.json";
            CreateDefaultConfig(defaultPath);
            return Path.GetFullPath(defaultPath);
        }

        /// <summary>
        /// Tạo file config mặc định nếu không tồn tại
        /// </summary>
        private static void CreateDefaultConfig(string filePath)
        {
            var defaultConfig = new AppSettings
            {
                Database = new DatabaseConfig
                {
                    Server = "localhost",
                    Database = "EcoStationManager",
                    UserId = "root",
                    Password = "",
                    Port = 3306,
                    ConnectionTimeout = 30,
                    CommandTimeout = 120,
                    AllowZeroDateTime = false,
                    ConvertZeroDateTime = true,
                    MaxPoolSize = 100,
                    MinPoolSize = 1
                },
                Application = new ApplicationConfig
                {
                    Name = "EcoStation Manager",
                    Version = "1.0.0",
                    Environment = "Development",
                    SessionTimeout = 30,
                    MaxLoginAttempts = 5,
                    Theme = "Light"
                },
                Logging = new LoggingConfig
                {
                    LogLevel = "Information",
                    LogPath = "logs",
                    MaxFileSize = 10,
                    EnableConsole = true,
                    EnableFile = true
                },
                UI = new UIConfig
                {
                    PrimaryColor = "#2C3E50",
                    SecondaryColor = "#3498DB",
                    SuccessColor = "#27AE60",
                    WarningColor = "#F39C12",
                    DangerColor = "#E74C3C",
                    FontFamily = "Segoe UI",
                    FontSize = 9,
                    EnableAnimations = true,
                    GunaTheme = "Light"
                }
            };

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var json = JsonHelper.Serialize(defaultConfig);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Load cấu hình từ file
        /// </summary>
        private static void LoadConfiguration()
        {
            lock (_lock)
            {
                try
                {
                    if (!File.Exists(_configFilePath))
                    {
                        CreateDefaultConfig(_configFilePath);
                    }

                    var json = File.ReadAllText(_configFilePath);
                    _appSettings = JsonHelper.Deserialize<AppSettings>(json) ?? new AppSettings();

                    // Đảm bảo thư mục log tồn tại
                    if (!string.IsNullOrEmpty(_appSettings.Logging?.LogPath))
                    {
                        Directory.CreateDirectory(_appSettings.Logging.LogPath);
                    }
                }
                catch (Exception ex)
                {
                    _appSettings = new AppSettings();
                }
            }
        }

        /// <summary>
        /// Lấy connection string từ cấu hình
        /// </summary>
        public static string GetConnectionString()
        {
            var dbConfig = _appSettings.Database;

            return $"Server={dbConfig.Server};" +
                   $"Database={dbConfig.Database};" +
                   $"Uid={dbConfig.UserId};" +
                   $"Pwd={dbConfig.Password};" +
                   $"Port={dbConfig.Port};" +
                   $"Connection Timeout={dbConfig.ConnectionTimeout};" +
                   $"Allow Zero Datetime={dbConfig.AllowZeroDateTime};" +
                   $"Convert Zero Datetime={dbConfig.ConvertZeroDateTime};" +
                   $"Max Pool Size={dbConfig.MaxPoolSize};" +
                   $"Min Pool Size={dbConfig.MinPoolSize};" +
                   $"Charset=utf8mb4;";
        }

        /// <summary>
        /// Lấy toàn bộ cấu hình
        /// </summary>
        public static AppSettings GetAppSettings() => _appSettings;

        /// <summary>
        /// Lấy cấu hình database
        /// </summary>
        public static DatabaseConfig GetDatabaseConfig() => _appSettings.Database;

        /// <summary>
        /// Lấy cấu hình ứng dụng
        /// </summary>
        public static ApplicationConfig GetApplicationConfig() => _appSettings.Application;

        /// <summary>
        /// Lấy cấu hình logging
        /// </summary>
        public static LoggingConfig GetLoggingConfig() => _appSettings.Logging;

        /// <summary>
        /// Làm mới cấu hình từ file
        /// </summary>
        public static UIConfig GetUIConfig() => _appSettings.UI;

        /// <summary>
        /// Lấy đường dẫn file config
        /// </summary>
        public static void ReloadConfiguration() => LoadConfiguration();
        public static string GetConfigFilePath() => _configFilePath;

        /// <summary>
        /// Kiểm tra xem có phải môi trường development không
        /// </summary>
        public static bool IsDevelopment()
        {
            return _appSettings.Application.Environment?.ToLower() == "development";
        }

        /// <summary>
        /// Kiểm tra xem có phải môi trường production không
        /// </summary>
        public static bool IsProduction()
        {
            return _appSettings.Application.Environment?.ToLower() == "production";
        }
    }
}
