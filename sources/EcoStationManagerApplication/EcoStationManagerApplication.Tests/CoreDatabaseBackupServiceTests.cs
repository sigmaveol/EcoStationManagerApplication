using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoStationManagerApplication.Core.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Tests
{
    class FakeDatabaseBackupService : IDatabaseBackupService
    {
        public Task DumpToSqlFileAsync(string filePath)
        {
            File.WriteAllText(filePath, "-- dump");
            return Task.CompletedTask;
        }

        public Task RestoreFromSqlFileAsync(string filePath)
        {
            var _ = File.ReadAllText(filePath);
            return Task.CompletedTask;
        }
    }

    [TestClass]
    public class CoreDatabaseBackupServiceTests
    {
        [TestMethod]
        public async Task DumpAndRestore_Works_On_File()
        {
            var svc = new FakeDatabaseBackupService();
            var temp = Path.ChangeExtension(Path.GetTempFileName(), ".sql");
            try
            {
                await svc.DumpToSqlFileAsync(temp);
                Assert.IsTrue(File.Exists(temp));
                await svc.RestoreFromSqlFileAsync(temp);
            }
            finally
            {
                if (File.Exists(temp)) File.Delete(temp);
            }
        }
    }
}
