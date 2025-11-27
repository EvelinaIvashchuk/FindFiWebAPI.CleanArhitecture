using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FindFi.CL.Application.Common.Behaviors;

/// <summary>
/// Централізоване логування помилок у MediatR конвеєрі.
/// </summary>
internal sealed class ExceptionHandlingBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (ValidationException)
        {
            // Валідаційні помилки не дублюємо в логах як помилки рівня Error
            throw;
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogError(ex, "Unhandled exception processing {RequestName}: {@Request}", requestName, request);
            throw;
        }
    }
}
