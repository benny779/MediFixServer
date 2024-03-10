namespace MediFix.Domain.ServiceCalls;

public enum ServiceCallStatus : byte
{
    New = 1,
    AssignedToPractitioner,
    Started,
    Finished,
    Cancelled
}