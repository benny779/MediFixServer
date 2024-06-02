using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Users;

namespace MediFix.Application.Users;

public interface IClientRepository : IRepository<Client, ClientId>;