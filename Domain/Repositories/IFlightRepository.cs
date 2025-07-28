using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories;

public interface IFlightRepository
{
    Task<Result<IEnumerable<Flight>>> GetAllAsync(
        string? origin = null,
        string? destination = null,
        CancellationToken cancellationToken = default);
    
    Task<Result<Flight?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<Result<Flight>> AddAsync(Flight flight, CancellationToken cancellationToken = default);
    
    Task<Result> UpdateAsync(Flight flight, CancellationToken cancellationToken = default);
    
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
