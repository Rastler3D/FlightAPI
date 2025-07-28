using Application.Flights.Commands.CreateFlight;
using Application.Flights.Commands.UpdateFlightStatus;
using Application.Flights.Queries.GetFlights;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class FlightsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get all flights with optional filtering
    /// </summary>
    /// <param name="origin">Filter by origin</param>
    /// <param name="destination">Filter by destination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of flights</returns>
    [HttpGet]
    public async Task<IActionResult> GetFlights(
        [FromQuery] string? origin,
        [FromQuery] string? destination,
        CancellationToken cancellationToken)
    {
        var query = new GetFlightsQuery(origin, destination);
        var result = await mediator.Send(query, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Create a new flight (Moderator only)
    /// </summary>
    /// <param name="command">Flight details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created flight</returns>
    [HttpPost]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> CreateFlight([FromBody] CreateFlightCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
            
        return CreatedAtAction(nameof(GetFlights), new { }, result.Value);
    }

    /// <summary>
    /// Update flight status (Moderator only)
    /// </summary>
    /// <param name="command">Status update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPut("status")]
    [Authorize(Roles = Roles.Moderator)]
    public async Task<IActionResult> UpdateFlightStatus([FromBody] UpdateFlightStatusCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(new { error = result.Error });
            
        return NoContent();
    }
}
