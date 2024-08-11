using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.CreateUser;

public record CreateUserResponse(Guid Id) : ICreatedResponse;