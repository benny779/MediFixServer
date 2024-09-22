using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Persistence.Seed;

internal sealed class SeedDataCommandHandler(
    IPersistenceService persistenceService)
    : ICommandHandler<SeedDataCommand>
{
    public Task<Result> Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        return persistenceService.SeedData(request);
    }
}