using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using EcoStationManagerApplication.Core.Services;
using EcoStationManagerApplication.DAL.Database;

namespace EcoStationManagerApplication.Tests
{
    [TestClass]
    public class CoreDatabaseBackupServiceTests
    {
        [TestMethod]
        public async Task RestoreFromSqlFileAsync_FileNotFound_Throws()
        {
            var svc = new DatabaseBackupService(new DatabaseHelper("Server=localhost;Database=eco;User Id=root;Password=invalid;"));
            await Assert.ThrowsExceptionAsync<System.IO.FileNotFoundException>(async () =>
            {
                await svc.RestoreFromSqlFileAsync("D:/missing-file.sql");
            });
        }
    }
}

