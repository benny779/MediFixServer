using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MediFix.Application.ServiceCalls.GetServiceCallsWithFilter;

public record GetServiceCallsWithFilterRequest(
    Guid? ClientId,
    Guid? PractitionerId)
    : IQuery<ServiceCallsResponse>;

internal sealed class GetServiceCallsWithFilterRequestHandler(
    IServiceCallRepository serviceCallRepository,
    IDbConnectionFactory dbConnectionFactory)
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

        var serviceCallsResponse = await serviceCallRepository
            .ToResponse(serviceCalls)
            .ToListAsync(cancellationToken);

        if (serviceCallsResponse.Count == 0)
        {
            return Error.NotFound(
                "ServiceCalls.NotFound",
                isClient
                    ? "No service calls found for this client."
                    : isPractitioner
                        ? "No service calls found for this practitioner."
                        : "No service calls with the specified filter were found.");
        }

        var orderedServiceCallsResponse = isClient
            ? serviceCallsResponse
                .OrderBy(sc => (byte)sc.CurrentStatus.Status)
                .ThenByDescending(sc => sc.DateCreated)
                .ToList()
            : isPractitioner
                ? serviceCallsResponse
                    .OrderBy(sc => (byte)sc.CurrentStatus.Status)
                    .ThenBy(sc => sc.DateCreated)
                    .ToList()
                : serviceCallsResponse;

        return new ServiceCallsResponse(orderedServiceCallsResponse);
    }
}