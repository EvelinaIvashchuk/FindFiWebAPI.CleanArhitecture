using System.Reflection;
using MongoDB.Bson;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Seeding;

internal static class SeedJsonLoader
{
    public static IReadOnlyList<BsonDocument> LoadArrayFromResource(string fileName)
    {
        var asm = Assembly.GetExecutingAssembly();
        var name = asm.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
        if (name is null)
            return Array.Empty<BsonDocument>();

        using var stream = asm.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonArray>(json);
        return bson.Select(v => v.AsBsonDocument).ToList();
    }
}
