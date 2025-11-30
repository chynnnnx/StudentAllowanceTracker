using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle( TRequest request, RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();
 
        var json = JsonSerializer.Serialize(request);
         
        var safeJson = MaskSensitiveData(json);

        _logger.LogInformation("Handling {RequestName} with payload: {Payload}", requestName, safeJson);

        var response = await next();

        stopwatch.Stop();
        _logger.LogInformation(
            "Handled {RequestName} in {ElapsedMilliseconds} ms",
            requestName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }

    private string MaskSensitiveData(string json)
    {
        string[] sensitiveFields = { "password", "newpassword", "oldpassword", "confirmpassword", "token" };

        foreach (var field in sensitiveFields)
        { 
            json = Regex.Replace(
                json,
                $"\"{field}\"\\s*:\\s*\".*?\"",
                $"\"{field}\":\"***MASKED***\"",
                RegexOptions.IgnoreCase);
        }

        return json;
    }
}
