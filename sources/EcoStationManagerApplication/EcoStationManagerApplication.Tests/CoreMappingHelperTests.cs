using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Helpers;
using EcoStationManagerApplication.Models.Entities;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CoreMappingHelperTests
    {
        [TestMethod]
        public void MapToUserDTO_Should_Copy_Fields()
        {
            var user = new User
            {
                UserId = 1,
                Username = "u",
                Fullname = "User Name",
                Role = "Admin",
                IsActive = true
            };

            var dto = MappingHelper.MapToUserDTO(user);
            Assert.AreEqual(1, dto.UserId);
            Assert.AreEqual("u", dto.Username);
            Assert.AreEqual("User Name", dto.Fullname);
            Assert.AreEqual("Admin", dto.Role);
            Assert.IsTrue(dto.IsActive);
        }
    }
}
