using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options; 
using StudentAllowanceTracker.Application.Interfaces;
using StudentAllowanceTracker.Application.Interfaces.Repositories;
using StudentAllowanceTracker.Infrastructure.Persistence.Data;
using StudentAllowanceTracker.Infrastructure.Persistence.Repositories;
using StudentAllowanceTracker.Infrastructure.Services;
using StudentAllowanceTracker.Infrastructure.Settings;

namespace StudentAllowanceTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddHttpContextAccessor();


            var assembly = typeof(CurrentUserService).Assembly;
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.Where(type =>
                        (type.Name.EndsWith("Repository") || type.Name.EndsWith("Service")) &&
                        !typeof(BackgroundService).IsAssignableFrom(type)))

                .AsImplementedInterfaces()
                .WithScopedLifetime());
            services.AddHostedService<DailyReminderService>();


            return services;
        }
    }
}
