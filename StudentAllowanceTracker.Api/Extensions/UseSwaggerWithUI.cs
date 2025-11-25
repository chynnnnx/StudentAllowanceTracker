using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace StudentAllowanceTracker.Api.Extensions
{
    public static class SwaggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentAllowanceTracker API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            return app;
        }
    }
}
