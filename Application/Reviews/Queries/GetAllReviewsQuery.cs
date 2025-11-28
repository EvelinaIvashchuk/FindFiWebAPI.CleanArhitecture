using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Application.Common.Behaviors;
using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Domain.Abstractions.Queries;
using FindFi.CL.Domain.Entities;
using MediatR;

namespace FindFi.CL.Application.Reviews.Queries;

public sealed record  GetAllReviewsQuery(
    int Skip = 0,
    int Take = 20 ) : IQuery<IReadOnlyList<Review>>, ICacheableRequest<IReadOnlyList<Review>>
{
    public string CacheKey => $"reviews:s:{Skip}:t:{Take}";
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromSeconds(30);
}

internal sealed class GetAllReviewsQueryHandler(IReviewRepository repository)
    : IRequestHandler<GetAllReviewsQuery, IReadOnlyList<Review>>
{
    public Task<IReadOnlyList<Review>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        => repository.GetAllAsync(request.Skip, request.Take, cancellationToken);
}