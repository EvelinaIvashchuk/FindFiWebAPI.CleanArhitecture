using FindFi.CL.Application.Abstractions.Repositories;
using FindFi.CL.Domain.Entities;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Repositories;

internal sealed class ReportRepository(IMongoDatabase database) : IReportRepository
{
    private readonly IMongoCollection<Report> _collection = database.GetCollection<Report>("reports");

    public Task<Report?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.FromResult<Report?>(null);
    }

    public Task AddAsync(Report report, CancellationToken cancellationToken = default)
    {
        // Skeleton implementation for Part 1
        return Task.CompletedTask;
    }
}
