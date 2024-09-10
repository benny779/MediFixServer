using MediFix.Application.Expertises;
using MediFix.Application.Users;
using MediFix.Application.Users.Clients;
using MediFix.Application.Users.Practitioners;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ClientRepository(ApplicationDbContext dbContext) 
    : Repository<Client, ClientId>(dbContext)
        , IClientRepository
{
    public override IQueryable<Client> GetQueryableWithNavigation()
    {
        return GetQueryable();
    }

    public IQueryable<ClientResponse> GetResponseQueryable()
    {
        return GetResponseQueryable(GetQueryableWithNavigation());
    }

    public IQueryable<ClientResponse> GetResponseQueryable(IQueryable<Client> queryable)
    {
        return queryable
            .AsNoTracking()
            .Join(dbContext.Users,
                practitioner => practitioner.Id,
                appUser => appUser.Id,
                (practitioner, appUser) => new ClientResponse(
                    practitioner.Id,
                    appUser.FirstName,
                    appUser.LastName,
                    appUser.FullName,
                    appUser.Email,
                    appUser.PhoneNumber)
            );
    }
}