﻿using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.StartServiceCall;

internal sealed class StartServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<StartServiceCallCommand>
{
    public async Task<Result> Handle(StartServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.ServiceCallId);

        var serviceCallResult = await serviceCallRepository.GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        // TODO: Get current user
        var currentUserId = Guid.NewGuid();
        var startResult = serviceCall.Start(currentUserId);

        if (startResult.IsFailure)
        {
            return startResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}