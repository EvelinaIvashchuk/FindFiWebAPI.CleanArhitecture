using FluentValidation;
using MediatR;

namespace FindFi.CL.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior для автоматичної валідації усіх запитів (Commands/Queries) за допомогою FluentValidation.
/// </summary>
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationTasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
        var results = await Task.WhenAll(validationTasks);
        var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}
