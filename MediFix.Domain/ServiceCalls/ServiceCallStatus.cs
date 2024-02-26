namespace MediFix.Domain.ServiceCalls;

public enum ServiceCallStatus : byte
{
    New = 1,
    AssignedToTechnician,
    TechnicianStarted,
    Finished,
    Cancelled
}