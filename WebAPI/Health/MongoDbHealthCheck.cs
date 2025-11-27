using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebAPI.Health;

public sealed class MongoDbHealthCheck(IMongoDatabase database) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var cmd = new BsonDocument("ping", 1);
            await database.RunCommandAsync<BsonDocument>(cmd, cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("MongoDB reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("MongoDB unreachable", ex);
        }
    }
}
