using Microsoft.Extensions.DependencyInjection;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IDALRegistration
    {
        IServiceCollection RegisterDAL(IServiceCollection services);
    }
}