﻿namespace MediFix.Application.Users.Login;

public record LoginResponse(
    string AccessToken,
    string RefreshToken);