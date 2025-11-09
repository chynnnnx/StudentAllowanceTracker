using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using StudentAllowanceTracker.Client.Security;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StudentAllowanceTracker.Client.Dependency_Injection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, string apiBaseUrl)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.Contains("Services"));

            foreach (var type in serviceTypes)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    services.AddScoped(iface, type);
                }
            }

            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl),
                DefaultRequestVersion = new Version(2, 0)
            });

            return services;
        }
    }
}
