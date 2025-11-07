using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Common.Logging
{
    public interface ILogHelper
    {
        void Info(string message);
        void Error(string message);
        void Warning(string message);
        void Debug(string message);
    }

    public class LogHelper : ILogHelper
    {
        private readonly string _category;
        private readonly string _logPath;
        private readonly bool _enableFileLog;

        public LogHelper(string category)
        {
            _category = category;
            _logPath = Config.ConfigManager.GetLoggingConfig()?.LogPath ?? "logs";
            _enableFileLog = Config.ConfigManager.GetLoggingConfig()?.EnableFile ?? true;

            // Đảm bảo thư mục log tồn tại
            if (_enableFileLog && !Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        public void Error(string message)
        {
            WriteLog("ERROR", message);
        }

        public void Warning(string message)
        {
            WriteLog("WARN", message);
        }

        public void Debug(string message)
        {
            if (Config.ConfigManager.IsDevelopment())
            {
                WriteLog("DEBUG", message);
            }
        }

        private void WriteLog(string level, string message)
        {
            var logMessage = $"[{level}] [{_category}] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";

            // Log ra console
            if (Config.ConfigManager.GetLoggingConfig()?.EnableConsole ?? true)
            {
                Console.WriteLine(logMessage);
            }

            // Log ra file
            if (_enableFileLog)
            {
                try
                {
                    var logFile = Path.Combine(_logPath, $"log_{DateTime.Now:yyyyMMdd}.txt");
                    File.AppendAllText(logFile, logMessage + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Không thể ghi log file: {ex.Message}");
                }
            }
        }
    }

    public static class LogHelperFactory
    {
        public static ILogHelper CreateLogger(string category)
        {
            return new LogHelper(category);
        }
    }
}
