﻿using MediatR;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}