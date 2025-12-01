using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class WorkShiftServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IWorkShiftRepository> _repo;
        private WorkShiftService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _repo = new Mock<IWorkShiftRepository>();
            _uow.Setup(u => u.WorkShifts).Returns(_repo.Object);
            _svc = new WorkShiftService(_uow.Object);
        }

        [TestMethod]
        public async Task Create_Duplicate_Fails()
        {
            var shift = new WorkShift { UserId = 1, ShiftDate = DateTime.Today };
            _repo.Setup(r => r.ExistsByUserIdAndDateAsync(1, shift.ShiftDate)).ReturnsAsync(true);
            var res = await _svc.CreateAsync(shift);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task Update_NotFound_Fails()
        {
            var shift = new WorkShift { ShiftId = 10, UserId = 1, ShiftDate = DateTime.Today };
            _repo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((WorkShift)null);
            var res = await _svc.UpdateAsync(shift);
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task UpdateStartTime_Ok()
        {
            _repo.Setup(r => r.UpdateStartTimeAsync(5, TimeSpan.FromHours(8))).ReturnsAsync(true);
            var res = await _svc.UpdateStartTimeAsync(5, TimeSpan.FromHours(8));
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task CalculateKPI_Updates_Shift()
        {
            var shift = new WorkShift { ShiftId = 7 };
            _repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(shift);
            _repo.Setup(r => r.UpdateAsync(It.Is<WorkShift>(w => w.KpiScore > 0))).ReturnsAsync(true);
            var res = await _svc.CalculateKPIAsync(7, ordersHandled: 10, targetOrders: 20);
            Assert.IsTrue(res.Success);
        }
    }
}
