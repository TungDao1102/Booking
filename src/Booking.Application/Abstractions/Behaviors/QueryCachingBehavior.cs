using Booking.Application.Abstractions.Caching;
using Booking.Domain.Commons;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Booking.Application.Abstractions.Behaviors
{
    public class QueryCachingBehavior<TRequest, TResponse>(
        ICacheService cacheService,
        ILogger<QueryCachingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
            string requestName = typeof(TRequest).Name;
            if (cachedResult is not null)
            {
                logger.LogInformation("Cache hit for {request}", requestName);
                return cachedResult;
            }

            logger.LogInformation("Cache miss for {request}", requestName);
            var result = await next();
            if (result.IsSuccess)
            {
                await cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
            }

            return result;
        }
    }
}
