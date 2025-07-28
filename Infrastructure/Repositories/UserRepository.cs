using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    : IUserRepository
{
    public async Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
                
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving user {Username}", username);
            return Result.Failure<User?>("Failed to retrieve user");
        }
    }

    public async Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
                
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving user {UserId}", id);
            return Result.Failure<User?>("Failed to retrieve user");
        }
    }

    public async Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding user");
            return Result.Failure<User>("Failed to add user");
        }
    }
}
