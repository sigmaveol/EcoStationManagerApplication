using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using ClosedXML.Excel;

// Note: PDF export sử dụng PDFsharp
// Excel export sử dụng ClosedXML

namespace EcoStationManagerApplication.UI.Common
{
    /// <summary>
    /// Configuration class for report export options
    /// </summary>
    public class ReportExportOptions
    {
        /// <summary>
        /// Report title (required)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Optional subtitle or description
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Optional date range - from date
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Optional date range - to date
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Custom header text (appears above table)
        /// </summary>
        public string CustomHeader { get; set; }

        /// <summary>
        /// Custom footer text (appears below table)
        /// </summary>
        public string CustomFooter { get; set; }

        /// <summary>
        /// Column display names mapping (ColumnName -> DisplayName)
        /// </summary>
        public Dictionary<string, string> ColumnDisplayNames { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Columns to exclude from export
        /// </summary>
        public HashSet<string> ExcludedColumns { get; set; } = new HashSet<string>();

        /// <summary>
        /// Columns to include (if empty, all columns are included)
        /// </summary>
        public HashSet<string> IncludedColumns { get; set; } = new HashSet<string>();

        /// <summary>
        /// Column format strings (ColumnName -> FormatString)
        /// </summary>
        public Dictionary<string, string> ColumnFormats { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Page orientation (true = landscape, false = portrait)
        /// </summary>
        public bool Landscape { get; set; } = false;

        /// <summary>
        /// Show grid lines in PDF/Excel
        /// </summary>
        public bool ShowGridLines { get; set; } = true;

        /// <summary>
        /// Auto-fit columns in Excel
        /// </summary>
        public bool AutoFitColumns { get; set; } = true;

        /// <summary>
        /// Worksheet name for Excel export
        /// </summary>
        public string WorksheetName { get; set; } = "Báo cáo";

        /// <summary>
        /// Font name for PDF
        /// </summary>
        public string FontName { get; set; } = "Arial";

        /// <summary>
        /// Title font size
        /// </summary>
        public double TitleFontSize { get; set; } = 14;

        /// <summary>
        /// Regular font size
        /// </summary>
        public double FontSize { get; set; } = 10;

        /// <summary>
        /// Page margins (left, top, right, bottom)
        /// </summary>
        public Margins Margins { get; set; } = new Margins(40, 40, 40, 40);
    }

    /// <summary>
    /// Page margins structure
    /// </summary>
    public class Margins
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }

        public Margins(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margins(double all) : this(all, all, all, all) { }
    }

    /// <summary>
    /// Universal helper class for report generation and export
    /// Supports multiple formats: PDF (using PDFsharp), Excel (using ClosedXML), CSV
    /// Works with DataTable and IEnumerable<T>
    /// </summary>
    public static class FastReportHelper
    {
        #region Public Export Methods

        /// <summary>
        /// Export DataTable to PDF using PDFsharp
        /// </summary>
        public static void ExportToPdf(DataTable data, string outputPath, ReportExportOptions options = null)
        {
            ValidateExportInputs(data, outputPath);
            options = options ?? CreateDefaultOptions();

            ExportToPdfDirect(data, outputPath, options);
        }

        /// <summary>
        /// Export DataTable to PDF (backward compatibility)
        /// </summary>
        public static void ExportToPdf(DataTable data, string outputPath, string reportTitle, DateTime fromDate, DateTime toDate)
        {
            var options = new ReportExportOptions
            {
                Title = reportTitle,
                FromDate = fromDate,
                ToDate = toDate
            };
            ExportToPdf(data, outputPath, options);
        }

        /// <summary>
        /// Export DataTable to Excel with flexible options
        /// </summary>
        public static void ExportToExcel(DataTable data, string outputPath, ReportExportOptions options = null)
        {
            ValidateExportInputs(data, outputPath);
            options = options ?? CreateDefaultOptions();

            ExportToExcelDirect(data, outputPath, options);
        }

        /// <summary>
        /// Export DataTable to Excel (backward compatibility)
        /// </summary>
        public static void ExportToExcel(DataTable data, string outputPath, string reportTitle, DateTime fromDate, DateTime toDate)
        {
            var options = new ReportExportOptions
            {
                Title = reportTitle,
                FromDate = fromDate,
                ToDate = toDate
            };
            ExportToExcel(data, outputPath, options);
        }

        /// <summary>
        /// Export DataTable to CSV
        /// </summary>
        public static void ExportToCsv(DataTable data, string outputPath, ReportExportOptions options = null)
        {
            ValidateExportInputs(data, outputPath);
            options = options ?? CreateDefaultOptions();

            var filteredColumns = GetFilteredColumns(data, options);
            var lines = new List<string>();

            // Add title if provided
            if (!string.IsNullOrEmpty(options.Title))
            {
                lines.Add(options.Title);
            }

            // Add date range if provided
            if (options.FromDate.HasValue && options.ToDate.HasValue)
            {
                lines.Add($"Từ ngày: {options.FromDate.Value:dd/MM/yyyy} - Đến ngày: {options.ToDate.Value:dd/MM/yyyy}");
            }

            // Add empty line
            if (lines.Count > 0)
            {
                lines.Add(string.Empty);
            }

            // Headers
            var headers = filteredColumns.Select(col => 
                options.ColumnDisplayNames.ContainsKey(col.ColumnName) 
                    ? options.ColumnDisplayNames[col.ColumnName] 
                    : col.ColumnName);
            lines.Add(string.Join(",", headers.Select(h => EscapeCsvValue(h))));

            // Data rows
            foreach (DataRow row in data.Rows)
            {
                var values = filteredColumns.Select(col =>
                {
                    var value = row[col];
                    if (value == null || value == DBNull.Value)
                        return string.Empty;

                    // Apply format if available
                    if (options.ColumnFormats.ContainsKey(col.ColumnName))
                    {
                        try
                        {
                            if (value is DateTime dt)
                                return dt.ToString(options.ColumnFormats[col.ColumnName]);
                            if (value is IFormattable formattable)
                                return formattable.ToString(options.ColumnFormats[col.ColumnName], null);
                        }
                        catch { }
                    }

                    return value.ToString();
                });
                lines.Add(string.Join(",", values.Select(v => EscapeCsvValue(v))));
            }

            File.WriteAllLines(outputPath, lines);
        }

        /// <summary>
        /// Export IEnumerable<T> to PDF
        /// </summary>
        public static void ExportToPdf<T>(IEnumerable<T> data, string outputPath, ReportExportOptions options = null)
        {
            var dataTable = ConvertToDataTable(data);
            ExportToPdf(dataTable, outputPath, options);
        }

        /// <summary>
        /// Export IEnumerable<T> to Excel
        /// </summary>
        public static void ExportToExcel<T>(IEnumerable<T> data, string outputPath, ReportExportOptions options = null)
        {
            var dataTable = ConvertToDataTable(data);
            ExportToExcel(dataTable, outputPath, options);
        }

        #endregion

        #region Private Implementation Methods

        private static void ValidateExportInputs(DataTable data, string outputPath)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "DataTable cannot be null");

            if (data.Rows.Count == 0)
                throw new InvalidOperationException("No data to export");

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));
        }

        private static ReportExportOptions CreateDefaultOptions()
        {
            return new ReportExportOptions
            {
                Title = "Báo cáo",
                WorksheetName = "Báo cáo"
            };
        }

        private static void ExportToPdfDirect(DataTable data, string outputPath, ReportExportOptions options)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                if (options.Landscape)
                {
                    page.Orientation = PdfSharp.PageOrientation.Landscape;
                }

                var gfx = XGraphics.FromPdfPage(page);
                var titleFont = new XFont(options.FontName, options.TitleFontSize, XFontStyle.Bold);
                var fontRegular = new XFont(options.FontName, options.FontSize);
                var fontBold = new XFont(options.FontName, options.FontSize, XFontStyle.Bold);

                double yPos = options.Margins.Top;
                double lineHeight = options.FontSize + 4;
                var filteredColumns = GetFilteredColumns(data, options);

                // Title
                if (!string.IsNullOrEmpty(options.Title))
                {
                    gfx.DrawString(options.Title, titleFont, XBrushes.Black,
                        new XRect(options.Margins.Left, yPos, page.Width - options.Margins.Left - options.Margins.Right, lineHeight),
                        XStringFormats.TopLeft);
                    yPos += lineHeight * 1.5;
                }

                // Subtitle
                if (!string.IsNullOrEmpty(options.Subtitle))
                {
                    gfx.DrawString(options.Subtitle, fontRegular, XBrushes.Black,
                        new XRect(options.Margins.Left, yPos, page.Width - options.Margins.Left - options.Margins.Right, lineHeight),
                        XStringFormats.TopLeft);
                    yPos += lineHeight * 1.2;
                }

                // Date range
                if (options.FromDate.HasValue && options.ToDate.HasValue)
                {
                    gfx.DrawString($"Từ ngày: {options.FromDate.Value:dd/MM/yyyy} - Đến ngày: {options.ToDate.Value:dd/MM/yyyy}",
                        fontRegular, XBrushes.Black,
                        new XRect(options.Margins.Left, yPos, page.Width - options.Margins.Left - options.Margins.Right, lineHeight),
                        XStringFormats.TopLeft);
                    yPos += lineHeight * 1.2;
                }

                // Custom header
                if (!string.IsNullOrEmpty(options.CustomHeader))
                {
                    yPos += lineHeight * 0.5;
                    gfx.DrawString(options.CustomHeader, fontRegular, XBrushes.Black,
                        new XRect(options.Margins.Left, yPos, page.Width - options.Margins.Left - options.Margins.Right, lineHeight),
                        XStringFormats.TopLeft);
                    yPos += lineHeight * 1.2;
                }

                yPos += lineHeight * 0.5;

                // Table headers
                double availableWidth = page.Width - options.Margins.Left - options.Margins.Right;
                double colWidth = availableWidth / filteredColumns.Count;
                double xPos = options.Margins.Left;

                foreach (DataColumn column in filteredColumns)
                {
                    var headerText = options.ColumnDisplayNames.ContainsKey(column.ColumnName)
                        ? options.ColumnDisplayNames[column.ColumnName]
                        : column.ColumnName;

                    if (options.ShowGridLines)
                    {
                        gfx.DrawRectangle(XPens.Black, new XRect(xPos, yPos, colWidth, lineHeight));
                    }
                    gfx.DrawString(headerText, fontBold, XBrushes.Black,
                        new XRect(xPos + 5, yPos, colWidth - 10, lineHeight),
                        XStringFormats.TopLeft);
                    xPos += colWidth;
                }
                yPos += lineHeight;

                // Table data
                foreach (DataRow row in data.Rows)
                {
                    xPos = options.Margins.Left;
                    double maxRowHeight = lineHeight;

                    // Calculate max height for this row
                    foreach (DataColumn column in filteredColumns)
                    {
                        var cellValue = row[column];
                        var cellText = FormatCellValue(cellValue, column.ColumnName, options);
                        if (!string.IsNullOrEmpty(cellText))
                        {
                            var textRect = new XRect(xPos + 5, yPos, colWidth - 10, double.MaxValue);
                            var size = gfx.MeasureString(cellText, fontRegular, textRect, XStringFormats.TopLeft);
                            maxRowHeight = Math.Max(maxRowHeight, size.Height + 5);
                        }
                    }

                    // Draw cells
                    xPos = options.Margins.Left;
                    foreach (DataColumn column in filteredColumns)
                    {
                        var cellValue = row[column];
                        var cellText = FormatCellValue(cellValue, column.ColumnName, options);

                        if (options.ShowGridLines)
                        {
                            gfx.DrawRectangle(XPens.Black, new XRect(xPos, yPos, colWidth, maxRowHeight));
                        }

                        if (!string.IsNullOrEmpty(cellText))
                        {
                            gfx.DrawString(cellText, fontRegular, XBrushes.Black,
                                new XRect(xPos + 5, yPos + 2, colWidth - 10, maxRowHeight - 4),
                                XStringFormats.TopLeft);
                        }
                        xPos += colWidth;
                    }
                    yPos += maxRowHeight;

                    // New page if needed
                    if (yPos > page.Height - options.Margins.Bottom - lineHeight)
                    {
                        page = document.AddPage();
                        if (options.Landscape)
                        {
                            page.Orientation = PdfSharp.PageOrientation.Landscape;
                        }
                        gfx = XGraphics.FromPdfPage(page);
                        yPos = options.Margins.Top;
                    }
                }

                // Custom footer
                if (!string.IsNullOrEmpty(options.CustomFooter))
                {
                    yPos += lineHeight;
                    gfx.DrawString(options.CustomFooter, fontRegular, XBrushes.Black,
                        new XRect(options.Margins.Left, yPos, page.Width - options.Margins.Left - options.Margins.Right, lineHeight),
                        XStringFormats.TopLeft);
                }

                document.Save(outputPath);
            }
        }

        private static void ExportToExcelDirect(DataTable data, string outputPath, ReportExportOptions options)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(options.WorksheetName);
                var filteredColumns = GetFilteredColumns(data, options);
                int currentRow = 1;

                // Title
                if (!string.IsNullOrEmpty(options.Title))
                {
                    worksheet.Cell(currentRow, 1).Value = options.Title;
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 1).Style.Font.FontSize = options.TitleFontSize;
                    worksheet.Range(currentRow, 1, currentRow, filteredColumns.Count).Merge();
                    currentRow++;
                }

                // Subtitle
                if (!string.IsNullOrEmpty(options.Subtitle))
                {
                    worksheet.Cell(currentRow, 1).Value = options.Subtitle;
                    worksheet.Range(currentRow, 1, currentRow, filteredColumns.Count).Merge();
                    currentRow++;
                }

                // Date range
                if (options.FromDate.HasValue && options.ToDate.HasValue)
                {
                    worksheet.Cell(currentRow, 1).Value = $"Từ ngày: {options.FromDate.Value:dd/MM/yyyy} - Đến ngày: {options.ToDate.Value:dd/MM/yyyy}";
                    worksheet.Range(currentRow, 1, currentRow, filteredColumns.Count).Merge();
                    currentRow++;
                }

                // Custom header
                if (!string.IsNullOrEmpty(options.CustomHeader))
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = options.CustomHeader;
                    worksheet.Range(currentRow, 1, currentRow, filteredColumns.Count).Merge();
                    currentRow++;
                }

                currentRow++;

                // Headers
                int col = 1;
                foreach (DataColumn column in filteredColumns)
                {
                    var headerText = options.ColumnDisplayNames.ContainsKey(column.ColumnName)
                        ? options.ColumnDisplayNames[column.ColumnName]
                        : column.ColumnName;

                    worksheet.Cell(currentRow, col).Value = headerText;
                    worksheet.Cell(currentRow, col).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, col).Style.Fill.BackgroundColor = XLColor.LightGray;
                    if (options.ShowGridLines)
                    {
                        worksheet.Cell(currentRow, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                    col++;
                }
                currentRow++;

                // Data
                foreach (DataRow dataRow in data.Rows)
                {
                    col = 1;
                    foreach (DataColumn column in filteredColumns)
                    {
                        var cellValue = dataRow[column];
                        var cell = worksheet.Cell(currentRow, col);

                        if (cellValue == null || cellValue == DBNull.Value)
                        {
                            cell.Value = string.Empty;
                        }
                        else
                        {
                            // Apply formatting
                            FormatExcelCell(cell, cellValue, column.ColumnName, options);
                        }

                        if (options.ShowGridLines)
                        {
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                        col++;
                    }
                    currentRow++;
                }

                // Custom footer
                if (!string.IsNullOrEmpty(options.CustomFooter))
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = options.CustomFooter;
                    worksheet.Range(currentRow, 1, currentRow, filteredColumns.Count).Merge();
                }

                // Auto-fit columns
                if (options.AutoFitColumns)
                {
                    worksheet.Columns().AdjustToContents();
                }
                else
                {
                    worksheet.Columns().Width = 15;
                }

                workbook.SaveAs(outputPath);
            }
        }

        private static List<DataColumn> GetFilteredColumns(DataTable data, ReportExportOptions options)
        {
            var columns = data.Columns.Cast<DataColumn>().ToList();

            // Filter by included/excluded columns
            if (options.IncludedColumns.Count > 0)
            {
                columns = columns.Where(c => options.IncludedColumns.Contains(c.ColumnName)).ToList();
            }
            else if (options.ExcludedColumns.Count > 0)
            {
                columns = columns.Where(c => !options.ExcludedColumns.Contains(c.ColumnName)).ToList();
            }

            return columns;
        }

        private static string FormatCellValue(object value, string columnName, ReportExportOptions options)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            // Apply custom format if available
            if (options.ColumnFormats.ContainsKey(columnName))
            {
                try
                {
                    if (value is DateTime dt)
                        return dt.ToString(options.ColumnFormats[columnName]);
                    if (value is IFormattable formattable)
                        return formattable.ToString(options.ColumnFormats[columnName], null);
                }
                catch { }
            }

            // Default formatting based on type
            if (value is DateTime dateValue)
                return dateValue.ToString("dd/MM/yyyy");
            if (value is decimal || value is double || value is float)
                return ((IFormattable)value).ToString("#,##0.00", null);

            return value.ToString();
        }

        private static void FormatExcelCell(IXLCell cell, object value, string columnName, ReportExportOptions options)
        {
            // Apply custom format if available
            if (options.ColumnFormats.ContainsKey(columnName))
            {
                try
                {
                    cell.Style.NumberFormat.Format = options.ColumnFormats[columnName];
                }
                catch { }
            }

            // Default formatting based on type
            if (value is DateTime dateValue)
            {
                cell.Value = dateValue;
                if (!options.ColumnFormats.ContainsKey(columnName))
                {
                    cell.Style.DateFormat.Format = "dd/MM/yyyy";
                }
            }
            else if (value is decimal || value is double || value is float)
            {
                cell.Value = Convert.ToDouble(value);
                if (!options.ColumnFormats.ContainsKey(columnName))
                {
                    cell.Style.NumberFormat.Format = "#,##0.00";
                }
            }
            else
            {
                cell.Value = value.ToString();
            }
        }

        private static string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        private static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            var dataTable = new DataTable();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, columnType);
            }

            foreach (var item in data)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    row[prop.Name] = value ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        #endregion
    }
}

