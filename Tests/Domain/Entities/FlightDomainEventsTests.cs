using Domain.Enums;
using Domain.Events;

namespace Flight.Tests.Domain.Entities;

public sealed class FlightDomainEventsTests
{
    [Fact]
    public void Create_ShouldRaiseFlightCreatedEvent()
    {
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));

       
        Assert.Single(flight.DomainEvents);
        Assert.IsType<FlightCreatedEvent>(flight.DomainEvents.First());
        
        var createdEvent = (FlightCreatedEvent)flight.DomainEvents.First();
        Assert.Equal("NYC", createdEvent.Origin);
        Assert.Equal("LAX", createdEvent.Destination);
    }

    [Fact]
    public void UpdateStatus_WithDifferentStatus_ShouldRaiseFlightStatusUpdatedEvent()
    {
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        flight.ClearDomainEvents(); // Clear creation event

        flight.UpdateStatus(FlightStatus.Delayed);

       
        Assert.Single(flight.DomainEvents);
        Assert.IsType<FlightStatusUpdatedEvent>(flight.DomainEvents.First());
        
        var statusEvent = (FlightStatusUpdatedEvent)flight.DomainEvents.First();
        Assert.Equal(FlightStatus.InTime, statusEvent.OldStatus);
        Assert.Equal(FlightStatus.Delayed, statusEvent.NewStatus);
    }

    [Fact]
    public void UpdateStatus_WithSameStatus_ShouldNotRaiseEvent()
    {
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        flight.ClearDomainEvents(); // Clear creation event

        flight.UpdateStatus(FlightStatus.InTime); // Same status

       
        Assert.Empty(flight.DomainEvents);
    }

    [Fact]
    public void UpdateDetails_WithChanges_ShouldRaiseFlightUpdatedEvent()
    {
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        flight.ClearDomainEvents(); // Clear creation event

        flight.UpdateDetails("CHI", "MIA", DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow.AddDays(1).AddHours(3));

       
        Assert.Single(flight.DomainEvents);
        Assert.IsType<FlightUpdatedEvent>(flight.DomainEvents.First());
        
        var updatedEvent = (FlightUpdatedEvent)flight.DomainEvents.First();
        Assert.Equal("CHI", updatedEvent.Origin);
        Assert.Equal("MIA", updatedEvent.Destination);
    }

    [Fact]
    public void UpdateDetails_WithoutChanges_ShouldNotRaiseEvent()
    {
        var departure = DateTimeOffset.UtcNow;
        var arrival = departure.AddHours(5);
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", departure, arrival);
        flight.ClearDomainEvents(); // Clear creation event

        flight.UpdateDetails("NYC", "LAX", departure, arrival); // Same details

       
        Assert.Empty(flight.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var flight = global::Domain.Entities.Flight.Create("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        flight.UpdateStatus(FlightStatus.Delayed);
        
        Assert.Equal(2, flight.DomainEvents.Count); // Creation + Status update

        flight.ClearDomainEvents();

       
        Assert.Empty(flight.DomainEvents);
    }
}
