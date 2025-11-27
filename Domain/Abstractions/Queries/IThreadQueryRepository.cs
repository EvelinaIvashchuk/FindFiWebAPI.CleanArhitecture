using MongoDB.Bson;

namespace FindFi.CL.Domain.Abstractions.Queries;

public interface IThreadQueryRepository
{
    Task<IReadOnlyList<Entities.DiscussionThread>> GetByListingAsync(int listingId, int skip = 0, int take = 20, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.DiscussionThread>> GetByCreatorAsync(int creatorId, int skip = 0, int take = 20, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.DiscussionThread>> SearchByTitleAsync(int listingId, string query, int skip = 0, int take = 20, CancellationToken ct = default);
}
