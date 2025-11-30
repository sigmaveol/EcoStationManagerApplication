using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IDatabaseBackupService
    {
        Task DumpToSqlFileAsync(string filePath);
        Task RestoreFromSqlFileAsync(string filePath);
    }
}