using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.AssignToPractitioner;

internal sealed class AssignToPractitionerCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AssignToPractitionerCommand>
{
    public async Task<Result> Handle(AssignToPractitionerCommand request, CancellationToken cancellationToken)
    {
        var serviceCallResult = await serviceCallRepository
            .GetByIdAsync(request.ServiceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value!;

        // TODO: Get current user
        var currentUserId = Guid.NewGuid();

        var assignResult = serviceCall.AssignToPractitioner(currentUserId, request.PractitionerId);

        if (assignResult.IsFailure)
        {
            return assignResult.Error;
        }

        serviceCallRepository.Update(serviceCall);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}