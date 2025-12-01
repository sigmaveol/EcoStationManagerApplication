using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace EcoStationManagerApplication.Common.Exporters
{
    /// <summary>
    /// PDF Exporter chỉ dùng PdfSharp 1.50 (không dùng MigraDoc)
    /// </summary>
    public class PdfExporter : IPdfExporter
    {
        private const int ItemsPerPage = 25;
        private const double LeftMargin = 40;
        private const double RightMargin = 40;
        private const double LineHeight = 18;
        private const double HeaderHeight = 22;
        private const double TableTopMargin = 100;

        private static bool _fontResolverInitialized = false;
        private static readonly object _lock = new object();

        static PdfExporter()
        {
            InitializeFontResolver();
        }

        private static void InitializeFontResolver()
        {
            lock (_lock)
            {
                if (!_fontResolverInitialized)
                {
                    try
                    {
            if (GlobalFontSettings.FontResolver == null)
                            GlobalFontSettings.FontResolver = new WindowsFontResolver();
                        _fontResolverInitialized = true;
                    }
                    catch (Exception ex)
            {
                        System.Diagnostics.Debug.WriteLine($"Font resolver error: {ex.Message}");
                        _fontResolverInitialized = true;
                    }
                }
            }
        }

        public void ExportToPdf<T>(IEnumerable<T> data, string filePath, string title = "Danh sách", Dictionary<string, string> headers = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required", nameof(filePath));

            title = string.IsNullOrWhiteSpace(title) ? "Danh sách" : title;
            var dataList = data.ToList();

            if (!dataList.Any()) throw new InvalidOperationException("No data to export");

            var properties = typeof(T).GetProperties()
                .Where(p => p != null && p.CanRead && !string.IsNullOrWhiteSpace(p.Name) && !IsComplexType(p.PropertyType))
                .ToList();

            if (!properties.Any()) throw new InvalidOperationException("No valid properties found");

            // Tạo document và tính toán trước
            using (var document = new PdfDocument())
            {
                document.Info.Title = title;
                document.Info.Author = "EcoStation Manager";

                int totalPages = (int)Math.Ceiling((double)dataList.Count / ItemsPerPage);
                if (totalPages == 0) totalPages = 1;

                int itemIndex = 0;

                for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                {
            var page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        var fonts = CreateFonts();
                        double totalWidth = page.Width - LeftMargin - RightMargin;
                        double[] colWidths = CalculateColumnWidths(properties.Count, totalWidth);

                        double yPos = 40;

                        // Chỉ vẽ title ở trang đầu
                        if (pageNum == 1)
                        {
                            DrawTitle(gfx, page, title, fonts.Title, ref yPos);
                            DrawExportInfo(gfx, dataList.Count, fonts.Normal, ref yPos);
                        }

                        // Table header
                        yPos = pageNum == 1 ? TableTopMargin : 40;
                        DrawTableHeader(gfx, properties, headers, colWidths, yPos, totalWidth, fonts.Header);
                        yPos += HeaderHeight;

                        // Data rows cho trang này
                        int itemsOnThisPage = 0;
                        while (itemIndex < dataList.Count && itemsOnThisPage < ItemsPerPage)
                        {
                            var item = dataList[itemIndex];
                            if (item != null)
                            {
                                DrawDataRow(gfx, item, properties, colWidths, yPos, totalWidth, itemIndex, fonts.Normal, page);
                                yPos += LineHeight;
                            }
                            itemIndex++;
                            itemsOnThisPage++;
                        }

                        // Footer
                        DrawFooter(gfx, page, pageNum, totalPages, fonts.Small);
                    }
                }

                EnsureDirectoryExists(filePath);
                document.Save(filePath);
            }
        }

        public void ExportToPdf(DataTable dataTable, string filePath, string title = "Danh sách", Dictionary<string, string> headers = null)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required", nameof(filePath));
            if (dataTable.Rows.Count == 0) throw new InvalidOperationException("No data to export");

            title = string.IsNullOrWhiteSpace(title) ? "Danh sách" : title;

            using (var document = new PdfDocument())
            {
                document.Info.Title = title;

                int totalPages = (int)Math.Ceiling((double)dataTable.Rows.Count / ItemsPerPage);
                if (totalPages == 0) totalPages = 1;

                int rowIndex = 0;

                for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                {
                    var page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        var fonts = CreateFonts();
                        double totalWidth = page.Width - LeftMargin - RightMargin;
                        double[] colWidths = CalculateColumnWidths(dataTable.Columns.Count, totalWidth);

                        double yPos = 40;

                        if (pageNum == 1)
                        {
                            DrawTitle(gfx, page, title, fonts.Title, ref yPos);
                            DrawExportInfo(gfx, dataTable.Rows.Count, fonts.Normal, ref yPos);
                        }

                        yPos = pageNum == 1 ? TableTopMargin : 40;
                        DrawDataTableHeader(gfx, dataTable, headers, colWidths, yPos, totalWidth, fonts.Header);
                        yPos += HeaderHeight;

                        int itemsOnThisPage = 0;
                        while (rowIndex < dataTable.Rows.Count && itemsOnThisPage < ItemsPerPage)
                        {
                            var row = dataTable.Rows[rowIndex];
                            DrawDataTableRow(gfx, row, dataTable, colWidths, yPos, totalWidth, rowIndex, fonts.Normal, page);
                            yPos += LineHeight;
                            rowIndex++;
                            itemsOnThisPage++;
                        }

                        DrawFooter(gfx, page, pageNum, totalPages, fonts.Small);
                    }
                }

                EnsureDirectoryExists(filePath);
                document.Save(filePath);
            }
        }

        public void ExportToPdf(DataTable dataTable, string filePath, string title, Dictionary<string, string> headers, byte[] chartImageBytes)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required", nameof(filePath));
            if (dataTable.Rows.Count == 0) throw new InvalidOperationException("No data to export");

            title = string.IsNullOrWhiteSpace(title) ? "Danh sách" : title;

            using (var document = new PdfDocument())
            {
                document.Info.Title = title;

                int totalPages = (int)Math.Ceiling((double)dataTable.Rows.Count / ItemsPerPage);
                if (totalPages == 0) totalPages = 1;

                int rowIndex = 0;

                for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                {
                    var page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        var fonts = CreateFonts();
                        double totalWidth = page.Width - LeftMargin - RightMargin;
                        double[] colWidths = CalculateColumnWidths(dataTable.Columns.Count, totalWidth);

                        double yPos = 40;

                        if (pageNum == 1)
                        {
                            DrawTitle(gfx, page, title, fonts.Title, ref yPos);
                            DrawExportInfo(gfx, dataTable.Rows.Count, fonts.Normal, ref yPos);

                            if (chartImageBytes != null && chartImageBytes.Length > 0)
                            {
                                using (var ms = new MemoryStream(chartImageBytes))
                                {
                                    var img = XImage.FromStream(ms);
                                    double maxWidth = totalWidth;
                                    double scale = img.PixelWidth > 0 ? Math.Min(maxWidth / img.PixelWidth, 1.0) : 1.0;
                                    double imgWidth = img.PixelWidth * scale;
                                    double imgHeight = img.PixelHeight * scale;
                                    gfx.DrawImage(img, LeftMargin, yPos + 10, imgWidth, imgHeight);
                                    yPos += imgHeight + 20;
                                }
                            }
                        }

                        yPos = pageNum == 1 ? Math.Max(yPos, TableTopMargin) : 40;
                        DrawDataTableHeader(gfx, dataTable, headers, colWidths, yPos, totalWidth, fonts.Header);
                        yPos += HeaderHeight;

                        int itemsOnThisPage = 0;
                        while (rowIndex < dataTable.Rows.Count && itemsOnThisPage < ItemsPerPage)
                        {
                            var row = dataTable.Rows[rowIndex];
                            DrawDataTableRow(gfx, row, dataTable, colWidths, yPos, totalWidth, rowIndex, fonts.Normal, page);
                            yPos += LineHeight;
                            rowIndex++;
                            itemsOnThisPage++;
                        }

                        DrawFooter(gfx, page, pageNum, totalPages, fonts.Small);
                    }
                }

                EnsureDirectoryExists(filePath);
                document.Save(filePath);
            }
        }

        public void ExportToPdf(DataTable dataTable, string filePath, string title, Dictionary<string, string> headers, IList<byte[]> chartImages)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required", nameof(filePath));
            if (dataTable.Rows.Count == 0) throw new InvalidOperationException("No data to export");

            title = string.IsNullOrWhiteSpace(title) ? "Danh sách" : title;

            using (var document = new PdfDocument())
            {
                document.Info.Title = title;

                int totalPages = (int)Math.Ceiling((double)dataTable.Rows.Count / ItemsPerPage);
                if (totalPages == 0) totalPages = 1;

                int rowIndex = 0;

                for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                {
                    var page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        var fonts = CreateFonts();
                        double totalWidth = page.Width - LeftMargin - RightMargin;
                        double[] colWidths = CalculateColumnWidths(dataTable.Columns.Count, totalWidth);

                        double yPos = 40;

                        if (pageNum == 1)
                        {
                            DrawTitle(gfx, page, title, fonts.Title, ref yPos);
                            DrawExportInfo(gfx, dataTable.Rows.Count, fonts.Normal, ref yPos);

                            if (chartImages != null)
                            {
                                foreach (var bytes in chartImages)
                                {
                                    if (bytes == null || bytes.Length == 0) continue;
                                    using (var ms = new MemoryStream(bytes))
                                    {
                                        var img = XImage.FromStream(ms);
                                        double maxWidth = totalWidth;
                                        double scale = img.PixelWidth > 0 ? Math.Min(maxWidth / img.PixelWidth, 1.0) : 1.0;
                                        double imgWidth = img.PixelWidth * scale;
                                        double imgHeight = img.PixelHeight * scale;
                                        gfx.DrawImage(img, LeftMargin, yPos + 10, imgWidth, imgHeight);
                                        yPos += imgHeight + 20;
                                    }
                                }
                            }
                        }

                        yPos = pageNum == 1 ? Math.Max(yPos, TableTopMargin) : 40;
                        DrawDataTableHeader(gfx, dataTable, headers, colWidths, yPos, totalWidth, fonts.Header);
                        yPos += HeaderHeight;

                        int itemsOnThisPage = 0;
                        while (rowIndex < dataTable.Rows.Count && itemsOnThisPage < ItemsPerPage)
                        {
                            var row = dataTable.Rows[rowIndex];
                            DrawDataTableRow(gfx, row, dataTable, colWidths, yPos, totalWidth, rowIndex, fonts.Normal, page);
                            yPos += LineHeight;
                            rowIndex++;
                            itemsOnThisPage++;
                        }

                        DrawFooter(gfx, page, pageNum, totalPages, fonts.Small);
                    }
                }

                EnsureDirectoryExists(filePath);
                document.Save(filePath);
            }
        }

        public void ExportMultipleSections(Dictionary<string, DataTable> sections, string filePath, Dictionary<string, Dictionary<string, string>> headersBySection = null, Dictionary<string, string> titlesBySection = null, Dictionary<string, IList<byte[]>> chartsBySection = null)
        {
            if (sections == null) throw new ArgumentNullException(nameof(sections));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path is required", nameof(filePath));

            using (var document = new PdfDocument())
            {
                document.Info.Title = "Báo cáo tổng hợp";

                foreach (var kv in sections)
                {
                    var sectionName = string.IsNullOrWhiteSpace(kv.Key) ? "SECTION" : kv.Key;
                    var dt = kv.Value ?? new DataTable();
                    var headers = headersBySection != null && headersBySection.ContainsKey(sectionName) ? headersBySection[sectionName] : null;
                    var title = titlesBySection != null && titlesBySection.ContainsKey(sectionName) ? titlesBySection[sectionName] : sectionName.ToUpper();
                    var charts = chartsBySection != null && chartsBySection.ContainsKey(sectionName) ? chartsBySection[sectionName] : null;

                    int totalRecords = dt.Rows.Count;
                    int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalRecords / ItemsPerPage));
                    int rowIndex = 0;

                    for (int pageNum = 1; pageNum <= totalPages; pageNum++)
                    {
                        var page = document.AddPage();
                        page.Size = PdfSharp.PageSize.A4;

                        using (var gfx = XGraphics.FromPdfPage(page))
                        {
                            var fonts = CreateFonts();
                            double totalWidth = page.Width - LeftMargin - RightMargin;
                            int colCount = Math.Max(1, dt.Columns.Count);
                            double[] colWidths = CalculateColumnWidths(colCount, totalWidth);

                            double yPos = 40;

                            if (pageNum == 1)
                            {
                                DrawTitle(gfx, page, title, fonts.Title, ref yPos);
                                DrawExportInfo(gfx, totalRecords, fonts.Normal, ref yPos);
                                if (charts != null)
                                {
                                    foreach (var bytes in charts)
                                    {
                                        if (bytes == null || bytes.Length == 0) continue;
                                        using (var ms = new MemoryStream(bytes))
                                        {
                                            var img = XImage.FromStream(ms);
                                            double maxWidth = totalWidth;
                                            double scale = img.PixelWidth > 0 ? Math.Min(maxWidth / img.PixelWidth, 1.0) : 1.0;
                                            double imgWidth = img.PixelWidth * scale;
                                            double imgHeight = img.PixelHeight * scale;
                                            gfx.DrawImage(img, LeftMargin, yPos + 10, imgWidth, imgHeight);
                                            yPos += imgHeight + 20;
                                        }
                                    }
                                }
                            }

                            yPos = pageNum == 1 ? Math.Max(yPos, TableTopMargin) : 40;

                            if (dt.Columns.Count > 0)
                            {
                                DrawDataTableHeader(gfx, dt, headers, colWidths, yPos, totalWidth, fonts.Header);
                                yPos += HeaderHeight;

                                int itemsOnThisPage = 0;
                                while (rowIndex < dt.Rows.Count && itemsOnThisPage < ItemsPerPage)
                                {
                                    var row = dt.Rows[rowIndex];
                                    DrawDataTableRow(gfx, row, dt, colWidths, yPos, totalWidth, rowIndex, fonts.Normal, page);
                                    yPos += LineHeight;
                                    rowIndex++;
                                    itemsOnThisPage++;
                                }
                            }

                            DrawFooter(gfx, page, pageNum, totalPages, fonts.Small);
                        }
                    }
                }

                EnsureDirectoryExists(filePath);
                document.Save(filePath);
            }
        }

        #region Drawing Methods

        private void DrawTitle(XGraphics gfx, PdfPage page, string title, XFont font, ref double yPos)
        {
            var lines = (title ?? string.Empty).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) lines = new[] { string.Empty };
            foreach (var line in lines)
            {
                var text = (line ?? string.Empty).ToUpper();
                gfx.DrawString(text, font, XBrushes.Black,
                    new XRect(LeftMargin, yPos, page.Width - LeftMargin - RightMargin, 30),
                    XStringFormats.TopCenter);
                yPos += 24;
            }
        }

        private void DrawExportInfo(XGraphics gfx, int recordCount, XFont font, ref double yPos)
        {
            gfx.DrawString($"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", font, XBrushes.Black,
                new XPoint(LeftMargin, yPos));
            yPos += 15;
            gfx.DrawString($"Tổng số bản ghi: {recordCount}", font, XBrushes.Black,
                new XPoint(LeftMargin, yPos));
        }

        private void DrawTableHeader(XGraphics gfx, List<System.Reflection.PropertyInfo> properties,
            Dictionary<string, string> headers, double[] colWidths, double yPos, double totalWidth, XFont font)
        {
            // Background
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(46, 125, 50)),
                new XRect(LeftMargin, yPos, totalWidth, HeaderHeight));

            double xPos = LeftMargin;
                    for (int i = 0; i < properties.Count; i++)
                    {
                string headerText = GetHeaderText(properties[i].Name, headers);
                gfx.DrawString(headerText, font, XBrushes.White,
                    new XRect(xPos + 3, yPos + 3, colWidths[i] - 6, HeaderHeight - 6),
                    XStringFormats.TopLeft);
                xPos += colWidths[i];
            }
        }

        private void DrawDataTableHeader(XGraphics gfx, DataTable dataTable,
            Dictionary<string, string> headers, double[] colWidths, double yPos, double totalWidth, XFont font)
        {
            gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(46, 125, 50)),
                new XRect(LeftMargin, yPos, totalWidth, HeaderHeight));

            double xPos = LeftMargin;
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string headerText = GetHeaderText(dataTable.Columns[i].ColumnName, headers);
                gfx.DrawString(headerText, font, XBrushes.White,
                    new XRect(xPos + 3, yPos + 3, colWidths[i] - 6, HeaderHeight - 6),
                            XStringFormats.TopLeft);
                xPos += colWidths[i];
            }
        }

        private void DrawDataRow<T>(XGraphics gfx, T item, List<System.Reflection.PropertyInfo> properties,
            double[] colWidths, double yPos, double totalWidth, int rowIndex, XFont font, PdfPage page)
        {
            // Alternate row color
            if (rowIndex % 2 == 0)
                {
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 245, 245)),
                        new XRect(LeftMargin, yPos, totalWidth, LineHeight));
                }

            double xPos = LeftMargin;
                for (int i = 0; i < properties.Count; i++)
                {
                string value = GetPropertyValue(item, properties[i]);
                gfx.DrawString(value, font, XBrushes.Black,
                    new XRect(xPos + 3, yPos + 2, colWidths[i] - 6, LineHeight - 4),
                                XStringFormats.TopLeft);
                xPos += colWidths[i];
            }

            // Bottom border
            gfx.DrawLine(XPens.LightGray, LeftMargin, yPos + LineHeight, page.Width - RightMargin, yPos + LineHeight);
        }

        private void DrawDataTableRow(XGraphics gfx, DataRow row, DataTable dataTable,
            double[] colWidths, double yPos, double totalWidth, int rowIndex, XFont font, PdfPage page)
        {
            if (rowIndex % 2 == 0)
            {
                gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 245, 245)),
                    new XRect(LeftMargin, yPos, totalWidth, LineHeight));
            }

            double xPos = LeftMargin;
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var col = dataTable.Columns[i];
                string value = FormatValue(row[col], col.DataType);
                value = TruncateText(value, 22);

                gfx.DrawString(value, font, XBrushes.Black,
                    new XRect(xPos + 3, yPos + 2, colWidths[i] - 6, LineHeight - 4),
                    XStringFormats.TopLeft);
                xPos += colWidths[i];
            }

            gfx.DrawLine(XPens.LightGray, LeftMargin, yPos + LineHeight, page.Width - RightMargin, yPos + LineHeight);
        }

        private void DrawFooter(XGraphics gfx, PdfPage page, int currentPage, int totalPages, XFont font)
        {
            string footerText = $"Trang {currentPage} / {totalPages}";
            gfx.DrawString(footerText, font, XBrushes.Gray,
                new XRect(LeftMargin, page.Height - 30, page.Width - LeftMargin - RightMargin, 20),
                XStringFormats.BottomCenter);
        }

        #endregion

        #region Helper Methods

        private FontSet CreateFonts()
        {
            return new FontSet
            {
                Title = new XFont("Times New Roman", 16, XFontStyle.Bold),
                Header = new XFont("Times New Roman", 9, XFontStyle.Bold),
                Normal = new XFont("Times New Roman", 8),
                Small = new XFont("Times New Roman", 7)
            };
        }

        private class FontSet
        {
            public XFont Title { get; set; }
            public XFont Header { get; set; }
            public XFont Normal { get; set; }
            public XFont Small { get; set; }
        }

        private double[] CalculateColumnWidths(int columnCount, double totalWidth)
        {
            if (columnCount <= 0) return new double[0];
            double[] widths = new double[columnCount];
            double width = totalWidth / columnCount;
            for (int i = 0; i < columnCount; i++)
                widths[i] = width;
            return widths;
        }

        private string GetHeaderText(string propName, Dictionary<string, string> headers)
        {
            if (string.IsNullOrWhiteSpace(propName)) return "Column";
            if (headers != null && headers.TryGetValue(propName, out var text) && !string.IsNullOrWhiteSpace(text))
                return text;
            return propName;
        }

        private string GetPropertyValue<T>(T item, System.Reflection.PropertyInfo prop)
        {
            try
            {
                var value = prop.GetValue(item);
                string result = FormatValue(value, prop.PropertyType);
                return TruncateText(result, 22);
            }
            catch { return string.Empty; }
        }

        private string FormatValue(object value, Type type)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            try
            {
                if (type == typeof(decimal) || type == typeof(decimal?))
                    return Convert.ToDecimal(value).ToString("N0");
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                    return Convert.ToDateTime(value).ToString("dd/MM/yyyy");
                if (type == typeof(int) || type == typeof(int?))
                    return Convert.ToInt32(value).ToString();
                if (type == typeof(double) || type == typeof(double?))
                    return Convert.ToDouble(value).ToString("N2");
                return value.ToString() ?? string.Empty;
            }
            catch { return value?.ToString() ?? string.Empty; }
        }

        private string TruncateText(string text, int max)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= max ? text : text.Substring(0, max - 3) + "...";
        }

        private bool IsComplexType(Type type)
        {
            if (type == null) return true;
            if (type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTime?)) return false;
            if (type.IsPrimitive || type == typeof(decimal) || type == typeof(decimal?)) return false;
            if (type == typeof(double) || type == typeof(double?) || type == typeof(float) || type == typeof(float?)) return false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return IsComplexType(Nullable.GetUnderlyingType(type));
            return type.IsClass && type != typeof(string);
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        #endregion
    }
}
