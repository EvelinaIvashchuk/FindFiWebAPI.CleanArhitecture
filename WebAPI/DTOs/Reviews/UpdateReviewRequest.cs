namespace WebAPI.DTOs.Reviews;

public sealed class UpdateReviewRequest
{
    public string? Title { get; init; }
    public string? Text { get; init; }
    public bool? IsVisible { get; init; }
    public IReadOnlyCollection<string>? AddPhotos { get; init; }
    public IReadOnlyCollection<string>? RemovePhotos { get; init; }
}
