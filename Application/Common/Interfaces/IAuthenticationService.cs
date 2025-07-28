using System.Security.Claims;
using Application.Common.Models;
using Domain.Common;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthenticationResult>> AuthenticateAsync(
        string username, 
        string password, 
        CancellationToken cancellationToken = default);
    
    Task<Result<User?>> GetUserFromClaimsAsync(
        ClaimsPrincipal principal, 
        CancellationToken cancellationToken = default);
    
    bool IsInRole(ClaimsPrincipal principal, string role);
}
