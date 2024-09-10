using MediFix.Application.Abstractions.Data;
using MediFix.Application.Users.Clients;
using MediFix.Domain.Users;

namespace MediFix.Application.Users;

public interface IClientRepository : IRepository<Client, ClientId>
{
    IQueryable<ClientResponse> GetResponseQueryable();

    IQueryable<ClientResponse> GetResponseQueryable(IQueryable<Client> queryable);
}