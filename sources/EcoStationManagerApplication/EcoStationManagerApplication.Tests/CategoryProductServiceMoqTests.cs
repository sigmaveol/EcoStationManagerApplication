using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Core.Interfaces;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CategoryProductServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<ICategoryRepository> _catRepo;
        private Mock<IProductRepository> _prodRepo;
        private CategoryService _catSvc;
        private ProductService _prodSvc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _catRepo = new Mock<ICategoryRepository>();
            _prodRepo = new Mock<IProductRepository>();
            _uow.Setup(u => u.Categories).Returns(_catRepo.Object);
            _uow.Setup(u => u.Products).Returns(_prodRepo.Object);
            _catSvc = new CategoryService(_uow.Object);
            _prodSvc = new ProductService(_uow.Object, _catSvc);
        }

        [TestMethod]
        public async Task Category_Create_DuplicateName_Fail()
        {
            var cat = new Category { Name = "Drink", CategoryType = Models.Enums.CategoryType.PRODUCT };
            _catRepo.Setup(r => r.IsNameExistsAsync("Drink", null)).ReturnsAsync(true);
            var res = await _catSvc.CreateCategoryAsync(cat);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Product_Create_InvalidSku_Duplicate_Fail()
        {
            var prod = new Product { Name = "Water", ProductType = Models.Enums.ProductType.BEVERAGE, Sku = "W001" };
            _prodRepo.Setup(r => r.IsSkuExistsAsync("W001", null)).ReturnsAsync(true);
            var res = await _prodSvc.CreateProductAsync(prod);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Product_GetByCategory_ChecksCategoryExists()
        {
            _catRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Category)null);
            var res = await _prodSvc.GetProductsByCategoryAsync(10);
            Assert.IsFalse(res.Success);
        }
    }
}

