using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class StationServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IStationRepository> _repo;
        private Mock<IUserRepository> _userRepo;
        private StationService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IStationRepository>();
            _userRepo = new Mock<IUserRepository>();
            _uow.Setup(u => u.Stations).Returns(_repo.Object);
            _uow.Setup(u => u.Users).Returns(_userRepo.Object);
            _svc = new StationService(_uow.Object);
        }

        [TestMethod]
        public async Task CreateStation_NameExists_Fails()
        {
            _repo.Setup(r => r.GetByNameAsync("A")).ReturnsAsync(new Station { StationId = 2, Name = "A" });
            var res = await _svc.CreateStationAsync(new Station { Name = "A", Address = "addr" });
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task UpdateStation_NotFound_Fails()
        {
            _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Station)null);
            var res = await _svc.UpdateStationAsync(new Station { StationId = 1, Name = "A", Address = "addr" });
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ToggleStatus_Ok()
        {
            _repo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(new Station { StationId = 3, Name = "S" });
            _repo.Setup(r => r.ToggleStatusAsync(3, true)).ReturnsAsync(true);
            var res = await _svc.ToggleStationStatusAsync(3, true);
            Assert.IsTrue(res.Success);
        }
    }
}
