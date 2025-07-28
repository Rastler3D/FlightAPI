using Domain.Common;

namespace Domain.Events;

public sealed record FlightCreatedEvent(
    int FlightId,
    string Origin,
    string Destination) : DomainEvent;
