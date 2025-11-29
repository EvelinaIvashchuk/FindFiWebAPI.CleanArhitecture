using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Application.Common.CQRS;
using MediatR;

namespace FindFi.CL.Application.Reviews.Queries;

public sealed record GetReviewsCountQuery : IQuery<int>;

internal sealed class GetReviewsCountQueryHandler(IReviewRepository repository)
    : IRequestHandler<GetReviewsCountQuery, int>
{
    public Task<int> Handle(GetReviewsCountQuery request, CancellationToken cancellationToken)
    {
        return repository.GetReviewsCount(cancellationToken);
    }
}