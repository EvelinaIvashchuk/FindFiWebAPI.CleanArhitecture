using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FindFi.CL.Application.Common.Behaviors;

/// <summary>
/// Логування всіх запитів/відповідей з базовим контекстом.
/// </summary>
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);

        var response = await next();

        logger.LogInformation("Handled {RequestName} -> {ResponseType}", requestName, typeof(TResponse).Name);
        return response;
    }
}
