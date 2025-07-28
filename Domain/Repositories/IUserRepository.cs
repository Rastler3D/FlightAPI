using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<Result<User?>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Result<User?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken = default);
}
