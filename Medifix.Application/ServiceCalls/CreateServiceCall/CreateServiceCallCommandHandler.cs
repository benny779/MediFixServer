using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

internal sealed class CreateServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateServiceCallCommand, CreateServiceCallResponse>
{
    public async Task<Result<CreateServiceCallResponse>> Handle(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallResult = ServiceCall.Create(
            ClientId.From(request.ClientId),
            LocationId.From(request.LocationId),
            request.ServiceCallType,
            SubCategoryId.From(request.SubCategoryId),
            request.Details,
            request.Priority);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        serviceCallRepository.Insert(serviceCall);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateServiceCallResponse(serviceCall.Id);
    }
}