using FindFi.CL.Domain.Abstractions.Queries;
using FindFi.CL.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// MongoDB implementation for read-optimized review queries.
/// </summary>
internal sealed class ReviewQueryRepository(IMongoDatabase database) : IReviewQueryRepository
{
    private readonly IMongoCollection<Review> _collection = database.GetCollection<Review>("reviews");

    public async Task<IReadOnlyList<Review>> GetByListingAsync(int listingId, int skip = 0, int take = 20, CancellationToken ct = default)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.ListingId, listingId);
        var sort = Builders<Review>.Sort.Descending(x => x.CreatedAt);
        var list = await _collection
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(ct);
        return list;
    }

    public async Task<IReadOnlyList<Review>> GetByAuthorAsync(int authorId, int skip = 0, int take = 20, CancellationToken ct = default)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.AuthorId, authorId);
        var sort = Builders<Review>.Sort.Descending(x => x.CreatedAt);
        var list = await _collection
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(ct);
        return list;
    }

    public async Task<(int count, int sumRatings)> AggregateForListingAsync(int listingId, CancellationToken ct = default)
    {
        // Use BsonDocument pipeline to aggregate over the raw stored fields (rating is stored as int under "rating")
        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("listingId", listingId)),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "count", new BsonDocument("$sum", 1) },
                { "sumRatings", new BsonDocument("$sum", "$rating") }
            })
        };

        var result = await _collection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync(ct);
        if (result is null)
            return (0, 0);

        var count = result.GetValue("count", 0).ToInt32();
        var sum = result.GetValue("sumRatings", 0).ToInt32();
        return (count, sum);
    }

    public async Task<IReadOnlyList<Review>> TextSearchAsync(int listingId, string query, int skip = 0, int take = 20, CancellationToken ct = default)
    {
        // Case-insensitive regex against title or text within the same listing
        var regex = new BsonRegularExpression(query, "i");
        var filter = Builders<Review>.Filter.And(
            Builders<Review>.Filter.Eq(x => x.ListingId, listingId),
            Builders<Review>.Filter.Or(
                Builders<Review>.Filter.Regex("title", regex),
                Builders<Review>.Filter.Regex("text", regex)
            )
        );

        var sort = Builders<Review>.Sort.Descending(x => x.CreatedAt);
        var list = await _collection
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(ct);
        return list;
    }
}
