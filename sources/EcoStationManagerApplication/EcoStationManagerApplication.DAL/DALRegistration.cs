using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.DAL.Database;
using EcoStationManagerApplication.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace EcoStationManagerApplication.DAL
{
    public class DALRegistration : IDALRegistration
    {
        public IServiceCollection RegisterDAL(IServiceCollection services)
        {
            // Database
            services.AddScoped<IDbHelper, DbHelper>();

            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();  

            foreach (var repoType in repositoryTypes)
            {
                var interfaceType = repoType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{repoType.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repoType);
                }
            }

            return services;
        }
    }
}