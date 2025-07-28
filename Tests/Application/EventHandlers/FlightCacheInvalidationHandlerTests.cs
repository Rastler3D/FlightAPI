using Application.Common.Abstractions.Caching;
using Application.Flights.EventHandlers;
using Domain.Enums;
using Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace Flight.Tests.Application.EventHandlers;

public sealed class FlightCacheInvalidationHandlerTests
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<FlightCacheInvalidationHandler>> _loggerMock;
    private readonly FlightCacheInvalidationHandler _handler;

    public FlightCacheInvalidationHandlerTests()
    {
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<FlightCacheInvalidationHandler>>();
        _handler = new FlightCacheInvalidationHandler(_cacheServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_FlightCreatedEvent_ShouldInvalidateRelevantCaches()
    {
       
        var flightCreatedEvent = new FlightCreatedEvent(1, "NYC", "LAX");

        
        await _handler.Handle(flightCreatedEvent, CancellationToken.None);

        
        _cacheServiceMock.Verify(x => x.RemoveByPatternAsync("flights:*", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FlightStatusUpdatedEvent_ShouldInvalidateRelevantCaches()
    {
       
        var statusUpdatedEvent = new FlightStatusUpdatedEvent(1, FlightStatus.InTime, FlightStatus.Delayed, "NYC", "LAX");

        
        await _handler.Handle(statusUpdatedEvent, CancellationToken.None);

        
        _cacheServiceMock.Verify(x => x.RemoveByPatternAsync("flights:*", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_FlightUpdatedEvent_ShouldInvalidateRelevantCaches()
    {
       
        var flightUpdatedEvent = new FlightUpdatedEvent(1, "NYC", "LAX");

        
        await _handler.Handle(flightUpdatedEvent, CancellationToken.None);

        
        _cacheServiceMock.Verify(x => x.RemoveByPatternAsync("flights:*", It.IsAny<CancellationToken>()), Times.Once);
    }
}
