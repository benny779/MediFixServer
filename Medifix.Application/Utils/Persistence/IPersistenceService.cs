using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Persistence;

public interface IPersistenceService
{
    Task<Result> ResetDb();
    Task<Result> SeedData();
}
