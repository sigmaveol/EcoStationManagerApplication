using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Common.Helpers;

namespace EcoStationManagerApplication.Tests
{
    public class AB
    {
        public int A { get; set; }
        public string B { get; set; }
    }

    [TestClass]
    public class CommonJsonHelperTests
    {
        [TestMethod]
        public void Serialize_Deserialize_RoundTrip()
        {
            var obj = new AB { A = 7, B = "x" };
            var json = JsonHelper.Serialize(obj);
            var back = JsonHelper.Deserialize<AB>(json);
            Assert.AreEqual(7, back.A);
            Assert.AreEqual("x", back.B);
        }
    }
}
