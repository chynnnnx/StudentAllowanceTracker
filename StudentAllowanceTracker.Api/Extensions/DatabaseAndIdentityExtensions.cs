using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Infrastructure.Persistence.Data;

namespace StudentAllowanceTracker.Api.Extensions
{
    public static class DatabaseAndIdentityExtensions
    {
        public static IServiceCollection AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
