using MediFix.Domain.Practitioners;

namespace MediFix.Domain.ServiceCalls;

public record ServiceCallStatusUpdate(ServiceCallStatus Status, DateTime DateTime, PractitionerId? PractitionerId = null);