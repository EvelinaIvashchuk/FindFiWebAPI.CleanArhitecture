using MongoDB.Bson;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Setup;

internal sealed class MongoIndexCreationService(IMongoDatabase database) : IIndexCreationService
{
    public async Task CreateIndexesAsync(CancellationToken ct = default)
    {
        await CreateReviewsIndexesAsync(ct);
        await CreateListingRatingsIndexesAsync(ct);
        await CreateThreadsIndexesAsync(ct);
        await CreateReactionsIndexesAsync(ct);
        await CreateReportsIndexesAsync(ct);
    }

    private async Task CreateReviewsIndexesAsync(CancellationToken ct)
    {
        var col = database.GetCollection<BsonDocument>("reviews");
        var indexModels = new List<CreateIndexModel<BsonDocument>>
        {
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("listingId")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_reviews_listing_created" }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("authorId")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_reviews_author_created" }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("bookingId"),
                new CreateIndexOptions { Name = "idx_reviews_booking" }),
            // optional text index for search
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Text("title")
                    .Text("text"),
                new CreateIndexOptions { Name = "txt_reviews_title_text" })
        };
        await col.Indexes.CreateManyAsync(indexModels, cancellationToken: ct);
    }

    private async Task CreateListingRatingsIndexesAsync(CancellationToken ct)
    {
        var col = database.GetCollection<BsonDocument>("listing_ratings");
        var indexes = new List<CreateIndexModel<BsonDocument>>
        {
            new(new IndexKeysDefinitionBuilder<BsonDocument>().Ascending("listingId"),
                new CreateIndexOptions { Name = "ux_listing_ratings_listing", Unique = true }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>().Descending("updatedAt"),
                new CreateIndexOptions { Name = "idx_listing_ratings_updated" })
        };
        await col.Indexes.CreateManyAsync(indexes, cancellationToken: ct);
    }

    private async Task CreateThreadsIndexesAsync(CancellationToken ct)
    {
        var col = database.GetCollection<BsonDocument>("threads");
        var indexes = new List<CreateIndexModel<BsonDocument>>
        {
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("listingId")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_threads_listing_created" }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("createdBy")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_threads_creator_created" }),
            // text index for title
            new(new IndexKeysDefinitionBuilder<BsonDocument>().Text("title"),
                new CreateIndexOptions { Name = "txt_threads_title" })
        };
        await col.Indexes.CreateManyAsync(indexes, cancellationToken: ct);
    }

    private async Task CreateReactionsIndexesAsync(CancellationToken ct)
    {
        var col = database.GetCollection<BsonDocument>("reactions");
        var indexes = new List<CreateIndexModel<BsonDocument>>
        {
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("targetType")
                    .Ascending("targetId")
                    .Ascending("userId")
                    .Ascending("type"),
                new CreateIndexOptions { Name = "ux_reactions_target_user_type", Unique = true }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>().Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_reactions_created" })
        };
        await col.Indexes.CreateManyAsync(indexes, cancellationToken: ct);
    }

    private async Task CreateReportsIndexesAsync(CancellationToken ct)
    {
        var col = database.GetCollection<BsonDocument>("reports");
        var indexes = new List<CreateIndexModel<BsonDocument>>
        {
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("targetType")
                    .Ascending("targetId")
                    .Ascending("status")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_reports_target_status_created" }),
            new(new IndexKeysDefinitionBuilder<BsonDocument>()
                    .Ascending("reporterId")
                    .Descending("createdAt"),
                new CreateIndexOptions { Name = "idx_reports_reporter_created" })
        };
        await col.Indexes.CreateManyAsync(indexes, cancellationToken: ct);
    }
}
