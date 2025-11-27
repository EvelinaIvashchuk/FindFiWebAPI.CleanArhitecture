namespace FindFi.CL.Infrastructure.Configuration;

public sealed class MongoOptions
{
    public const string SectionName = "MongoDb";
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;

    // Optional pooling/timeouts (apply if > 0)
    public int MaxPoolSize { get; init; } = 0;
    public int MinPoolSize { get; init; } = 0;
    public int ConnectTimeoutSeconds { get; init; } = 0;
    public int SocketTimeoutSeconds { get; init; } = 0;
    public int ServerSelectionTimeoutSeconds { get; init; } = 0;

    // Run data seeders at startup (typically enabled only in Development)
    public bool Seed { get; init; } = false;
}
