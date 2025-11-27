namespace WebAPI.DTOs.Reviews;

public sealed class ReviewDto
{
    public string Id { get; init; } = string.Empty;
    public int AuthorId { get; init; }
    public int BookingId { get; init; }
    public int ListingId { get; init; }
    public bool IsVisible { get; init; }
    public string? Title { get; init; }
    public string? Text { get; init; }
    public int Rating { get; init; }
    public IReadOnlyList<string> Photos { get; init; } = Array.Empty<string>();
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
