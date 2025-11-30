 namespace StudentAllowanceTracker.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (TaskCanceledException ex)  
            {
                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
                await context.Response.WriteAsync("Request was cancelled");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Something went wrong!",
                Detailed = exception.Message
            };

            var json = System.Text.Json.JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }


    }
}
