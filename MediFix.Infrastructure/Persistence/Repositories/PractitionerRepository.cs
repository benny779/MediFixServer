using MediFix.Application.Users;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Abstractions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class PractitionerRepository(ApplicationDbContext dbContext)
    : Repository<Practitioner, PractitionerId>(dbContext)
        , IPractitionerRepository;