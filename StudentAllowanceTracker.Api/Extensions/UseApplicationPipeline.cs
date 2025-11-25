using Serilog;

namespace StudentAllowanceTracker.Api.Extensions
{
    public static class UseApplicationPipelinesExtensions
    {
        public static IApplicationBuilder UseApplicationPipeline(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseSwaggerWithUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
