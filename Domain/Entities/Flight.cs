using Domain.Common;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

public sealed class Flight : Entity
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

    public static Flight Create(
        string origin,
        string destination,
        DateTimeOffset departure,
        DateTimeOffset arrival,
        FlightStatus status = FlightStatus.InTime)
    {
        var flight = new Flight(origin, destination, departure, arrival, status);
        
      
        flight.RaiseDomainEvent(new FlightCreatedEvent(flight.Id, origin, destination));
        
        return flight;
    }

    public void UpdateStatus(FlightStatus newStatus)
    {
        if (Status == newStatus)
            return;

        var oldStatus = Status;
        Status = newStatus;
        
   
        RaiseDomainEvent(new FlightStatusUpdatedEvent(Id, oldStatus, newStatus, Origin, Destination));
    }

    public void UpdateDetails(
        string origin,
        string destination,
        DateTimeOffset departure,
        DateTimeOffset arrival)
    {
        var hasChanges = Origin != origin || 
                        Destination != destination || 
                        Departure != departure || 
                        Arrival != arrival;

        if (!hasChanges)
            return;

        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
        
        
        RaiseDomainEvent(new FlightUpdatedEvent(Id, origin, destination));
    }
}
