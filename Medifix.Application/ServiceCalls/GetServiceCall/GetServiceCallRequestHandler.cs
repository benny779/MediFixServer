using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.ServiceCalls.GetServiceCall;

internal sealed class GetServiceCallRequestHandler(
    IServiceCallRepository serviceCallRepository)
    : IQueryHandler<GetServiceCallRequest, GetServiceCallResponse>
{
    public async Task<Result<GetServiceCallResponse>> Handle(GetServiceCallRequest request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.Id);

        var serviceCall = await serviceCallRepository
            .ToResponse(sc => sc.Id == serviceCallId)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);


        if (serviceCall is null)
        {
            return Error.EntityNotFound<ServiceCall>();
        }

        return new GetServiceCallResponse(serviceCall);
    }
}