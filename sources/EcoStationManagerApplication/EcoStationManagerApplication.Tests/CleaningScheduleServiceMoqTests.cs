using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CleaningScheduleServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<ICleaningScheduleRepository> _repo;
        private CleaningScheduleService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<ICleaningScheduleRepository>();
            _uow.Setup(u => u.CleaningSchedules).Returns(_repo.Object);
            _svc = new CleaningScheduleService(_uow.Object);
        }

        [TestMethod]
        public async Task Create_PastDate_Fails()
        {
            var s = new CleaningSchedule { CleaningDate = DateTime.Now.AddDays(-1), CleaningType = CleaningType.TANK };
            var res = await _svc.CreateAsync(s);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Update_NotFound_Fails()
        {
            var s = new CleaningSchedule { CsId = 10, CleaningDate = DateTime.Now.AddDays(1) };
            _repo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((CleaningSchedule)null);
            var res = await _svc.UpdateAsync(s);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task MarkAsCompleted_Cancelled_Fails()
        {
            var id = 20;
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new CleaningSchedule { CsId = id, Status = CleaningStatus.CANCELLED });
            var res = await _svc.MarkAsCompletedAsync(id, cleanedBy: 1, notes: "done");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Cancel_Completed_Fails()
        {
            var id = 21;
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new CleaningSchedule { CsId = id, Status = CleaningStatus.COMPLETED });
            var res = await _svc.CancelAsync(id, reason: "no need");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Delete_Valid_Ok()
        {
            var id = 22;
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new CleaningSchedule { CsId = id, Status = CleaningStatus.SCHEDULED });
            _repo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);
            var res = await _svc.DeleteAsync(id);
            Assert.IsTrue(res.Success);
            _repo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
