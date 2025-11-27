namespace FindFi.CL.Domain.Abstractions.Services;

/// <summary>
/// Доменний сервіс для перерахунку агрегованих рейтингів лістингів
/// з урахуванням оптимістичної конкурентності.
/// </summary>
public interface IListingRatingCalculator
{
    /// <summary>
    /// Повний перерахунок середнього рейтингу за лістингом (агрегація по відгуках).
    /// </summary>
    Task<Entities.ListingRating> RecalculateAsync(int listingId, CancellationToken ct = default);

    /// <summary>
    /// Спроба застосувати новий рейтинг без повної агрегації (on-the-fly).
    /// Повертає оновлену сутність або викидає виключення у випадку конфлікту версій.
    /// </summary>
    Task<Entities.ListingRating> ApplyNewReviewAsync(int listingId, int rating, long expectedVersion, CancellationToken ct = default);
}
