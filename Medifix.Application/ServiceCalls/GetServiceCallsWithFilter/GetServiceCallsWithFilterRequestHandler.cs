using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.ServiceCalls.GetServiceCallsWithFilter;

internal sealed class GetServiceCallsWithFilterRequestHandler(
    IServiceCallRepository serviceCallRepository)
    : IQueryHandler<GetServiceCallsWithFilterRequest, ServiceCallsResponse>
{
    public async Task<Result<ServiceCallsResponse>> Handle(
        GetServiceCallsWithFilterRequest request,
        CancellationToken cancellationToken)
    {
        var serviceCalls = serviceCallRepository
            .GetQueryable()
            .AsNoTracking();

        bool isClient = request.ClientId is not null;
        bool isPractitioner = request.PractitionerId is not null;

        serviceCalls = FilterServiceCalls(serviceCalls, request, isClient, isPractitioner);

        var serviceCallsResponse = await serviceCallRepository
            .ToResponse(serviceCalls)
            .ToListAsync(cancellationToken);

        if (serviceCallsResponse.Count == 0)
        {
            return NotFound(isClient, isPractitioner);
        }

        var orderedServiceCallsResponse = OrderServiceCallsResponse(serviceCallsResponse, isClient, isPractitioner);

        return new ServiceCallsResponse(orderedServiceCallsResponse);
    }

    private static Error NotFound(bool isClient, bool isPractitioner)
    {
        return Error.NotFound(
            "ServiceCalls.NotFound",
            isClient
                ? "No service calls found for this client."
                : isPractitioner
                    ? "No service calls found for this practitioner."
                    : "No service calls with the specified filter were found.");
    }

    private static IEnumerable<ServiceCallResponse> OrderServiceCallsResponse(
        IEnumerable<ServiceCallResponse> serviceCallsResponse,
        bool isClient, 
        bool isPractitioner)
    {
        return isClient
            ? serviceCallsResponse
                .OrderBy(sc => (byte)sc.CurrentStatus.Status)
                .ThenByDescending(sc => sc.DateCreated)
            : isPractitioner
                ? serviceCallsResponse
                    .OrderBy(sc => (byte)sc.CurrentStatus.Status)
                    .ThenBy(sc => sc.DateCreated)
                : serviceCallsResponse;
    }

    private static IQueryable<ServiceCall> FilterServiceCalls(
        IQueryable<ServiceCall> serviceCalls,
        GetServiceCallsWithFilterRequest request,
        bool isClient,
        bool isPractitioner)
    {
        if (isClient)
        {
            var clientId = ClientId.From(request.ClientId!.Value);
            serviceCalls = serviceCalls
                .Where(sc => sc.ClientId == clientId);
        }

        if (isPractitioner)
        {
            var practitionerId = PractitionerId.From(request.PractitionerId!.Value);
            serviceCalls = serviceCalls
                .Include(sc => sc.StatusHistory)
                .Where(sc => sc.PractitionerId == practitionerId);
        }

        return serviceCalls;
    }
}