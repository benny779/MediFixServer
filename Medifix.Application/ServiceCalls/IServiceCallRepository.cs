using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls;

public interface IServiceCallRepository
{
    Task<Result> Add(ServiceCall serviceCall, CancellationToken cancellationToken);

    Task<Result<ServiceCall>> GetById(ServiceCallId serviceCallId, CancellationToken cancellationToken);

    Task<Result> SetStatus(
        ServiceCallId serviceCallId,
        //ServiceCallStatusUpdate serviceCallStatusUpdate,
        CancellationToken cancellationToken);
}