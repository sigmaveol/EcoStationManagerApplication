using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class ProductServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IProductRepository> _prodRepo;
        private Mock<ICategoryService> _catSvc;
        private ProductService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _prodRepo = new Mock<IProductRepository>();
            _catSvc = new Mock<ICategoryService>();
            _uow.Setup(u => u.Products).Returns(_prodRepo.Object);
            _svc = new ProductService(_uow.Object, _catSvc.Object);
        }

        [TestMethod]
        public async Task ToggleProductStatus_NotFound_Fails()
        {
            var id = 1;
            _prodRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);
            var res = await _svc.ToggleProductStatusAsync(id, true);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ToggleProductStatus_Valid_Ok()
        {
            var id = 2;
            _prodRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Product { ProductId = id });
            _prodRepo.Setup(r => r.ToggleActiveAsync(id, true)).ReturnsAsync(true);
            var res = await _svc.ToggleProductStatusAsync(id, true);
            Assert.IsTrue(res.Success);
            _prodRepo.Verify(r => r.ToggleActiveAsync(id, true), Times.Once);
        }

        [TestMethod]
        public async Task DeleteProduct_Valid_Ok()
        {
            var id = 3;
            _prodRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new Product { ProductId = id });
            _prodRepo.Setup(r => r.ToggleActiveAsync(id, false)).ReturnsAsync(true);
            var res = await _svc.DeleteProductAsync(id);
            Assert.IsTrue(res.Success);
            _prodRepo.Verify(r => r.ToggleActiveAsync(id, false), Times.Once);
        }

        [TestMethod]
        public async Task IsSkuExists_Empty_ReturnsFalse()
        {
            var res = await _svc.IsSkuExistsAsync("");
            Assert.IsFalse(res.Data);
        }
    }
}
