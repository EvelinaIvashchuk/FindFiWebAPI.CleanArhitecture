using FindFi.CL.Application.Reviews.Commands;
using FindFi.CL.Domain.Abstractions.Queries;
using FindFi.CL.Domain.ValueObjects;
using FluentValidation;

namespace FindFi.CL.Application.Reviews.Commands.Validators;

internal sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    private readonly IReviewQueryRepository _reviews;

    public CreateReviewCommandValidator(IReviewQueryRepository reviews)
    {
        _reviews = reviews;
        RuleFor(x => x.AuthorId).GreaterThan(0);
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.ListingId).GreaterThan(0);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title!)
                .NotEmpty()
                .MaximumLength(Title.MaxLength);
        });

        When(x => x.Text is not null, () =>
        {
            RuleFor(x => x.Text!)
                .NotEmpty()
                .MaximumLength(TextContent.MaxLength);
        });

        When(x => x.Photos is not null, () =>
        {
            RuleForEach(x => x.Photos!)
                .NotEmpty()
                .Must(p => !string.IsNullOrWhiteSpace(p)).WithMessage("Некоректна адреса фото");
        });

        // Асинхронна перевірка унікальності: один відгук на бронювання від автора
        RuleFor(x => x)
            .MustAsync(BeUniqueReviewForBookingAsync)
            .WithMessage("Відгук для цього бронювання вже існує");
    }

    private async Task<bool> BeUniqueReviewForBookingAsync(CreateReviewCommand cmd, CancellationToken ct)
    {
        var existing = await _reviews.GetByAuthorAsync(cmd.AuthorId, 0, 50, ct);
        return existing.All(r => r.BookingId != cmd.BookingId);
    }
}
