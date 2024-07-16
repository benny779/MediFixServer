using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CancelServiceCall;

internal sealed class CancelServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelServiceCallCommand>
{
    public async Task<Result> Handle(CancelServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.Id);

        var serviceCallResult = await serviceCallRepository
            .GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        // TODO: Get current user
        var updateUserId = Guid.NewGuid();
        
        var cancelResult = serviceCall.Cancel(updateUserId);

        if (cancelResult.IsFailure)
        {
            return cancelResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}