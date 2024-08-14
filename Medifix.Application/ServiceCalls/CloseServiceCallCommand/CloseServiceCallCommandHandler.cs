using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CloseServiceCallCommand;

internal sealed class CloseServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : ICommandHandler<CloseServiceCallCommand>
{
    public async Task<Result> Handle(CloseServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.ServiceCallId);

        var serviceCallResult = await serviceCallRepository.GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        var startResult = serviceCall.Finish(currentUser.Id, request.CloseDetails);

        if (startResult.IsFailure)
        {
            return startResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}