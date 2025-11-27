using FindFi.CL.Application.Reviews.Queries;
using FluentValidation;
using MongoDB.Bson;

namespace FindFi.CL.Application.Reviews.Queries.Validators;

internal sealed class GetReviewByIdQueryValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => ObjectId.TryParse(id, out _)).WithMessage("Id має бути коректним ObjectId");
    }
}
