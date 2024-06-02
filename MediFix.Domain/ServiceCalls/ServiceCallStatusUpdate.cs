using MediFix.Domain.Users;

namespace MediFix.Domain.ServiceCalls;

public record ServiceCallStatusUpdate(
    ServiceCallId ServiceCallId,
    ServiceCallStatus Status,
    DateTime DateTime,
    Guid UpdatedBy,
    PractitionerId? PractitionerId = null);