using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Application.Abstractions.Repositories;
using MediatR;

namespace FindFi.CL.Application.Reviews.Commands;

public sealed record DeleteReviewCommand(string Id) : ICommand<bool>;

internal sealed class DeleteReviewCommandHandler(IReviewRepository repository)
    : IRequestHandler<DeleteReviewCommand, bool>
{
    public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
