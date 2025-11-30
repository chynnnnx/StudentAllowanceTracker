using Serilog;

namespace StudentAllowanceTracker.Api.Extensions
{
    public static class UseApplicationPipelinesExtensions
    {
        public static IApplicationBuilder UseApplicationPipeline(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseExceptionMiddleware();
            app.UseSwaggerWithUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCustomCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();  
             
            });

            return app;
        }
    }
}
