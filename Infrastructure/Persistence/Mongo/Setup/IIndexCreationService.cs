using System.Threading;
using System.Threading.Tasks;

namespace FindFi.CL.Infrastructure.Persistence.Mongo.Setup;

public interface IIndexCreationService
{
    Task CreateIndexesAsync(CancellationToken ct = default);
}
