using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
