using StudentAllowanceTracker.Api.Middlewares;
namespace StudentAllowanceTracker.Api.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
