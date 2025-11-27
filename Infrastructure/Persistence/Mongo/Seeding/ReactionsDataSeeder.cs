using MongoDB.Bson;
using MongoDB.Driver;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Seeding;

internal sealed class ReactionsDataSeeder(IMongoDatabase database) : IDataSeeder
{
    public string Name => "reactions";

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var docs = SeedJsonLoader.LoadArrayFromResource("reactions.json");
        if (docs.Count == 0) return;

        var col = database.GetCollection<BsonDocument>(Name);
        var models = new List<WriteModel<BsonDocument>>();
        foreach (var d in docs)
        {
            if (!d.Contains("_id")) d["_id"] = ObjectId.GenerateNewId();
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("targetType", d.GetValue("targetType").AsString),
                Builders<BsonDocument>.Filter.Eq("targetId", d.GetValue("targetId").AsObjectId),
                Builders<BsonDocument>.Filter.Eq("userId", d.GetValue("userId").AsInt32),
                Builders<BsonDocument>.Filter.Eq("type", d.GetValue("type").AsString)
            );
            models.Add(new ReplaceOneModel<BsonDocument>(filter, d) { IsUpsert = true });
        }

        if (models.Count > 0)
            await col.BulkWriteAsync(models, new BulkWriteOptions { IsOrdered = false }, ct);
    }
}
