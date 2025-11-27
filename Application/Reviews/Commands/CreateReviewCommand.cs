using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using FindFi.CL.Domain.ValueObjects;
using MediatR;

namespace FindFi.CL.Application.Reviews.Commands;

public sealed record CreateReviewCommand(
    int AuthorId,
    int BookingId,
    int ListingId,
    int Rating,
    string? Title,
    string? Text,
    IReadOnlyCollection<string>? Photos
) : ICommand<string>;

internal sealed class CreateReviewCommandHandler(IReviewRepository repository)
    : IRequestHandler<CreateReviewCommand, string>
{
    public async Task<string> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var titleVo = request.Title is null ? null : Title.Create(request.Title);
        var textVo = request.Text is null ? null : TextContent.Create(request.Text);
        var ratingVo = Rating.Create(request.Rating);

        var entity = Review.Create(
            request.AuthorId,
            request.BookingId,
            request.ListingId,
            ratingVo,
            titleVo,
            textVo,
            request.Photos);

        await repository.AddAsync(entity, cancellationToken);
        return entity.Id.ToString();
    }
}
