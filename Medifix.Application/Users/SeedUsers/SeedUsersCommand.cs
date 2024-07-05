using MediatR;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.CreateUser;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.SeedUsers;

public record SeedUsersCommand : ICommand;

internal sealed class SeedUsersCommandHandler(
    IUnitOfWork unitOfWork,
    IApplicationUserService applicationUserService,
    ISender sender)
    : ICommandHandler<SeedUsersCommand>
{
    private const string DefaultPassword = "Aq123456";

    public async Task<Result> Handle(SeedUsersCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync();

        var createUserCommands = GetCreateUserCommands();

        var createUserCommandTasks = createUserCommands
            .Select(c => sender.Send(c, cancellationToken));

        var createUserResults = await Task.WhenAll(createUserCommandTasks);

        if (createUserResults.HasFailure())
        {
            transaction.Rollback();

            return ValidationError.FromResults(createUserResults);
        }

        transaction.Commit();

        return Result.Success();
    }

    private static IEnumerable<CreateUserCommand> GetCreateUserCommands()
    {
        return [
            new CreateUserCommand(
                UserType.Client,
                "Client",
                "Client",
                "client@medifix.com",
                DefaultPassword, DefaultPassword),
            new CreateUserCommand(
                UserType.Manager,
                "Manager",
                "Manager",
                "manager@medifix.com",
                DefaultPassword, DefaultPassword),
            new CreateUserCommand(
                UserType.Practitioner,
                "Practitioner",
                "Practitioner",
                "practitioner@medifix.com",
                DefaultPassword, DefaultPassword)
        ];
    }
}