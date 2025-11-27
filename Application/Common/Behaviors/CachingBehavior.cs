using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace FindFi.CL.Application.Common.Behaviors;

/// <summary>
/// Простий кешуючий behavior для read-only запитів, які реалізують ICacheableRequest.
/// </summary>
internal sealed class CachingBehavior<TRequest, TResponse>(IMemoryCache cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableRequest<TResponse> cacheable)
            return await next();

        if (cache.TryGetValue(cacheable.CacheKey, out TResponse? cached) && cached is not null)
            return cached;

        var response = await next();
        var ttl = cacheable.AbsoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(1);
        cache.Set(cacheable.CacheKey, response!, ttl);
        return response;
    }
}

public interface ICacheableRequest<TResponse>
{
    string CacheKey { get; }
    TimeSpan? AbsoluteExpirationRelativeToNow { get; }
}
