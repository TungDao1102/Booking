using Booking.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Booking.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                logger.LogInformation("Handling Command {requestName}", requestName);
                var result = await next();
                logger.LogInformation("Command {requestName} handled successfully", requestName);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Command {requestName} failed", requestName);
                throw;
            }
        }
    }
}
