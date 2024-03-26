using MediFix.Domain.Practitioners;

namespace MediFix.Domain.ServiceCalls;

public record ServiceCallStatusUpdate(
    ServiceCallId ServiceCallId,
    ServiceCallStatus Status,
    DateTime DateTime,
    PractitionerId? PractitionerId = null)
{
    public int Id { get; set; }
}