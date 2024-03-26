using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Practitioners;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.AssignToPractitioner;

public record AssignToPractitionerCommand(ServiceCallId ServiceCallId, PractitionerId PractitionerId) : ICommand;

public sealed class AssignToPractitionerCommandHandler
    (IServiceCallRepository serviceCallRepository)
    : ICommandHandler<AssignToPractitionerCommand>
{
    public async Task<Result> Handle(AssignToPractitionerCommand request, CancellationToken cancellationToken)
    {
        // Get service call
        var serviceCallResult = await serviceCallRepository.GetById(request.ServiceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
            return serviceCallResult.Error;

        // assign
        var serviceCall = serviceCallResult.Value!;

        var assignResult = serviceCall.AssignToPractitioner(request.PractitionerId);

        if (assignResult.IsFailure)
            return assignResult.Error;

        // save
        var saveResult = await serviceCallRepository.SetStatus(
            serviceCall.Id,
            //serviceCall.CurrentStatusUpdate,
            cancellationToken);

        // return
        return saveResult;
    }
}