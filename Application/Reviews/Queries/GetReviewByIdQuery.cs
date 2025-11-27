using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MediatR;

namespace FindFi.CL.Application.Reviews.Queries;

public sealed record GetReviewByIdQuery(string Id) : IQuery<Review?>;

internal sealed class GetReviewByIdQueryHandler(IReviewRepository repository)
    : IRequestHandler<GetReviewByIdQuery, Review?>
{
    public Task<Review?> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        => repository.GetByIdAsync(request.Id, cancellationToken);
}
