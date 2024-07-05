using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Persistence.ResetDb;

public record ResetDbCommand : ICommand;

internal sealed class ResetDbCommandHandler(
    IPersistenceService persistenceService) 
    : ICommandHandler<ResetDbCommand>
{
    public async Task<Result> Handle(ResetDbCommand request, CancellationToken cancellationToken)
    {
        return await persistenceService.ResetDb();
    }
}