using FindFi.CL.Domain.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FindFi.CL.Domain.ValueObjects;

/// <summary>
/// Заголовок (наприклад для треду або відгуку). Максимум 200 символів.
/// </summary>
public sealed class Title : ValueObject
{
    public const int MaxLength = 200;
    public string Value { get; }

    private Title(string value)
    {
        value = value.Trim();
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Title не може бути порожнім");
        if (value.Length > MaxLength) throw new DomainException($"Title довжина > {MaxLength}");
        Value = value;
    }

    public static Title Create(string value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public sealed class BsonSerializer : SerializerBase<Title>
    {
        public override Title Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return Create(value);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Title value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}
