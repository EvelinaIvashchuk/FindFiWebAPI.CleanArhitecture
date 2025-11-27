using FindFi.CL.Application.Reviews.Commands;
using FluentValidation;
using MongoDB.Bson;

namespace FindFi.CL.Application.Reviews.Commands.Validators;

internal sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Id має бути коректним ObjectId");
    }
}
