using Application.Common.Abstractions.Messaging;
using Application.Common.Security;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;

namespace Application.Flights.Commands.UpdateFlightStatus;

[Authorize(Roles = Roles.Moderator)]
public sealed record UpdateFlightStatusCommand(
    int Id,
    FlightStatus Status) : ICommand
{
}
