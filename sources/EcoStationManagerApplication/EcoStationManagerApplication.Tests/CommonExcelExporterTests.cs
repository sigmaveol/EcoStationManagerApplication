using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Common.Exporters;
using System.Data;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CommonExcelExporterTests
    {
        [TestMethod]
        public void ExportMultipleSheets_Creates_Sheets_With_Names()
        {
            var table1 = new DataTable();
            table1.Columns.Add("OrderId", typeof(int));
            table1.Columns.Add("OrderCode", typeof(string));
            table1.Rows.Add(1, "ORD-001");

            var table2 = new DataTable();
            table2.Columns.Add("PackagingId", typeof(int));
            table2.Columns.Add("PackagingName", typeof(string));
            table2.Rows.Add(10, "Chai 1L");

            var sheets = new System.Collections.Generic.Dictionary<string, DataTable>
            {
                { "ChiTietDonHang", table1 },
                { "TonKhoBaoBi", table2 }
            };

            var headers = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>
            {
                { "ChiTietDonHang", new System.Collections.Generic.Dictionary<string, string> { { "OrderId", "ID Đơn" }, { "OrderCode", "Mã đơn" } } },
                { "TonKhoBaoBi", new System.Collections.Generic.Dictionary<string, string> { { "PackagingId", "ID Bao bì" }, { "PackagingName", "Bao bì" } } }
            };

            var exporter = new ExcelExporter();
            var temp = Path.GetTempFileName();

            try
            {
                exporter.ExportMultipleSheets(sheets, temp, headers, null, null);

                using (var doc = SpreadsheetDocument.Open(temp, false))
                {
                    var wbPart = doc.WorkbookPart;
                    var sheetNames = wbPart.Workbook.Sheets.Cast<DocumentFormat.OpenXml.Spreadsheet.Sheet>()
                        .Select(s => s.Name.Value).ToList();
                    CollectionAssert.Contains(sheetNames, "ChiTietDonHang");
                    CollectionAssert.Contains(sheetNames, "TonKhoBaoBi");
                }
            }
            finally
            {
                if (File.Exists(temp)) File.Delete(temp);
            }
        }
    }
}
