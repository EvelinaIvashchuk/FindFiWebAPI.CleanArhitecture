using FindFi.CL.Application.Reviews.Queries;
using FluentValidation;

namespace FindFi.CL.Application.Reviews.Queries.Validators;

internal sealed class GetReviewsByListingQueryValidator : AbstractValidator<GetReviewsByListingQuery>
{
    public GetReviewsByListingQueryValidator()
    {
        RuleFor(x => x.ListingId).GreaterThan(0);
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).InclusiveBetween(1, 200);
    }
}
