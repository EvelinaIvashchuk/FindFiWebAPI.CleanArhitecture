namespace WebAPI.DTOs.Reviews;

public sealed class CreateReviewRequest
{
    public int AuthorId { get; init; }
    public int BookingId { get; init; }
    public int ListingId { get; init; }
    public int Rating { get; init; }
    public string? Title { get; init; }
    public string? Text { get; init; }
    public IReadOnlyCollection<string>? Photos { get; init; }
}
