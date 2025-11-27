using FindFi.CL.Domain.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace FindFi.CL.Domain.ValueObjects;

/// <summary>
/// Текстовий вміст (наприклад для відгуку або повідомлення). Максимум 4000 символів.
/// </summary>
public sealed class TextContent : ValueObject
{
    public const int MaxLength = 4000;
    public string Value { get; }

    private TextContent(string value)
    {
        value = value.Trim();
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Text не може бути порожнім");
        if (value.Length > MaxLength) throw new DomainException($"Text довжина > {MaxLength}");
        Value = value;
    }

    public static TextContent Create(string value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public sealed class BsonSerializer : SerializerBase<TextContent>
    {
        public override TextContent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return Create(value);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TextContent value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}
