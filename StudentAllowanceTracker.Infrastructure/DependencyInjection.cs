using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICodeGenerator, CodeGenerator>();
            services.AddScoped<IEmailVerificationCodeRepository, EmailVerificationCodeRepository>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddHttpContextAccessor();
           services.AddScoped<ICurrentUserService, CurrentUserService>();


            return services;
        }
    }
}
