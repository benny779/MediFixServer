namespace MediFix.Application.Users.Practitioners;

public record PractitionerWithServiceCallCount(
    Guid PractitionerId,
    string FirstName,
    string LastName,
    int AssignedServiceCalls,
    int StartedServiceCalls);