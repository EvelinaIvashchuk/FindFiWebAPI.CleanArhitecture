using FindFi.CL.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FindFi.CL.Domain.Entities;

public sealed class Report : BaseEntity
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
        { "open", "in_review", "resolved", "rejected" };

    [BsonElement("comment")]
    public string Comment { get; private set; } = string.Empty;
    [BsonElement("reasonCode")]
    public string ReasonCode { get; private set; } = string.Empty;
    [BsonElement("status")]
    public string Status { get; private set; } = "open";
    [BsonElement("targetType")]
    public string TargetType { get; private set; } = string.Empty;

    [BsonElement("targetId")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public ObjectId TargetId { get; private set; }

    [BsonElement("reporterId")]
    public int ReporterId { get; private set; }

    private Report() { }

    private Report(string comment, string reasonCode, string targetType, ObjectId targetId, int reporterId)
    {
        if (string.IsNullOrWhiteSpace(comment)) throw new DomainException("Comment не може бути порожнім");
        if (string.IsNullOrWhiteSpace(reasonCode)) throw new DomainException("ReasonCode не може бути порожнім");
        if (string.IsNullOrWhiteSpace(targetType)) throw new DomainException("TargetType не може бути порожнім");
        if (reporterId <= 0) throw new DomainException("ReporterId має бути > 0");
        Comment = comment.Trim();
        ReasonCode = reasonCode.Trim();
        TargetType = targetType.Trim().ToLowerInvariant();
        TargetId = targetId;
        ReporterId = reporterId;
        Status = "open";
    }

    public static Report Create(string comment, string reasonCode, string targetType, ObjectId targetId, int reporterId)
        => new(comment, reasonCode, targetType, targetId, reporterId);

    public void ChangeStatus(string newStatus)
    {
        if (!AllowedStatuses.Contains(newStatus)) throw new DomainException("Некоректний статус скарги");
        Status = newStatus.ToLowerInvariant();
        Touch();
    }
}
