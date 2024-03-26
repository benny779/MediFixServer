using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUsersRepository usersRepository) : 
    ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create();

        var hashedPassword = request.Password;

        var createUserResult = User.Create(
            userId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            hashedPassword
        );

        if (createUserResult.IsFailure)
            return createUserResult.Error;

        var user = createUserResult.Value!;

        var result = await usersRepository.Add(user, cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return new CreateUserResponse(
            user.Id.Value.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.Phone);
    }
}