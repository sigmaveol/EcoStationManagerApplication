using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class ImportServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private ImportService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _svc = new ImportService(_uow.Object);
        }

        [TestMethod]
        public async Task ImportOrders_NonExistingFile_Fail()
        {
            var res = await _svc.ImportOrdersFromExcelTemplateAsync("D:/notfound.xlsx");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ImportOrders_WrongExtension_Fail()
        {
            var res = await _svc.ImportOrdersFromExcelTemplateAsync("C:/tmp/data.txt");
            Assert.IsFalse(res.Success);
        }
    }
}

