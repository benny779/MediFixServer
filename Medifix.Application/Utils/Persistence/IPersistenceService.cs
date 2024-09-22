using MediFix.Application.Utils.Persistence.Seed;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Persistence;

public interface IPersistenceService
{
    Task<Result> ResetDb();
    Task<Result> SeedData(SeedDataCommand command);
}
