using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoStationManagerApplication.Models.Entities
{
    [Table("GoogleIntegrationConfig")]
    public class GoogleIntegrationConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("integration_id")]
        public int IntegrationId { get; set; }

        [Required]
        [Column("sheet_url")]
        public string SheetUrl { get; set; }

        [Required]
        [Column("spreadsheet_id")]
        public string SpreadsheetId { get; set; }

        [Column("sheet_name")]
        public string SheetName { get; set; }

        [Column("api_key")]
        public string ApiKey { get; set; }

        [Column("last_sync_time")]
        public DateTime? LastSyncTime { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("GoogleOrderMapping")]
    public class GoogleOrderMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("sheet_row_index")]
        public int SheetRowIndex { get; set; }

        [Required]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("synced_at")]
        public DateTime SyncedAt { get; set; } = DateTime.Now;

        [Column("config_id")]
        public int? ConfigId { get; set; }
    }
}
