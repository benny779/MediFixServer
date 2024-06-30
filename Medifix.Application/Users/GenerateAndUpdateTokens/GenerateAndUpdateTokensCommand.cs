using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.Entities;

namespace MediFix.Application.Users.GenerateAndUpdateTokens;

internal record GenerateAndUpdateTokensCommand(
    ApplicationUser User)
    : ICommand<TokensResponse>;