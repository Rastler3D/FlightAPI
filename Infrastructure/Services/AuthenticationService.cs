using System.Security.Claims;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Services;

public sealed class AuthenticationService(
    IUserRepository userRepository,
    IPasswordService passwordService,
    ITokenService tokenService,
    ILogger<AuthenticationService> logger)
    : IAuthenticationService
{
    public async Task<Result<AuthenticationResult>> AuthenticateAsync(
        string username, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Authentication attempt with empty credentials");
            return Result.Failure<AuthenticationResult>("Invalid credentials");
        }

        var userResult = await userRepository.GetByUsernameAsync(username, cancellationToken);
        
        if (userResult.IsFailure)
        {
            logger.LogError("Failed to retrieve user {Username}: {Error}", username, userResult.Error);
            return Result.Failure<AuthenticationResult>("Authentication failed");
        }

        var user = userResult.Value;
        if (user == null)
        {
            logger.LogWarning("Authentication failed - user {Username} not found", username);
            return Result.Failure<AuthenticationResult>("Invalid credentials");
        }

        if (!passwordService.VerifyPassword(password, user.PasswordHash))
        {
            logger.LogWarning("Authentication failed - invalid password for user {Username}", username);
            return Result.Failure<AuthenticationResult>("Invalid credentials");
        }

        var token = tokenService.GenerateToken(user);
        
        logger.LogInformation("User {Username} successfully authenticated", username);
        
        return Result.Success(new AuthenticationResult(
            Token: token,
            Username: user.Username,
            Role: user.Role.Code,
            UserId: user.Id));
    }

    public async Task<Result<User?>> GetUserFromClaimsAsync(
        ClaimsPrincipal principal, 
        CancellationToken cancellationToken = default)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Result.Success<User?>(null);
        }

        return await userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public bool IsInRole(ClaimsPrincipal principal, string role)
    {
        return principal.IsInRole(role);
    }
}


