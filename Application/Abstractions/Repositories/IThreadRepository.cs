using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IThreadRepository
{
    Task<DiscussionThread?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(DiscussionThread thread, CancellationToken cancellationToken = default);
}
