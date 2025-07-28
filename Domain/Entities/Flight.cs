using Domain.Common;

namespace Domain.Entities;

public sealed class Flight : BaseEntity
{
    public string Origin { get; private set; } = string.Empty;
    public string Destination { get; private set; } = string.Empty;
    public DateTimeOffset Departure { get; private set; }
    public DateTimeOffset Arrival { get; private set; }
    public FlightStatus Status { get; private set; }

    private Flight() { } // EF Core

    public Flight(
        string origin,
        string destination,
        DateTimeOffset departure,
        DateTimeOffset arrival,
        FlightStatus status = FlightStatus.InTime)
    {
        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
        Status = status;
    }

    public void UpdateStatus(FlightStatus status)
    {
        Status = status;
    }

    public void UpdateDetails(
        string origin,
        string destination,
        DateTimeOffset departure,
        DateTimeOffset arrival)
    {
        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
    }
}
