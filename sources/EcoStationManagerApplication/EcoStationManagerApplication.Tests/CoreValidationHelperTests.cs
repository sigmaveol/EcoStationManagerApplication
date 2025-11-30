using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Models.Entities;
using System.Linq;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CoreValidationHelperTests
    {
        [TestMethod]
        public void ValidateOrder_Fails_When_Note_Too_Long()
        {
            var order = new Order { Note = new string('x', 2000) };
            var errors = ValidationHelper.ValidateOrder(order);
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ValidatePrice_Fails_When_Negative()
        {
            var errors = ValidationHelper.ValidatePrice(-1m);
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ValidateOrderQuantity_Fails_When_Exceeds_Available()
        {
            var errors = ValidationHelper.ValidateOrderQuantity(10m, 5m);
            Assert.IsTrue(errors.Any());
        }
    }
}
