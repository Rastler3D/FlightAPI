using Domain.Common;
using Domain.Enums;

namespace Domain.Events;

public sealed record FlightStatusUpdatedEvent(
    int FlightId,
    FlightStatus OldStatus,
    FlightStatus NewStatus,
    string Origin,
    string Destination) : DomainEvent;
