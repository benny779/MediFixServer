using MediFix.Application.ServiceCalls;
using MediFix.Domain.ServiceCalls;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ServiceCallRepository(ApplicationDbContext dbContext) 
    : Repository<ServiceCall, ServiceCallId>(dbContext)
        , IServiceCallRepository
{
   
}
