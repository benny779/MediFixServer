using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public sealed class CreateServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository)
    : ICommandHandler<CreateServiceCallCommand>
{
    public async Task<Result> Handle(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallResult = ServiceCall.Create(
            request.UserId,
            request.LocationId,
            request.ServiceCallType,
            request.SubCategoryId,
            request.Details,
            request.Priority);

        if (serviceCallResult.IsFailure)
            return serviceCallResult.Error;

        var addServiceCallResult = await serviceCallRepository.Add(serviceCallResult.Value!, cancellationToken);

        return addServiceCallResult;
    }
}