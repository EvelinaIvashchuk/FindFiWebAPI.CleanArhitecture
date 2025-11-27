using FindFi.CL.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FindFi.CL.Domain.Entities;

public sealed class Reaction : BaseEntity
{
    private static readonly HashSet<string> AllowedTargets = new(StringComparer.OrdinalIgnoreCase)
        { "review", "thread", "message" };
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
        { "like", "dislike" };

    [BsonElement("targetId")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId TargetId { get; private set; }
    [BsonElement("targetType")]
    public string TargetType { get; private set; } = string.Empty;
    [BsonElement("type")]
    public string Type { get; private set; } = string.Empty;
    [BsonElement("userId")]
    public int UserId { get; private set; }

    private Reaction() { }

    private Reaction(ObjectId targetId, string targetType, string type, int userId)
    {
        if (userId <= 0) throw new DomainException("UserId має бути > 0");
        if (!AllowedTargets.Contains(targetType)) throw new DomainException("Неприпустимий TargetType");
        if (!AllowedTypes.Contains(type)) throw new DomainException("Неприпустимий тип реакції");
        TargetId = targetId;
        TargetType = targetType.ToLowerInvariant();
        Type = type.ToLowerInvariant();
        UserId = userId;
    }

    public static Reaction Create(ObjectId targetId, string targetType, string type, int userId)
        => new(targetId, targetType, type, userId);
}
