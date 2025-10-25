using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core.Interfaces
{
    public interface IDependencyInjectionDAL
    {
        IServiceCollection RegisterDAL(IServiceCollection services);
    }
}