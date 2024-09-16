namespace MediFix.Application.ServiceCalls.AssignPractitioner;

public record PractitionerAssignedEmailModel(
    string PractitionerName,
    string ClientName,
    string Location,
    string Type,
    string Category,
    string Subcategory,
    string Priority,
    DateTime CreatedDateTime,
    string Details);