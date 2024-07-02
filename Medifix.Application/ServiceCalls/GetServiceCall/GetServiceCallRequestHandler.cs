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

        var serviceCalls = serviceCallRepository
            .GetQueryable()
            .AsNoTracking()
            .Where(sc => sc.Id == serviceCallId);

        var serviceCallResponse = await serviceCallRepository
            .ToResponse(serviceCalls)
            .SingleOrDefaultAsync(cancellationToken);


        if (serviceCallResponse is null)
        {
            return Error.EntityNotFound<ServiceCall>();
        }

        return new GetServiceCallResponse(serviceCallResponse);
    }
}