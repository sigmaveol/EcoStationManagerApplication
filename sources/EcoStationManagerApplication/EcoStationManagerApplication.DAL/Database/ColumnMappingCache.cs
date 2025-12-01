using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EcoStationManagerApplication.DAL.Database
{
    public static class ColumnMappingCache<T> where T : class
    {
        public static readonly IReadOnlyList<PropertyInfo> Properties;
        public static readonly IReadOnlyDictionary<string, string> PropertyToColumn;
        public static readonly PropertyInfo IdProperty;
        public static readonly string IdColumn;
        public static readonly string TableName;
        public static readonly string InsertSql;
        public static readonly string UpdateSql;

        static ColumnMappingCache()
        {
            var type = typeof(T);

            // Lấy tên bảng từ [Table] attribute hoặc convert từ tên class
            TableName = GetTableName(type);

            // Lấy danh sách properties hỗ trợ map
            Properties = type.GetProperties()
                .Where(IsMappedProperty)
                .ToList();

            // Id property
            IdProperty = ResolveIdProperty(type);
            IdColumn = GetColumnName(IdProperty);

            // Build map property → column
            PropertyToColumn = Properties
                .ToDictionary(
                    p => p.Name,
                    p => GetColumnName(p),
                    StringComparer.OrdinalIgnoreCase
                );

            InsertSql = BuildInsertSql();
            UpdateSql = BuildUpdateSql();
        }

        /// <summary>
        /// Lấy tên bảng từ [Table] attribute hoặc convert từ tên class
        /// </summary>
        private static string GetTableName(Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
            {
                return tableAttribute.Name;
            }

            // Fallback: convert từ tên class sang snake_case
            return ToSnakeCase(type.Name);
        }

    private static bool IsMappedProperty(PropertyInfo p)
    {
        if (p.GetCustomAttribute<NotMappedAttribute>() != null)
            return false;

        var t = p.PropertyType;
        if (t.IsClass && t != typeof(string) && !t.IsPrimitive && !t.IsEnum &&
            !(Nullable.GetUnderlyingType(t)?.IsPrimitive ?? false))
            return false;

        return true;
    }

    private static PropertyInfo ResolveIdProperty(Type type)
    {
        // Ưu tiên ColumnAttribute
        var prop = type.GetProperties()
            .FirstOrDefault(p =>
            {
                var col = p.GetCustomAttribute<ColumnAttribute>();
                return col != null && col.Name.EndsWith("_id", StringComparison.OrdinalIgnoreCase);
            });

        if (prop != null) return prop;

        // snake_case → PascalCase conversion
        var idName = "Id";
        prop = type.GetProperty(type.Name + "Id") ??
               type.GetProperty(idName) ??
               type.GetProperties().FirstOrDefault(p => GetColumnName(p) == "id");

        if (prop == null)
            throw new InvalidOperationException($"Cannot find ID property for {type.Name}");

        return prop;
    }

    private static string GetColumnName(PropertyInfo p)
    {
        var col = p.GetCustomAttribute<ColumnAttribute>();
        if (col != null && !string.IsNullOrEmpty(col.Name))
            return col.Name;

        return ToSnakeCase(p.Name);
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(name[0]));

        for (int i = 1; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                sb.Append('_').Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private static string BuildInsertSql()
    {
        var insertProps = Properties.Where(p => p != IdProperty).ToList();
        var cols = string.Join(", ", insertProps.Select(p => PropertyToColumn[p.Name]));
        var vals = string.Join(", ", insertProps.Select(p => "@" + PropertyToColumn[p.Name]));
        return $"INSERT INTO {TableName} ({cols}) VALUES ({vals}); SELECT LAST_INSERT_ID();";
    }

    private static string BuildUpdateSql()
    {
        var updateProps = Properties.Where(p => p != IdProperty).ToList();
        var set = string.Join(", ",
            updateProps.Select(p => $"{PropertyToColumn[p.Name]} = @{PropertyToColumn[p.Name]}"));

        return $"UPDATE {TableName} SET {set} WHERE {IdColumn} = @IdParam";
    }
    }
}
