using Application.Common.Mappings;
using Application.Flights.Queries.GetFlights;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Flights.Commands.CreateFlight;

public sealed class CreateFlightCommandHandler(
    IFlightRepository flightRepository,
    ILogger<CreateFlightCommandHandler> logger)
    : IRequestHandler<CreateFlightCommand, Result<FlightResponse>>
{
    public async Task<Result<FlightResponse>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = Flight.Create(
            request.Origin,
            request.Destination,
            request.Departure,
            request.Arrival,
            request.Status);
        
        var result = await flightRepository.AddAsync(flight, cancellationToken);
        
        if (result.IsFailure)
        {
            logger.LogError("Failed to create flight: {Error}", result.Error);
            return Result.Failure<FlightResponse>(result.Error);
        }
        
        
        logger.LogInformation("Created flight {FlightId} from {Origin} to {Destination}", 
            result.Value.Id, result.Value.Origin, result.Value.Destination);
        
        return Result.Success(result.Value.ToResponse());
    }
}
