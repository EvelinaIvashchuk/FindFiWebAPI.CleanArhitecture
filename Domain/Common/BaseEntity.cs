using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FindFi.CL.Domain.Common;

/// <summary>
/// Базовий доменний клас для MongoDB документів.
/// Містить стандартні поля Id, CreatedAt, UpdatedAt та версію для оптимістичної конкурентності.
/// </summary>
public abstract class BaseEntity
{
    [BsonId]
    public ObjectId Id { get; protected set; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; protected set; }

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; protected set; }

    /// <summary>
    /// Версія документа для оптимістичної конкурентності (оновлюється інфраструктурою).
    /// </summary>
    [BsonElement("version")]
    public long Version { get; protected set; }

    protected BaseEntity()
    {
        Id = ObjectId.GenerateNewId();
        CreatedAt = UpdatedAt = DateTime.UtcNow;
        Version = 0;
    }

    protected void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
