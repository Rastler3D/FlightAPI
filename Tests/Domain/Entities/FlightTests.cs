using Domain.Enums;

namespace Flight.Tests.Domain.Entities;

public sealed class FlightTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateFlight()
    {
        // Arrange
        var origin = "NYC";
        var destination = "LAX";
        var departure = DateTimeOffset.UtcNow;
        var arrival = departure.AddHours(5);
        var status = FlightStatus.InTime;

        // Act
        var flight = new global::Domain.Entities.Flight(origin, destination, departure, arrival, status);

        // Assert
        Assert.Equal(origin, flight.Origin);
        Assert.Equal(destination, flight.Destination);
        Assert.Equal(departure, flight.Departure);
        Assert.Equal(arrival, flight.Arrival);
        Assert.Equal(status, flight.Status);
    }

    [Fact]
    public void UpdateStatus_WithNewStatus_ShouldUpdateStatus()
    {
        // Arrange
        var flight = new global::Domain.Entities.Flight("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        var newStatus = FlightStatus.Delayed;

        // Act
        flight.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, flight.Status);
    }

    [Fact]
    public void UpdateDetails_WithNewDetails_ShouldUpdateAllDetails()
    {
        // Arrange
        var flight = new global::Domain.Entities.Flight("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5));
        var newOrigin = "CHI";
        var newDestination = "MIA";
        var newDeparture = DateTimeOffset.UtcNow.AddDays(1);
        var newArrival = newDeparture.AddHours(3);

        // Act
        flight.UpdateDetails(newOrigin, newDestination, newDeparture, newArrival);

        // Assert
        Assert.Equal(newOrigin, flight.Origin);
        Assert.Equal(newDestination, flight.Destination);
        Assert.Equal(newDeparture, flight.Departure);
        Assert.Equal(newArrival, flight.Arrival);
    }
}
