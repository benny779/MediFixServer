namespace MediFix.Application.ServiceCalls.CancelServiceCall;

public record ServiceCallCancelledEmailModel(
    string ClientName,
    string Location,
    string Type,
    string Category,
    string Subcategory,
    DateTime CancellationDate);