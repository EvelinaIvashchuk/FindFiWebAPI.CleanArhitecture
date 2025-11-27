using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

internal sealed class ThreadRepository(IMongoDatabase database) : IThreadRepository
{
    private readonly IMongoCollection<DiscussionThread> _collection = database.GetCollection<DiscussionThread>("threads");

    public Task<DiscussionThread?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.FromResult<DiscussionThread?>(null);
    }

    public Task AddAsync(DiscussionThread thread, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.CompletedTask;
    }
}
