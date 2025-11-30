using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.Serialization;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class UIBackupControlHeadersTests
    {
        [TestMethod]
        public void GetOrderDetailColumnHeaders_Has_Expected_Keys()
        {
            var t = typeof(EcoStationManagerApplication.UI.Controls.BackupControl);
            var inst = FormatterServices.GetUninitializedObject(t);
            var m = t.GetMethod("GetOrderDetailColumnHeaders", BindingFlags.NonPublic | BindingFlags.Instance);
            var dict = (System.Collections.Generic.Dictionary<string, string>)m.Invoke(inst, null);
            Assert.IsTrue(dict.ContainsKey("OrderId"));
            Assert.IsTrue(dict.ContainsKey("OrderCode"));
            Assert.IsTrue(dict.ContainsKey("ProductId"));
            Assert.IsTrue(dict.ContainsKey("Quantity"));
            Assert.IsTrue(dict.ContainsKey("UnitPrice"));
        }

        [TestMethod]
        public void GetPackagingInventoryColumnHeaders_Has_Expected_Keys()
        {
            var t = typeof(EcoStationManagerApplication.UI.Controls.BackupControl);
            var inst = FormatterServices.GetUninitializedObject(t);
            var m = t.GetMethod("GetPackagingInventoryColumnHeaders", BindingFlags.NonPublic | BindingFlags.Instance);
            var dict = (System.Collections.Generic.Dictionary<string, string>)m.Invoke(inst, null);
            Assert.IsTrue(dict.ContainsKey("PackagingId"));
            Assert.IsTrue(dict.ContainsKey("PackagingName"));
            Assert.IsTrue(dict.ContainsKey("QtyNew"));
            Assert.IsTrue(dict.ContainsKey("QtyInUse"));
            Assert.IsTrue(dict.ContainsKey("LastUpdated"));
        }
    }
}
