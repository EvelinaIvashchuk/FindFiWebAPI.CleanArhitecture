using MongoDB.Bson;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Seeding;

internal sealed class ReviewsDataSeeder(IMongoDatabase database) : IDataSeeder
{
    public string Name => "reviews";

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var docs = SeedJsonLoader.LoadArrayFromResource("reviews.json");
        if (docs.Count == 0) return;

        var col = database.GetCollection<BsonDocument>(Name);
        var models = new List<WriteModel<BsonDocument>>();
        foreach (var d in docs)
        {
            if (!d.Contains("_id"))
            {
                d["_id"] = ObjectId.GenerateNewId();
            }
            var filter = Builders<BsonDocument>.Filter.Eq("_id", d["_id"]);
            models.Add(new ReplaceOneModel<BsonDocument>(filter, d) { IsUpsert = true });
        }

        if (models.Count > 0)
            await col.BulkWriteAsync(models, new BulkWriteOptions { IsOrdered = false }, ct);
    }
}
