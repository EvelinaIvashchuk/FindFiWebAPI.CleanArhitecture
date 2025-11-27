using FindFi.CL.Application.Common.Behaviors;
using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Domain.Abstractions.Queries;
using FindFi.CL.Domain.Entities;
using MediatR;

namespace FindFi.CL.Application.Reviews.Queries;

public sealed record GetReviewsByListingQuery(
    int ListingId,
    int Skip = 0,
    int Take = 20
) : IQuery<IReadOnlyList<Review>>, ICacheableRequest<IReadOnlyList<Review>>
{
    public string CacheKey => $"reviews:list:{ListingId}:s:{Skip}:t:{Take}";
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromSeconds(30);
}

internal sealed class GetReviewsByListingQueryHandler(IReviewQueryRepository repository)
    : IRequestHandler<GetReviewsByListingQuery, IReadOnlyList<Review>>
{
    public Task<IReadOnlyList<Review>> Handle(GetReviewsByListingQuery request, CancellationToken cancellationToken)
        => repository.GetByListingAsync(request.ListingId, request.Skip, request.Take, cancellationToken);
}
