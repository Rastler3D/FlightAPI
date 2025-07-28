namespace Application.Common.Models;

public sealed record AuthenticationResult(
    string Token,
    string Username,
    string Role,
    int UserId);