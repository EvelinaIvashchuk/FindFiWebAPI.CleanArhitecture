using FindFi.CL.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace FindFi.CL.Domain.Entities;

/// <summary>
/// Відгук до оголошення. Інкапсульований та адаптований під MongoDB.
/// </summary>
public sealed class Review : BaseEntity
{
    [BsonElement("authorId")]
    public int AuthorId { get; private set; }
    [BsonElement("bookingId")]
    public int BookingId { get; private set; }
    [BsonElement("isVisible")]
    public bool IsVisible { get; private set; }
    [BsonElement("listingId")]
    public int ListingId { get; private set; }

    [BsonElement("photos")]
    private readonly List<string> _photos = new();
    public IReadOnlyList<string> Photos => _photos;

    [BsonElement("title")]
    [BsonSerializer(typeof(global::FindFi.CL.Domain.ValueObjects.Title.BsonSerializer))]
    public global::FindFi.CL.Domain.ValueObjects.Title? Title { get; private set; }

    [BsonElement("text")]
    [BsonSerializer(typeof(global::FindFi.CL.Domain.ValueObjects.TextContent.BsonSerializer))]
    public global::FindFi.CL.Domain.ValueObjects.TextContent? Text { get; private set; }

    [BsonElement("rating")]
    [BsonSerializer(typeof(global::FindFi.CL.Domain.ValueObjects.Rating.BsonSerializer))]
    public global::FindFi.CL.Domain.ValueObjects.Rating Stars { get; private set; }

    private Review() { }

    private Review(int authorId, int bookingId, int listingId, global::FindFi.CL.Domain.ValueObjects.Rating rating,
        global::FindFi.CL.Domain.ValueObjects.Title? title,
        global::FindFi.CL.Domain.ValueObjects.TextContent? text,
        IEnumerable<string>? photos)
    {
        if (authorId <= 0) throw new DomainException("AuthorId має бути > 0");
        if (bookingId <= 0) throw new DomainException("BookingId має бути > 0");
        if (listingId <= 0) throw new DomainException("ListingId має бути > 0");

        AuthorId = authorId;
        BookingId = bookingId;
        ListingId = listingId;
        Stars = rating;
        Title = title;
        Text = text;
        IsVisible = true;
        if (photos != null) _photos.AddRange(photos.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

    public static Review Create(int authorId, int bookingId, int listingId, global::FindFi.CL.Domain.ValueObjects.Rating rating,
        global::FindFi.CL.Domain.ValueObjects.Title? title = null,
        global::FindFi.CL.Domain.ValueObjects.TextContent? text = null,
        IEnumerable<string>? photos = null)
        => new(authorId, bookingId, listingId, rating, title, text, photos);

    public void UpdateTitle(global::FindFi.CL.Domain.ValueObjects.Title? title)
    {
        Title = title;
        Touch();
    }

    public void UpdateText(global::FindFi.CL.Domain.ValueObjects.TextContent? text)
    {
        Text = text;
        Touch();
    }

    public void SetVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        Touch();
    }

    public void AddPhoto(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new DomainException("Некоректна адреса фото");
        _photos.Add(url);
        Touch();
    }

    public void RemovePhoto(string url)
    {
        _photos.Remove(url);
        Touch();
    }
}
