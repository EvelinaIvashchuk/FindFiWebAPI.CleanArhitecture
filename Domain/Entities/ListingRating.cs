using FindFi.CL.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace FindFi.CL.Domain.Entities;

public sealed class ListingRating : BaseEntity
{
    [BsonElement("listingId")]
    public int ListingId { get; private set; }

    [BsonElement("avgRating")]
    public int AvgRating { get; private set; }

    [BsonElement("reviews_coreCount")]
    public int Reviews_CoreCount { get; private set; }

    private ListingRating() { }

    private ListingRating(int listingId, int avgRating, int reviewsCoreCount)
    {
        if (listingId <= 0) throw new DomainException("ListingId має бути > 0");
        if (avgRating is < 0 or > 5) throw new DomainException("AvgRating 0..5");
        if (reviewsCoreCount < 0) throw new DomainException("Reviews_CoreCount не може бути від'ємним");
        ListingId = listingId;
        AvgRating = avgRating;
        Reviews_CoreCount = reviewsCoreCount;
    }

    public static ListingRating CreateNew(int listingId)
        => new(listingId, 0, 0);

    public void ApplyNewReview(int rating)
    {
        if (rating is < 1 or > 5) throw new DomainException("rating 1..5");
        var total = AvgRating * Reviews_CoreCount + rating;
        Reviews_CoreCount += 1;
        AvgRating = (int)Math.Round((double)total / Reviews_CoreCount, MidpointRounding.AwayFromZero);
        Touch();
    }

    public void RecalculateFrom(int sumRatings, int count)
    {
        if (count < 0) throw new DomainException("count < 0");
        if (sumRatings < 0) throw new DomainException("sumRatings < 0");
        Reviews_CoreCount = count;
        AvgRating = count == 0 ? 0 : (int)Math.Round((double)sumRatings / count, MidpointRounding.AwayFromZero);
        Touch();
    }
}
