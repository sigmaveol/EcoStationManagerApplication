using System;
using System.Text.Json;

namespace EcoStationManagerApplication.Models.Entities
{
    public class SystemSetting
    {
        public int SsId { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Navigation properties
        public User UpdatedByUser { get; set; }
    }

    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleKey { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public JsonDocument Config { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}