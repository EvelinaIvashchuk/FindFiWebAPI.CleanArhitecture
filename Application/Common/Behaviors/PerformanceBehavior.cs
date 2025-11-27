using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FindFi.CL.Application.Common.Behaviors;

/// <summary>
/// Вимірює тривалість виконання запитів і попереджає, якщо поріг перевищено.
/// </summary>
internal sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int WarningThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            return await next();
        }
        finally
        {
            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            var requestName = typeof(TRequest).Name;
            if (elapsed > WarningThresholdMs)
                logger.LogWarning("{RequestName} handled in {Elapsed} ms", requestName, elapsed);
            else
                logger.LogDebug("{RequestName} handled in {Elapsed} ms", requestName, elapsed);
        }
    }
}
