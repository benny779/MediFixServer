using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Application.ServiceCalls;

public record ServiceCallsResponse(
    IEnumerable<ServiceCallResponse> ServiceCalls);

public record ServiceCallResponse(
    Guid Id,
    ServiceCallClient Client,
    ServiceCallLocations Location,
    ServiceCallType Type,
    ServiceCallCategory Category,
    ServiceCallSubCategory SubCategory,
    string Details,
    DateTime DateCreated,
    IList<ServiceCallStatusUpdate> StatusUpdates,
    ServiceCallStatusUpdate CurrentStatus,
    ServiceCallPractitioner? Practitioner
    );

public record ServiceCallLocations(
    ServiceCallLocation Building,
    ServiceCallLocation Floor,
    ServiceCallLocation Department,
    ServiceCallLocation Room
);

public record ServiceCallLocation(
    Guid Id,
    LocationType Type,
    string Name);

public record ServiceCallSubCategory(
    Guid Id,
    string Name);

public record ServiceCallCategory(
    Guid Id,
    string Name);

public record ServiceCallClient(
    Guid Id,
    string FullName);

public record ServiceCallPractitioner(
    Guid Id,
    string FullName);