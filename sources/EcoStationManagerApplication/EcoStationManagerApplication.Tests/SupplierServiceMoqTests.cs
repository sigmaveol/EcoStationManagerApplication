using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class SupplierServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<ISupplierRepository> _repo;
        private SupplierService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<ISupplierRepository>();
            _uow.Setup(u => u.Suppliers).Returns(_repo.Object);
            _svc = new SupplierService(_uow.Object);
        }

        [TestMethod]
        public async Task GetByContact_Empty_Fail()
        {
            var res = await _svc.GetSuppliersByContactPersonAsync("");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task GetByContact_Success()
        {
            _repo.Setup(r => r.GetByContactPersonAsync("A")).ReturnsAsync(new[] { new Supplier { SupplierId = 1, ContactPerson = "A" } });
            var res = await _svc.GetSuppliersByContactPersonAsync("A");
            Assert.IsTrue(res.Success);
            Assert.AreEqual(1, res.Data.Count);
        }
    }
}

