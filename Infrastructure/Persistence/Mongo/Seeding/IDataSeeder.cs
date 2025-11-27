using System.Threading;
using System.Threading.Tasks;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Seeding;

public interface IDataSeeder
{
    string Name { get; }
    Task SeedAsync(CancellationToken ct = default);
}
