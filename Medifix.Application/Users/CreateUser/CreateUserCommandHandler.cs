using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Extensions;
using MediFix.Application.Users.Entities;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IApplicationUserService applicationUserService,
    IClientRepository clientRepository,
    IManagerRepository managerRepository,
    IPractitionerRepository practitionerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await applicationUserService.FindByEmailAsync(request.Email) is not null)
        {
            return UserErrors.AlreadyExists(request.Email);
        }

        if (!request.Password.Equals(request.ConfirmPassword))
        {
            return UserErrors.PasswordsDoNotMatch;
        }

        var user = request.ToApplicationUser();

        using var transaction = await unitOfWork.BeginTransactionAsync();

        var result = await applicationUserService.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return result.Errors.ToValidationError();
        }

        AddDomainUser(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return new CreateUserResponse(user.Id);
    }

    private void AddDomainUser(ApplicationUser user)
    {
        switch (user.Type)
        {
            case UserType.Client:
                var client = Client.Create(ClientId.From(user.Id)).Value;
                clientRepository.Insert(client);
                break;
            case UserType.Manager:
                var manager = Manager.Create(ManagerId.From(user.Id)).Value;
                managerRepository.Insert(manager);
                break;
            case UserType.Practitioner:
                var practitioner = Practitioner.Create(PractitionerId.From(user.Id)).Value;
                practitionerRepository.Insert(practitioner);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}