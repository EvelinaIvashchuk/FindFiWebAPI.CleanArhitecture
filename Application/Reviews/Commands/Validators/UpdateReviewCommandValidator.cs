using FindFi.CL.Application.Reviews.Commands;
using FindFi.CL.Domain.ValueObjects;
using FluentValidation;
using MongoDB.Bson;

namespace FindFi.CL.Application.Reviews.Commands.Validators;

internal sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Id має бути коректним ObjectId");

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

        When(x => x.AddPhotos is not null, () =>
        {
            RuleForEach(x => x.AddPhotos!)
                .NotEmpty()
                .Must(p => !string.IsNullOrWhiteSpace(p)).WithMessage("Некоректна адреса фото");
        });

        When(x => x.RemovePhotos is not null, () =>
        {
            RuleForEach(x => x.RemovePhotos!)
                .NotEmpty();
        });
    }
}
