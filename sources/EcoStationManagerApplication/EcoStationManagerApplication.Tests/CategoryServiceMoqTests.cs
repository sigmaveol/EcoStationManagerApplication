using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CategoryServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<ICategoryRepository> _catRepo;
        private CategoryService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _catRepo = new Mock<ICategoryRepository>();
            _uow.Setup(u => u.Categories).Returns(_catRepo.Object);
            _svc = new CategoryService(_uow.Object);
        }

        [TestMethod]
        public async Task DeleteCategory_WhenHasProducts_Fails()
        {
            var cid = 1;
            _catRepo.Setup(r => r.GetByIdAsync(cid)).ReturnsAsync(new Category { CategoryId = cid });
            _catRepo.Setup(r => r.CountProductsInCategoryAsync(cid)).ReturnsAsync(2);

            var res = await _svc.DeleteCategoryAsync(cid);
            Assert.IsFalse(res.Success);
            _catRepo.Verify(r => r.ToggleActiveAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteCategory_NoProducts_Ok()
        {
            var cid = 2;
            _catRepo.Setup(r => r.GetByIdAsync(cid)).ReturnsAsync(new Category { CategoryId = cid });
            _catRepo.Setup(r => r.CountProductsInCategoryAsync(cid)).ReturnsAsync(0);
            _catRepo.Setup(r => r.ToggleActiveAsync(cid, false)).ReturnsAsync(true);

            var res = await _svc.DeleteCategoryAsync(cid);
            Assert.IsTrue(res.Success);
            _catRepo.Verify(r => r.ToggleActiveAsync(cid, false), Times.Once);
        }

        [TestMethod]
        public async Task ToggleCategoryStatus_NotFound_Fails()
        {
            var cid = 3;
            _catRepo.Setup(r => r.GetByIdAsync(cid)).ReturnsAsync((Category)null);
            var res = await _svc.ToggleCategoryStatusAsync(cid, true);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ToggleCategoryStatus_Valid_Ok()
        {
            var cid = 4;
            _catRepo.Setup(r => r.GetByIdAsync(cid)).ReturnsAsync(new Category { CategoryId = cid });
            _catRepo.Setup(r => r.ToggleActiveAsync(cid, true)).ReturnsAsync(true);
            var res = await _svc.ToggleCategoryStatusAsync(cid, true);
            Assert.IsTrue(res.Success);
            _catRepo.Verify(r => r.ToggleActiveAsync(cid, true), Times.Once);
        }
    }
}
