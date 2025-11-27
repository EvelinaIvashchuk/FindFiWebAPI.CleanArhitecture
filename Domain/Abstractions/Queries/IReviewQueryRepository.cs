using MongoDB.Bson;

namespace FindFi.CL.Domain.Abstractions.Queries;

/// <summary>
/// Доменний контракт для запитів по відгуках з урахуванням специфіки MongoDB
/// (фільтри, пагінація, сортування, текстовий пошук, агрегації).
/// </summary>
public interface IReviewQueryRepository
{
    Task<IReadOnlyList<Entities.Review>> GetByListingAsync(int listingId, int skip = 0, int take = 20, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Review>> GetByAuthorAsync(int authorId, int skip = 0, int take = 20, CancellationToken ct = default);
    Task<(int count, int sumRatings)> AggregateForListingAsync(int listingId, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Review>> TextSearchAsync(int listingId, string query, int skip = 0, int take = 20, CancellationToken ct = default);
}
