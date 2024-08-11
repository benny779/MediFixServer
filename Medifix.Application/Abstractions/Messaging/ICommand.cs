using MediatR;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IValidatable;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IValidatable;

public interface ICreateCommand<TResponse> : ICommand<TResponse>
    where TResponse : ICreatedResponse;