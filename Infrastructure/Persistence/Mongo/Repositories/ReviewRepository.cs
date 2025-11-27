using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

internal sealed class ReviewRepository(IMongoDatabase database) : IReviewRepository
{
    private readonly IMongoCollection<Review> _collection = database.GetCollection<Review>("reviews");

    public async Task<Review?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(id, out var objectId)) return null;
        var filter = Builders<Review>.Filter.Eq(x => x.Id, objectId);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public Task AddAsync(Review review, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(review, cancellationToken: cancellationToken);

    public async Task UpdateAsync(Review review, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.Id, review.Id);
        await _collection.ReplaceOneAsync(filter, review, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(id, out var objectId)) return;
        var filter = Builders<Review>.Filter.Eq(x => x.Id, objectId);
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }
}
