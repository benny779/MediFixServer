using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Users.Clients.GetClients;

internal sealed class GetClientsRequestHandler(
    IClientRepository clientRepository) 
    : IQueryHandler<GetClientsRequest, ClientsResponse>
{
    public async Task<Result<ClientsResponse>> Handle(GetClientsRequest request, CancellationToken cancellationToken)
    {
        var clients = await clientRepository
            .GetResponseQueryable()
            .ToListAsync(cancellationToken);

        if (clients.IsEmpty())
        {
            return Error.EntityNotFound<Client>();
        }

        return new ClientsResponse(clients);
    }
}