using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IListingRatingRepository
{
    Task<ListingRating?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default);
    Task UpsertAsync(ListingRating rating, CancellationToken cancellationToken = default);
}
