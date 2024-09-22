namespace MediFix.Application.ServiceCalls.CloseServiceCall;

public record ServiceCallClosedEmailModel(
    string ClientName,
    string Location,
    string Type,
    string Category,
    string Subcategory,
    string PractitionerName,
    DateTime CompletionDate,
    string CloseDetails);