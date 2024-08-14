using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Application.ServiceCalls;

public record ServiceCallsResponse(
    IEnumerable<ServiceCallResponse> Items) : IListResponse<ServiceCallResponse>;

public record ServiceCallResponse(
    Guid Id,
    ServiceCallClient Client,
    ServiceCallLocations Location,
    ServiceCallType Type,
    ServiceCallCategory Category,
    ServiceCallSubCategory SubCategory,
    string Details,
    DateTime DateCreated,
    ServiceCallPriority Priority,
    IList<ServiceCallStatusUpdate> StatusUpdates,
    ServiceCallStatusUpdate CurrentStatus,
    string? CloseDetails,
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
    string FullName,
    string? PhoneNumber);

public record ServiceCallPractitioner(
    Guid Id,
    string FullName,
    string? PhoneNumber);