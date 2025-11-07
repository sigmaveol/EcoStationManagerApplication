using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EcoStationManagerApplication.Common.Helpers
{
    public static class JsonHelper
    {
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public static T Deserialize<T>(string json)
        {
            try
            {
                return _serializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi parse JSON: {ex.Message}", ex);
            }
        }

        public static string Serialize<T>(T obj)
        {
            try
            {
                return _serializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi serialize JSON: {ex.Message}", ex);
            }
        }

        public static T LoadFromFile<T>(string filePath) where T : new()
        {
            try
            {
                if (!File.Exists(filePath))
                    return new T();

                var json = File.ReadAllText(filePath);
                return Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi đọc file JSON {filePath}: {ex.Message}");
                return new T();
            }
        }

        public static void SaveToFile<T>(string filePath, T obj)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = Serialize(obj);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi ghi file JSON {filePath}: {ex.Message}");
                throw;
            }
        }
    }
}
