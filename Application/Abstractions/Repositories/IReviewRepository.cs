using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IReviewRepository
{
    Task<int> GetReviewsCount(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetAllAsync(int skip = 0, int take = 20, CancellationToken cancellationToken = default);
    Task<Review?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
