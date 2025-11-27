using FindFi.CL.Domain.Entities;

namespace FindFi.CL.Application.Abstractions.Repositories;

public interface IReportRepository
{
    Task<Report?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task AddAsync(Report report, CancellationToken cancellationToken = default);
}
