using Domain.Common;

namespace Application.Authentication.Commands.Login;

public sealed record LoginCommand(
    string Username,
    string Password) : IRequest<Result<LoginResponse>>;
