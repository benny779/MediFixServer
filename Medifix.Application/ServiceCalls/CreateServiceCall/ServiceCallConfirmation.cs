namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public record ServiceCallConfirmation(
    string FullName,
    string Location,
    string Type,
    string Category,
    string Subcategory,
    string Priority,
    string Details);