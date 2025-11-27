using System.Diagnostics;
using System.Net.Mime;
using FluentValidation;
using FindFi.CL.Application.Common.Exceptions;
using FindFi.CL.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FindFi.CL.WebAPI.Common.Middleware;

/// <summary>
/// Глобальне middleware для централізованої обробки винятків і повернення стандартних ProblemDetails.
/// Враховує MongoDB специфічні помилки.
/// </summary>
public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var (status, title, detail) = MapException(exception);

        ProblemDetails problem;

        if (exception is ValidationException fvEx)
        {
            var errors = fvEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var vpd = new ValidationProblemDetails(errors)
            {
                Status = status,
                Title = title,
                Detail = detail
            };
            problem = vpd;
        }
        else
        {
            problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail
            };
        }

        // Додатковий контекст
        problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;
        problem.Extensions["exceptionType"] = exception.GetType().FullName;

        // Логування помилки
        if (status is >= 500)
            logger.LogError(exception, "HTTP {Status}: {Title} - {Detail}", status, title, detail);
        else
            logger.LogWarning(exception, "HTTP {Status}: {Title} - {Detail}", status, title, detail);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem);
    }

    private static (int Status, string Title, string Detail) MapException(Exception ex)
    {
        switch (ex)
        {
            case ValidationException v:
                return (StatusCodes.Status400BadRequest, "Validation failed", v.Message);

            case NotFoundException nf:
                return (StatusCodes.Status404NotFound, "Not Found", nf.Message);

            case ConflictException cf:
                return (StatusCodes.Status409Conflict, "Conflict", cf.Message);

            case DomainException de:
                return (StatusCodes.Status400BadRequest, "Domain rule violation", de.Message);

            case MongoConnectionException mce:
                return (StatusCodes.Status503ServiceUnavailable, "MongoDB connection error", mce.Message);

            case MongoWriteException mwe:
                var isDuplicate = mwe.WriteError?.Category == ServerErrorCategory.DuplicateKey;
                return (isDuplicate ? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest,
                    isDuplicate ? "Duplicate key" : "MongoDB write error",
                    mwe.Message);

            case MongoBulkWriteException mbwe:
                var dup = mbwe.WriteErrors.Any(e => e.Category == ServerErrorCategory.DuplicateKey);
                return (dup ? StatusCodes.Status409Conflict : StatusCodes.Status400BadRequest,
                    dup ? "Duplicate key" : "MongoDB bulk write error",
                    mbwe.Message);

            case MongoException me:
                return (StatusCodes.Status500InternalServerError, "MongoDB error", me.Message);

            default:
                return (StatusCodes.Status500InternalServerError, "Internal Server Error", ex.Message);
        }
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
