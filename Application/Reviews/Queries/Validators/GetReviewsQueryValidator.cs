using FluentValidation;

namespace FindFi.CL.Application.Reviews.Queries.Validators;

public class GetReviewsQueryValidator : AbstractValidator<GetAllReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}