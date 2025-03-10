using Booking.Domain.Commons;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Booking.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest where TResponse : Result
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                logger.LogInformation("Handling request {requestName}", requestName);
                TResponse result = await next();

                if (result.IsSuccess)
                {
                    logger.LogInformation("Request {requestName} handled successfully", requestName);
                }
                else
                {
                    using (LogContext.PushProperty("Error", result.Error, true))
                    {
                        logger.LogError("Request {RequestName} processed with error", requestName);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Request {requestName} failed", requestName);
                throw;
            }
        }
    }
}
