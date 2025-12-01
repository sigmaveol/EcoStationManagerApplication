using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CustomerServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<ICustomerRepository> _repo;
        private CustomerService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<ICustomerRepository>();
            _uow.Setup(u => u.Customers).Returns(_repo.Object);
            _svc = new CustomerService(_uow.Object);
        }

        [TestMethod]
        public async Task Create_DuplicatePhone_Fail()
        {
            var c = new Customer { Name = "A", CustomerCode = "C001", Phone = "123" };
            _repo.Setup(r => r.IsCodeExistsAsync("C001", null)).ReturnsAsync(false);
            _repo.Setup(r => r.IsPhoneExistsAsync("123", null)).ReturnsAsync(true);
            var res = await _svc.CreateCustomerAsync(c);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task UpdatePoints_Invalid_Fail()
        {
            var res = await _svc.UpdateCustomerPointsAsync(1, -1);
            Assert.IsFalse(res.Success);
        }
    }
}

