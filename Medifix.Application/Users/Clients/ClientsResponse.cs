using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Clients;

public record ClientsResponse(IEnumerable<ClientResponse> Items) : IListResponse<ClientResponse>;