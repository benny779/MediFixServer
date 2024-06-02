using MediFix.Application.Abstractions.Messaging;
using System.ComponentModel.DataAnnotations;

namespace MediFix.Application.Users.Login;

public record LoginRequest(
    [Required]
    [EmailAddress]
    string Email,
    [Required]
    string Password) : ICommand<LoginResponse>;