using FindFi.CL.Domain.Common;
using FindFi.CL.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace FindFi.CL.Domain.Entities;

public sealed class DiscussionThread : BaseEntity
{
    [BsonElement("createdBy")]
    public int CreatedBy { get; private set; }
    [BsonElement("listingId")]
    public int ListingId { get; private set; }

    [BsonElement("title")]
    [BsonSerializer(typeof(Title.BsonSerializer))]
    public Title Title { get; private set; } = Title.Create("Untitled");

    [BsonElement("isLocked")]
    public bool IsLocked { get; private set; }

    [BsonElement("messages")]
    private readonly List<ThreadMessage> _messages = new();
    public IReadOnlyList<ThreadMessage> Messages => _messages;

    private DiscussionThread() { }

    private DiscussionThread(int createdBy, int listingId, Title title)
    {
        if (createdBy <= 0) throw new DomainException("CreatedBy має бути > 0");
        if (listingId <= 0) throw new DomainException("ListingId має бути > 0");
        CreatedBy = createdBy;
        ListingId = listingId;
        Title = title;
        IsLocked = false;
    }

    public static DiscussionThread Create(int createdBy, int listingId, Title title)
        => new(createdBy, listingId, title);

    public ThreadMessage AddMessage(int authorId, TextContent text)
    {
        if (IsLocked) throw new DomainException("Неможливо додати повідомлення: тред зачинений");
        var msg = ThreadMessage.Create(authorId, text);
        _messages.Add(msg);
        Touch();
        return msg;
    }

    public void EditMessage(ObjectId messageId, int editorId, TextContent newText)
    {
        var msg = _messages.FirstOrDefault(m => m.Id == messageId)
                  ?? throw new DomainException("Повідомлення не знайдено");
        if (msg.AuthorId != editorId) throw new DomainException("Редагувати може лише автор повідомлення");
        msg.Edit(newText);
        Touch();
    }

    public void Lock() { IsLocked = true; Touch(); }
    public void Unlock() { IsLocked = false; Touch(); }
}

public sealed class ThreadMessage
{
    [BsonId]
    public ObjectId Id { get; private set; }
    [BsonElement("authorId")]
    public int AuthorId { get; private set; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; private set; }

    // У джерелі зберігається як string
    [BsonElement("editedAt")]
    public string? EditedAt { get; private set; }

    [BsonElement("text")]
    [BsonSerializer(typeof(TextContent.BsonSerializer))]
    public TextContent Text { get; private set; } = TextContent.Create("...");

    private ThreadMessage() { }

    private ThreadMessage(int authorId, TextContent text)
    {
        if (authorId <= 0) throw new DomainException("AuthorId має бути > 0");
        Id = ObjectId.GenerateNewId();
        AuthorId = authorId;
        CreatedAt = DateTime.UtcNow;
        Text = text;
    }

    internal static ThreadMessage Create(int authorId, TextContent text) => new(authorId, text);

    internal void Edit(TextContent newText)
    {
        Text = newText;
        EditedAt = DateTime.UtcNow.ToString("O");
    }
}
