using MediFix.Application.Abstractions.Data;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Application.ServiceCalls;

public interface IServiceCallRepository : IRepository<ServiceCall, ServiceCallId>;