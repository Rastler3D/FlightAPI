using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public sealed class FlightRepository(ApplicationDbContext context, ILogger<FlightRepository> logger)
    : IFlightRepository
{
    public async Task<Result<IEnumerable<Flight>>> GetAllAsync(
        string? origin = null,
        string? destination = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(origin))
                query = query.Where(f => f.Origin.Contains(origin));

            if (!string.IsNullOrEmpty(destination))
                query = query.Where(f => f.Destination.Contains(destination));

            var flights = await query.ToListAsync(cancellationToken);
            
            return Result.Success<IEnumerable<Flight>>(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving flights");
            return Result.Failure<IEnumerable<Flight>>("Failed to retrieve flights");
        }
    }

    public async Task<Result<Flight?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var flight = await context.Flights.FindAsync([id], cancellationToken);
            return Result.Success(flight);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving flight {FlightId}", id);
            return Result.Failure<Flight?>("Failed to retrieve flight");
        }
    }

    public async Task<Result<Flight>> AddAsync(Flight flight, CancellationToken cancellationToken = default)
    {
        try
        {
            context.Flights.Add(flight);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success(flight);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding flight");
            return Result.Failure<Flight>("Failed to add flight");
        }
    }

    public async Task<Result> UpdateAsync(Flight flight, CancellationToken cancellationToken = default)
    {
        try
        {
            context.Flights.Update(flight);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating flight {FlightId}", flight.Id);
            return Result.Failure("Failed to update flight");
        }
    }
}
