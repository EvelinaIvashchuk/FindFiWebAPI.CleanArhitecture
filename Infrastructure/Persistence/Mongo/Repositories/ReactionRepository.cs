using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

internal sealed class ReactionRepository(IMongoDatabase database) : IReactionRepository
{
    private readonly IMongoCollection<Reaction> _collection = database.GetCollection<Reaction>("reactions");

    public Task<Reaction?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.FromResult<Reaction?>(null);
    }

    public Task AddAsync(Reaction reaction, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.CompletedTask;
    }
}
