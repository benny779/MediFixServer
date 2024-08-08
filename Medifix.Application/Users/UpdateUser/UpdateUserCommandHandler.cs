using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.Entities;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IApplicationUserRepository applicationUserRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await applicationUserRepository
            .GetQueryable()
            .Where(user => user.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Error.EntityNotFound<ApplicationUser>(request.Id);
        }

        UpdateUser(user, request);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static void UpdateUser(ApplicationUser user, UpdateUserCommand request)
    {
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
    }
}