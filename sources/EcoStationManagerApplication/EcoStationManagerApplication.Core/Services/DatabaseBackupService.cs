using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Common.Config;
using EcoStationManagerApplication.DAL.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Services
{
    public class DatabaseBackupService : IDatabaseBackupService
    {
        private readonly DatabaseHelper _dbHelper;

        public DatabaseBackupService(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task DumpToSqlFileAsync(string filePath)
        {
            ValidatePath(filePath);

            using (var conn = await _dbHelper.CreateConnectionAsync())
            {
                //await conn. ();
                var dbName = ConnectionHelper.GetDatabaseName();
                var tables = await GetTablesAsync(conn, dbName);

                using (var writer = new StreamWriter(filePath, false, new UTF8Encoding(false)))
                {
                    // Write header
                    await WriteHeaderAsync(writer);

                    // Write database creation
                    await writer.WriteLineAsync($"CREATE DATABASE IF NOT EXISTS `{EscapeIdentifier(dbName)}` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;");
                    await writer.WriteLineAsync($"USE `{EscapeIdentifier(dbName)}`;");
                    await writer.WriteLineAsync();

                    await writer.WriteLineAsync("SET FOREIGN_KEY_CHECKS=0;");

                    // Dump stored procedures first
                    await BackupStoredProceduresAsync(conn, writer, dbName);

                    // Dump table structures first
                    foreach (var table in tables)
                    {
                        await DumpTableStructureAsync(conn, writer, table);
                    }

                    // Then dump table data
                    foreach (var table in tables)
                    {
                        await DumpTableDataAsync(conn, writer, table);
                    }

                    // Dump triggers last
                    await BackupTriggersAsync(conn, writer, dbName);

                    await writer.WriteLineAsync("SET FOREIGN_KEY_CHECKS=1;");

                    // Write footer
                    await WriteFooterAsync(writer);
                }
            }
        }

        public async Task RestoreFromSqlFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            using (var conn = await _dbHelper.CreateConnectionAsync())
            {
                //await conn.OpenAsync();

                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        var dbName = ConnectionHelper.GetDatabaseName();
                        await ExecuteNonQueryAsync(conn, tx, $"USE `{EscapeIdentifier(dbName)}`;");
                        await ExecuteNonQueryAsync(conn, tx, "SET FOREIGN_KEY_CHECKS=0;");
                        await ExecuteSqlScriptAsync(conn, tx, filePath);
                        await ExecuteNonQueryAsync(conn, tx, "SET FOREIGN_KEY_CHECKS=1;");
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task WriteHeaderAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync($"-- Host: {ConnectionHelper.GetServerName()}");
            await writer.WriteLineAsync($"-- Generation Time: {DateTime.Now:MMM dd, yyyy 'at' hh:mm tt}");
            await writer.WriteLineAsync("-- Server version: 10.4.32-MariaDB");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("SET SQL_MODE = \"NO_AUTO_VALUE_ON_ZERO\";");
            await writer.WriteLineAsync("START TRANSACTION;");
            await writer.WriteLineAsync("SET time_zone = \"+00:00\";");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;");
            await writer.WriteLineAsync("/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;");
            await writer.WriteLineAsync("/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;");
            await writer.WriteLineAsync("/*!40101 SET NAMES utf8mb4 */;");
            await writer.WriteLineAsync();
        }

        private async Task WriteFooterAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;");
            await writer.WriteLineAsync("/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;");
            await writer.WriteLineAsync("/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("COMMIT;");
        }

        private static void ValidatePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Invalid path", nameof(filePath));

            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        private async Task<List<string>> GetTablesAsync(IDbConnection conn, string dbName)
        {
            var sql = @"SELECT TABLE_NAME 
                       FROM INFORMATION_SCHEMA.TABLES 
                       WHERE TABLE_SCHEMA = @db AND TABLE_TYPE = 'BASE TABLE'
                       ORDER BY TABLE_NAME";

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                AddParameter(cmd, "@db", dbName);

                var tables = new List<string>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
                return tables;
            }
        }

        private async Task DumpTableStructureAsync(IDbConnection conn, StreamWriter writer, string table)
        {
            await writer.WriteLineAsync("-- --------------------------------------------------------");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync($"-- Table structure for table `{EscapeIdentifier(table)}`");
            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SHOW CREATE TABLE `{EscapeIdentifier(table)}`";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var createTableSql = reader.GetString(1);
                        if (createTableSql.StartsWith("CREATE TABLE "))
                        {
                            createTableSql = createTableSql.Replace("CREATE TABLE ", "CREATE TABLE IF NOT EXISTS ");
                        }
                        await writer.WriteLineAsync($"{createTableSql};");
                        await writer.WriteLineAsync();
                    }
                }
            }
        }

        private async Task DumpTableDataAsync(IDbConnection conn, StreamWriter writer, string table)
        {
            var dataExists = await CheckTableHasDataAsync(conn, table);
            if (!dataExists) return;

            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync($"-- Dumping data for table `{EscapeIdentifier(table)}`");
            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync();

            var columns = await GetTableColumnsAsync(conn, table);

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT * FROM `{EscapeIdentifier(table)}`";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var values = new List<string>();
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var val = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            values.Add(ToSqlValue(val));
                        }

                        var insertSql = $"INSERT IGNORE INTO `{EscapeIdentifier(table)}` ({string.Join(", ", columns.Select(c => $"`{EscapeIdentifier(c)}`"))}) VALUES ({string.Join(", ", values)});";
                        await writer.WriteLineAsync(insertSql);
                    }
                }
            }
            await writer.WriteLineAsync();
        }

        private async Task<List<string>> GetStoredProceduresAsync(IDbConnection conn, string dbName)
        {
            var procedures = new List<string>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = @db AND ROUTINE_TYPE = 'PROCEDURE' ORDER BY ROUTINE_NAME";
                AddParameter(cmd, "@db", dbName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        procedures.Add(reader.GetString(0));
                    }
                }
            }
            return procedures;
        }

        private async Task BackupStoredProceduresAsync(IDbConnection conn, StreamWriter writer, string dbName)
        {
            var procedures = await GetStoredProceduresAsync(conn, dbName);
            if (procedures.Count == 0) return;

            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync("-- Stored Procedures");
            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("DELIMITER $$");

            foreach (var proc in procedures)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SHOW CREATE PROCEDURE `{EscapeIdentifier(proc)}`";
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var def = r.GetString(2);
                            await writer.WriteLineAsync($"DROP PROCEDURE IF EXISTS `{EscapeIdentifier(proc)}`$$");
                            await writer.WriteLineAsync(def + "$$");
                        }
                    }
                }
                await writer.WriteLineAsync();
            }

            await writer.WriteLineAsync("DELIMITER ;");
            await writer.WriteLineAsync();
        }

        private async Task<List<string>> GetTriggersAsync(IDbConnection conn, string dbName)
        {
            var triggers = new List<string>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT TRIGGER_NAME FROM INFORMATION_SCHEMA.TRIGGERS WHERE TRIGGER_SCHEMA = @db ORDER BY TRIGGER_NAME";
                AddParameter(cmd, "@db", dbName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        triggers.Add(reader.GetString(0));
                    }
                }
            }
            return triggers;
        }

        private async Task BackupTriggersAsync(IDbConnection conn, StreamWriter writer, string dbName)
        {
            var triggers = await GetTriggersAsync(conn, dbName);
            if (triggers.Count == 0) return;

            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync("-- Triggers");
            await writer.WriteLineAsync("--");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync("DELIMITER $$");

            foreach (var trg in triggers)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SHOW CREATE TRIGGER `{EscapeIdentifier(trg)}`";
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            var def = r.GetString(2);
                            await writer.WriteLineAsync($"DROP TRIGGER IF EXISTS `{EscapeIdentifier(trg)}`$$");
                            await writer.WriteLineAsync(def + "$$");
                        }
                    }
                }
                await writer.WriteLineAsync();
            }

            await writer.WriteLineAsync("DELIMITER ;");
            await writer.WriteLineAsync();
        }

        private async Task<List<string>> GetTableColumnsAsync(IDbConnection conn, string table)
        {
            var columns = new List<string>();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SHOW COLUMNS FROM `{EscapeIdentifier(table)}`";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
            }
            return columns;
        }

        private async Task<bool> CheckTableHasDataAsync(IDbConnection conn, string table)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM `{EscapeIdentifier(table)}`";
                var count = cmd.ExecuteScalar();
                return Convert.ToInt32(count) > 0;
            }
        }

        private static string ToSqlValue(object val)
        {
            if (val == null || val == DBNull.Value)
                return "NULL";

            switch (val)
            {
                case string s:
                    return "'" + EscapeString(s) + "'";
                case DateTime dt:
                    return "'" + dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'";
                case TimeSpan ts:
                    return "'" + ts.ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture) + "'";
                case bool b:
                    return b ? "1" : "0";
                case byte[] bytes:
                    return "0x" + BitConverter.ToString(bytes).Replace("-", "");
                case sbyte sb:
                    return sb.ToString(CultureInfo.InvariantCulture);
                case byte by:
                    return by.ToString(CultureInfo.InvariantCulture);
                case short sh:
                    return sh.ToString(CultureInfo.InvariantCulture);
                case ushort ush:
                    return ush.ToString(CultureInfo.InvariantCulture);
                case int i:
                    return i.ToString(CultureInfo.InvariantCulture);
                case uint ui:
                    return ui.ToString(CultureInfo.InvariantCulture);
                case long l:
                    return l.ToString(CultureInfo.InvariantCulture);
                case ulong ul:
                    return ul.ToString(CultureInfo.InvariantCulture);
                case float f32:
                    return f32.ToString(CultureInfo.InvariantCulture);
                case double f64:
                    return f64.ToString(CultureInfo.InvariantCulture);
                case decimal dec:
                    return dec.ToString(CultureInfo.InvariantCulture);
                case Guid guid:
                    return "'" + EscapeString(guid.ToString()) + "'";
                default:
                    return "'" + EscapeString(val.ToString()) + "'";
            }
        }

        private static string EscapeString(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\\", "\\\\")
                   .Replace("'", "\\'")
                   .Replace("\"", "\\\"")
                   .Replace("\n", "\\n")
                   .Replace("\r", "\\r")
                   .Replace("\t", "\\t")
                   .Replace("\0", "\\0");
        }

        private static string EscapeIdentifier(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("`", "``");
        }

        #region Database Helper Methods

        private static void AddParameter(IDbCommand cmd, string name, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }

        private static async Task ExecuteNonQueryAsync(IDbConnection conn, IDbTransaction tx, string sql)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        private static async Task ExecuteSqlScriptAsync(IDbConnection conn, IDbTransaction tx, string filePath)
        {
            using (var reader = new StreamReader(filePath, new UTF8Encoding(false)))
            {
                var sb = new StringBuilder();
                string line;
                var delimiter = ";";
                var inBlockComment = false;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var trimmedLine = line.Trim();

                    if (inBlockComment)
                    {
                        if (trimmedLine.Contains("*/")) inBlockComment = false;
                        continue;
                    }

                    if (string.IsNullOrEmpty(trimmedLine)) continue;
                    if (trimmedLine.StartsWith("--") || trimmedLine.StartsWith("#")) continue;
                    if (trimmedLine.StartsWith("/*")) { if (!trimmedLine.Contains("*/")) inBlockComment = true; continue; }

                    if (trimmedLine.StartsWith("DELIMITER", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = trimmedLine.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2)
                        {
                            delimiter = parts[1].Trim();
                        }
                        sb.Clear();
                        continue;
                    }

                    sb.AppendLine(line);

                    var endsWithDelimiter = trimmedLine.EndsWith(delimiter);
                    if (endsWithDelimiter)
                    {
                        var sql = sb.ToString().Trim();
                        if (sql.EndsWith(delimiter))
                        {
                            sql = sql.Substring(0, sql.Length - delimiter.Length).TrimEnd();
                        }
                        if (!string.IsNullOrEmpty(sql))
                        {
                            await ExecuteNonQueryAsync(conn, tx, sql);
                        }
                        sb.Clear();
                    }
                }
    
                var remainingSql = sb.ToString().Trim();
                if (!string.IsNullOrEmpty(remainingSql))
                {
                    await ExecuteNonQueryAsync(conn, tx, remainingSql);
                }
            }
        }

        #endregion
    }
}
