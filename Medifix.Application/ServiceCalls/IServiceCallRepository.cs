using MediFix.Application.Abstractions.Data;
using MediFix.Domain.ServiceCalls;
using System.Linq.Expressions;

namespace MediFix.Application.ServiceCalls;

public interface IServiceCallRepository : IRepository<ServiceCall, ServiceCallId>
{
    public IQueryable<ServiceCallResponse> ToResponse(Expression<Func<ServiceCall, bool>> predicate);
    public IQueryable<ServiceCallResponse> ToResponse(IQueryable<ServiceCall> serviceCalls);
}