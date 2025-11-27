using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

internal sealed class ListingRatingRepository(IMongoDatabase database) : IListingRatingRepository
{
    private readonly IMongoCollection<ListingRating> _collection = database.GetCollection<ListingRating>("listing_ratings");

    public Task<ListingRating?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1 (structure only)
        return Task.FromResult<ListingRating?>(null);
    }

    public Task UpsertAsync(ListingRating rating, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1 (structure only)
        return Task.CompletedTask;
    }
}
