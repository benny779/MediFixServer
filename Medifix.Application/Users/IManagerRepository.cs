using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Users;

namespace MediFix.Application.Users;

public interface IManagerRepository : IRepository<Manager, ManagerId>;