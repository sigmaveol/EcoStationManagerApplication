using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Interfaces;
using EcoStationManagerApplication.Models.Entities;
using EcoStationManagerApplication.Models.Enums;
using EcoStationManagerApplication.Common.Helpers;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class UserServiceMoqTests
    {
        private Mock<IUnitOfWork> _uow;
        private Mock<IUserRepository> _userRepo;
        private Mock<IWorkShiftRepository> _shiftRepo;
        private UserService _svc;

        [TestInitialize]
        public void Setup()
        {
            _uow = new Mock<IUnitOfWork>();
            _userRepo = new Mock<IUserRepository>();
            _shiftRepo = new Mock<IWorkShiftRepository>();
            _uow.Setup(u => u.Users).Returns(_userRepo.Object);
            _uow.Setup(u => u.WorkShifts).Returns(_shiftRepo.Object);
            _svc = new UserService(_uow.Object);
        }

        [TestMethod]
        public async Task Authenticate_Success()
        {
            var user = new User { UserId = 1, Username = "u", PasswordHash = SecurityHelper.HashPassword("p"), IsActive = ActiveStatus.ACTIVE };
            _userRepo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
            _shiftRepo.Setup(r => r.ExistsByUserIdAndDateAsync(1, It.IsAny<System.DateTime>())).ReturnsAsync(true);
            var res = await _svc.AuthenticateAsync("u", "p");
            Assert.IsTrue(res.Success);
        }

        [TestMethod]
        public async Task Authenticate_Inactive_Fails()
        {
            var user = new User { UserId = 1, Username = "u", PasswordHash = SecurityHelper.HashPassword("p"), IsActive = ActiveStatus.INACTIVE };
            _userRepo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
            var res = await _svc.AuthenticateAsync("u", "p");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ChangePassword_WrongCurrent_Fails()
        {
            var user = new User { UserId = 1, Username = "u", PasswordHash = SecurityHelper.HashPassword("old") };
            _userRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            var res = await _svc.ChangePasswordAsync(1, "wrong", "newpass");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public async Task ToggleUserStatus_NotFound_Fails()
        {
            _userRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);
            var res = await _svc.ToggleUserStatusAsync(1, true);
            Assert.IsFalse(res.Success);
        }
    }
}
