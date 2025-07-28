using Domain.Common;

namespace Domain.Events;

public sealed record FlightUpdatedEvent(
    int FlightId,
    string Origin,
    string Destination) : DomainEvent;
