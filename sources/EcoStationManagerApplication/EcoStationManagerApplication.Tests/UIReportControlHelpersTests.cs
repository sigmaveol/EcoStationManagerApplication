using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class UIReportControlHelpersTests
    {
        [TestMethod]
        public void GetOrderSourceName_Returns_String()
        {
            var s = EcoStationManagerApplication.UI.Controls.ReportControlHelpers.GetOrderSourceName(EcoStationManagerApplication.Models.Enums.OrderSource.EXCEL);
            Assert.IsFalse(string.IsNullOrEmpty(s));
        }
    }
}
