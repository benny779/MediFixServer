using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Persistence.Seed;

internal sealed class SeedDataCommandHandler(
    IPersistenceService persistenceService) 
    : ICommandHandler<SeedDataCommand>
{
    public async Task<Result> Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        return await persistenceService.SeedData();
    }
}