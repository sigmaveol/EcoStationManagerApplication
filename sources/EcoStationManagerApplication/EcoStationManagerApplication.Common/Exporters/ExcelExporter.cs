using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// Implementation của IExcelExporter - Sử dụng ClosedXML
    /// </summary>
    public class ExcelExporter : IExcelExporter
    {
        public void ExportToExcel<T>(IEnumerable<T> data, string filePath, string worksheetName = "Sheet1", Dictionary<string, string> headers = null)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            var dataList = data.ToList();
            if (!dataList.Any())
                throw new InvalidOperationException("No data to export");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(worksheetName);

                // Lấy properties của T
                var properties = typeof(T).GetProperties()
                    .Where(p => p.CanRead && !IsComplexType(p.PropertyType))
                    .ToList();

                if (!properties.Any())
                    throw new InvalidOperationException("No valid properties found to export");

                int currentRow = 1;

                // Tiêu đề tài liệu (nếu có)
                worksheet.Cell(currentRow, 1).Value = worksheetName.ToUpper();
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range(currentRow, 1, currentRow, properties.Count).Merge();
                currentRow++;

                // Thông tin xuất
                worksheet.Cell(currentRow, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = $"Tổng số bản ghi: {dataList.Count}";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                // Header row
                int headerRow = currentRow;
                for (int i = 0; i < properties.Count; i++)
                {
                    var prop = properties[i];
                    string headerText = headers != null && headers.ContainsKey(prop.Name)
                        ? headers[prop.Name]
                        : prop.Name;

                    worksheet.Cell(headerRow, i + 1).Value = headerText;
                }

                // Format header
                var headerRange = worksheet.Range(headerRow, 1, headerRow, properties.Count);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(46, 125, 50);
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Data rows
                int dataRow = headerRow + 1;
                foreach (var item in dataList)
                {
                    for (int i = 0; i < properties.Count; i++)
                    {
                        var prop = properties[i];
                        var value = prop.GetValue(item);

                        var cell = worksheet.Cell(dataRow, i + 1);
                        
                        if (value != null)
                        {
                            // Format theo kiểu dữ liệu
                            if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                            {
                                cell.Value = Convert.ToDecimal(value);
                                cell.Style.NumberFormat.Format = "#,##0";
                            }
                            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                            {
                                cell.Value = Convert.ToDateTime(value);
                                cell.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                            }
                            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                            {
                                cell.Value = Convert.ToInt32(value);
                            }
                            else
                            {
                                cell.Value = value.ToString();
                            }
                        }
                    }
                    dataRow++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Thêm border cho toàn bộ dữ liệu
                var dataRange = worksheet.Range(headerRow, 1, dataRow - 1, properties.Count);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                workbook.SaveAs(filePath);
            }
        }

        private bool IsComplexType(Type type)
        {
            if (type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTime?))
                return false;

            if (type.IsPrimitive || type == typeof(decimal) || type == typeof(decimal?))
                return false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return IsComplexType(underlyingType);
            }

            return type.IsClass && type != typeof(string);
        }

        /// <summary>
        /// Export DataTable to Excel
        /// </summary>
        public void ExportToExcel(DataTable dataTable, string filePath, string worksheetName = "Sheet1", Dictionary<string, string> headers = null)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            if (dataTable.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(worksheetName);

                int currentRow = 1;

                // Tiêu đề tài liệu
                worksheet.Cell(currentRow, 1).Value = worksheetName.ToUpper();
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 16;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range(currentRow, 1, currentRow, dataTable.Columns.Count).Merge();
                currentRow++;

                // Thông tin xuất
                worksheet.Cell(currentRow, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = $"Tổng số bản ghi: {dataTable.Rows.Count}";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                // Header row
                int headerRow = currentRow;
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var column = dataTable.Columns[i];
                    string headerText = headers != null && headers.ContainsKey(column.ColumnName)
                        ? headers[column.ColumnName]
                        : column.ColumnName;

                    worksheet.Cell(headerRow, i + 1).Value = headerText;
                }

                // Format header
                var headerRange = worksheet.Range(headerRow, 1, headerRow, dataTable.Columns.Count);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(46, 125, 50);
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Data rows
                int dataRow = headerRow + 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        var column = dataTable.Columns[i];
                        var value = row[column];
                        var cell = worksheet.Cell(dataRow, i + 1);

                        if (value != null && value != DBNull.Value)
                        {
                            // Format theo kiểu dữ liệu
                            if (column.DataType == typeof(decimal) || column.DataType == typeof(decimal?))
                            {
                                cell.Value = Convert.ToDecimal(value);
                                cell.Style.NumberFormat.Format = "#,##0";
                            }
                            else if (column.DataType == typeof(DateTime) || column.DataType == typeof(DateTime?))
                            {
                                cell.Value = Convert.ToDateTime(value);
                                cell.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                            }
                            else if (column.DataType == typeof(int) || column.DataType == typeof(int?))
                            {
                                cell.Value = Convert.ToInt32(value);
                            }
                            else
                            {
                                cell.Value = value.ToString();
                            }
                        }
                    }
                    dataRow++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Thêm border cho toàn bộ dữ liệu
                var dataRange = worksheet.Range(headerRow, 1, dataRow - 1, dataTable.Columns.Count);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                workbook.SaveAs(filePath);
            }
        }
    }
}

