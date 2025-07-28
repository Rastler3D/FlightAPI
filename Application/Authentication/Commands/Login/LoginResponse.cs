namespace Application.Authentication.Commands.Login;

public sealed record LoginResponse(
    string Token,
    string Username,
    string Role);
