using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.GetServiceCall;

internal sealed class GetServiceCallRequestHandler(
    IServiceCallRepository serviceCallRepository)
    : IQueryHandler<GetServiceCallRequest, GetServiceCallResponse>
{
    public async Task<Result<GetServiceCallResponse>> Handle(GetServiceCallRequest request, CancellationToken cancellationToken)
    {
        var serviceCall = await serviceCallRepository.GetByIdAsync(
            ServiceCallId.From(request.Id),
            cancellationToken);

        if (serviceCall.IsFailure)
        {
            return serviceCall.Error;
        }

        return new GetServiceCallResponse(serviceCall.Value!);
    }
}