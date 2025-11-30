using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Common.Utilities;
using System;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CommonFormatHelperTests
    {
        [TestMethod]
        public void FormatCurrency_Returns_Dash_When_NonPositive()
        {
            var s1 = FormatHelper.FormatCurrency(0m);
            var s2 = FormatHelper.FormatCurrency(-1m);
            Assert.AreEqual("-", s1);
            Assert.AreEqual("-", s2);
        }

        [TestMethod]
        public void FormatCurrency_Returns_Number_When_Positive()
        {
            var s = FormatHelper.FormatCurrency(12345m);
            Assert.IsTrue(s.Contains("12"));
        }

        [TestMethod]
        public void FormatCurrencyWithUnit_Appends_VND()
        {
            var s = FormatHelper.FormatCurrencyWithUnit(1000m);
            Assert.IsTrue(s.EndsWith(" VNĐ"));
        }

        [TestMethod]
        public void FormatQuantity_Returns_Two_Decimals()
        {
            var s = FormatHelper.FormatQuantity(1.5m);
            Assert.IsTrue(s.Contains("1"));
        }

        [TestMethod]
        public void FormatDate_Returns_DDMMYYYY()
        {
            var dt = new DateTime(2024, 12, 31);
            var s = FormatHelper.FormatDate(dt);
            Assert.AreEqual("31/12/2024", s);
        }

        [TestMethod]
        public void FormatDateTime_Returns_DDMMYYYY_HHMM()
        {
            var dt = new DateTime(2024, 12, 31, 23, 45, 0);
            var s = FormatHelper.FormatDateTime(dt);
            Assert.AreEqual("31/12/2024 23:45", s);
        }
    }
}
