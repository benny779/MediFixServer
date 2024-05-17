using MediFix.Domain.Users;

namespace MediFix.Domain.ServiceCalls;

public record ServiceCallStatusUpdate(
    ServiceCallId ServiceCallId,
    ServiceCallStatus Status,
    DateTime DateTime,
    UserId UpdatedBy,
    PractitionerId? PractitionerId = null)
{
    public int Id { get; set; }
}