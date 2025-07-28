using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Messaging;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public sealed class CachingBehavior<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return cacheService.GetOrSetAsync(
            request.CacheKey,
            (token) => next(token),
            request.CacheExpiration,
            cancellationToken);
    }
}