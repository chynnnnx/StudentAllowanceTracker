using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
namespace StudentAllowanceTracker.Application.Common.Behaviors
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next , CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for request {RequestType}: {@Request}", typeof(TRequest).Name, request);
                throw;
            }
        }
    }
}
