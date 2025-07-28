using Application.Common.Mappings;
using Domain.Common;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Flights.Queries.GetFlights;

public sealed class GetFlightsQueryHandler(
    IFlightRepository flightRepository,
    ILogger<GetFlightsQueryHandler> logger)
    : IRequestHandler<GetFlightsQuery, Result<IEnumerable<FlightResponse>>>
{
    public async Task<Result<IEnumerable<FlightResponse>>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var flightsResult = await flightRepository.GetAllAsync(request.Origin, request.Destination, cancellationToken);
        
        if (flightsResult.IsFailure)
        {
            logger.LogError("Failed to retrieve flights: {Error}", flightsResult.Error);
            return Result.Failure<IEnumerable<FlightResponse>>(flightsResult.Error);
        }

        var flights = flightsResult.Value.OrderBy(f => f.Arrival).ToList();
        var response = flights.ToResponse();
        
        logger.LogInformation("Retrieved {Count} flights from database", flights.Count);
        
        return Result.Success(response);
    }
}
