using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthenticationService authenticationService,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly ILogger<LoginCommandHandler> _logger = logger;

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var authResult = await authenticationService.AuthenticateAsync(
            request.Username, 
            request.Password, 
            cancellationToken);

        if (authResult.IsFailure)
        {
            return Result.Failure<LoginResponse>(authResult.Error);
        }

        var result = authResult.Value;
        return Result.Success(new LoginResponse(result.Token, result.Username, result.Role));
    }
}
