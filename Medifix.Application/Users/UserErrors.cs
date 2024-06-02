﻿using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users;

public static class UserErrors
{
    public static Error PasswordsDoNotMatch
        => Error.Validation("Password.DoNotMatch", "The passwords do not match.");

    public static Error LockedOut(string email)
        => Error.Validation("User.LockedOut", $"The user '{email}' is locked out.");

    public static Error AlreadyExists(string email)
        => Error.Validation("User.AlreadyExists", $"The user '{email}' already exists.");
}
