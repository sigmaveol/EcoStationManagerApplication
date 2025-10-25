using EcoStationManagerApplication.Core.Interfaces;
using EcoStationManagerApplication.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace EcoStationManagerApplication.Core
{
    public static class DALRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Đăng ký tất cả các service
            var serviceTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .ToList();

            foreach (var serviceType in serviceTypes)
            {
                // Tìm interface tương ứng
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }

            return services;
        }     
        
        // Phương thức mới để đăng ký DAL thông qua interface
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IDALRegistration dalRegistration)
        {
            return dalRegistration.RegisterDAL(services);
        }
    }
}