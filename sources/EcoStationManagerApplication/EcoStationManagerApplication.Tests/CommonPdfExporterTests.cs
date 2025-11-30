using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Common.Exporters;
using System.Data;
using System.IO;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CommonPdfExporterTests
    {
        [TestMethod]
        public void ExportMultipleSections_Creates_Pdf_File()
        {
            var table1 = new DataTable();
            table1.Columns.Add("OrderId", typeof(int));
            table1.Columns.Add("OrderCode", typeof(string));
            table1.Rows.Add(1, "ORD-001");

            var table2 = new DataTable();
            table2.Columns.Add("PackagingId", typeof(int));
            table2.Columns.Add("PackagingName", typeof(string));
            table2.Rows.Add(10, "Chai 1L");

            var sections = new System.Collections.Generic.Dictionary<string, DataTable>
            {
                { "ChiTietDonHang", table1 },
                { "TonKhoBaoBi", table2 }
            };

            var headers = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>
            {
                { "ChiTietDonHang", new System.Collections.Generic.Dictionary<string, string> { { "OrderId", "ID Đơn" }, { "OrderCode", "Mã đơn" } } },
                { "TonKhoBaoBi", new System.Collections.Generic.Dictionary<string, string> { { "PackagingId", "ID Bao bì" }, { "PackagingName", "Bao bì" } } }
            };

            var exporter = new PdfExporter();
            var temp = Path.ChangeExtension(Path.GetTempFileName(), ".pdf");

            try
            {
                exporter.ExportMultipleSections(sections, temp, headers, null, null);
                Assert.IsTrue(File.Exists(temp));
                var len = new FileInfo(temp).Length;
                Assert.IsTrue(len > 0);
            }
            finally
            {
                if (File.Exists(temp)) File.Delete(temp);
            }
        }
    }
}
