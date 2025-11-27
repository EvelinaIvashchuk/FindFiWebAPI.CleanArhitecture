using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IReactionRepository
{
    Task<Reaction?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Reaction reaction, CancellationToken cancellationToken = default);
}
