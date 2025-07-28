using Domain.Common;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Flights.Commands.UpdateFlightStatus;

public sealed class UpdateFlightStatusCommandHandler(
    IFlightRepository flightRepository,
    ILogger<UpdateFlightStatusCommandHandler> logger)
    : IRequestHandler<UpdateFlightStatusCommand, Result>
{

    public async Task<Result> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var flightResult = await flightRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (flightResult.IsFailure)
        {
            logger.LogError("Failed to retrieve flight {FlightId}: {Error}", request.Id, flightResult.Error);
            return Result.Failure(flightResult.Error);
        }

        var flight = flightResult.Value;
        if (flight == null)
        {
            logger.LogWarning("Flight {FlightId} not found", request.Id);
            return Result.Failure("Flight not found");
        }

        flight.UpdateStatus(request.Status);
        
        var updateResult = await flightRepository.UpdateAsync(flight, cancellationToken);
        
        if (updateResult.IsFailure)
        {
            logger.LogError("Failed to update flight {FlightId}: {Error}", request.Id, updateResult.Error);
            return updateResult;
        }
        
        logger.LogInformation("Updated flight {FlightId} status to {Status}", request.Id, request.Status);
        
        return Result.Success();
    }
}
