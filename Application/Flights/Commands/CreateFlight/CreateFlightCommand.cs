using Application.Common.Abstractions.Messaging;
using Application.Common.Security;
using Application.Flights.Queries.GetFlights;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;

namespace Application.Flights.Commands.CreateFlight;


[Authorize(Roles = Roles.Moderator)]
public sealed record CreateFlightCommand(
    string Origin,
    string Destination,
    DateTimeOffset Departure,
    DateTimeOffset Arrival,
    FlightStatus Status = FlightStatus.InTime) : ICommand<FlightResponse>;
