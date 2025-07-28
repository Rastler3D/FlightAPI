using Domain.Enums;

namespace Application.Flights.Queries.GetFlights;

public sealed record FlightResponse(
    int Id,
    string Origin,
    string Destination,
    DateTimeOffset Departure,
    DateTimeOffset Arrival,
    FlightStatus Status);
