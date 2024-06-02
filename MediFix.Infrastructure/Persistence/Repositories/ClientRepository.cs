using MediFix.Application.Users;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Abstractions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ClientRepository(ApplicationDbContext dbContext) 
    : Repository<Client, ClientId>(dbContext)
        , IClientRepository;