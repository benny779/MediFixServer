using MediatR;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Messaging;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
{
}
