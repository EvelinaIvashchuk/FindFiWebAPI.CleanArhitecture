using FindFi.CL.Domain.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FindFi.CL.Domain.ValueObjects;

/// <summary>
/// Рейтинг 1..5 як Value Object.
/// </summary>
public sealed class Rating : ValueObject
{
    public int Value { get; }

    private Rating(int value)
    {
        if (value is < 1 or > 5)
            throw new DomainException("Рейтинг повинен бути у діапазоні 1..5");
        Value = value;
    }

    public static Rating Create(int value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    // BSON Serializer as nested type for convenience
    public sealed class BsonSerializer : SerializerBase<Rating>
    {
        public override Rating Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadInt32();
            return Create(value);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Rating value)
        {
            context.Writer.WriteInt32(value.Value);
        }
    }
}
