using FindFi.CL.Application.Common.CQRS;
using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Application.Common.Exceptions;
using FindFi.CL.Domain.ValueObjects;
using MediatR;

namespace FindFi.CL.Application.Reviews.Commands;

public sealed record UpdateReviewCommand(
    string Id,
    string? Title,
    string? Text,
    bool? IsVisible,
    IReadOnlyCollection<string>? AddPhotos,
    IReadOnlyCollection<string>? RemovePhotos
) : ICommand<bool>;

internal sealed class UpdateReviewCommandHandler(IReviewRepository repository)
    : IRequestHandler<UpdateReviewCommand, bool>
{
    public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new NotFoundException("Відгук не знайдено");

        if (request.Title is not null)
            entity.UpdateTitle(Title.Create(request.Title));

        if (request.Text is not null)
            entity.UpdateText(TextContent.Create(request.Text));

        if (request.IsVisible is not null)
            entity.SetVisibility(request.IsVisible.Value);

        if (request.AddPhotos is not null)
            foreach (var p in request.AddPhotos)
                entity.AddPhoto(p);

        if (request.RemovePhotos is not null)
            foreach (var p in request.RemovePhotos)
                entity.RemovePhoto(p);

        await repository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}
