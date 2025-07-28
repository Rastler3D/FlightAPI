using Application.Common.Abstractions.Caching;
using Application.Flights.Queries.GetFlights;
using Domain.Common;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Flight.Tests.Application.Flights.Queries;

public sealed class GetFlightsQueryHandlerTests
{
    private readonly Mock<IFlightRepository> _flightRepositoryMock;
    private readonly Mock<ILogger<GetFlightsQueryHandler>> _loggerMock;
    private readonly GetFlightsQueryHandler _handler;

    public GetFlightsQueryHandlerTests()
    {
        _flightRepositoryMock = new Mock<IFlightRepository>();
        _loggerMock = new Mock<ILogger<GetFlightsQueryHandler>>();
        _handler = new GetFlightsQueryHandler(_flightRepositoryMock.Object, _loggerMock.Object);
    }

   
    [Fact]
    public async Task Handle_ShouldReturnFromRepository()
    {
        
        var query = new GetFlightsQuery();
        var flights = new List<global::Domain.Entities.Flight>
        {
            new("NYC", "LAX", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(5), FlightStatus.InTime)
        };
        

        _flightRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IEnumerable<global::Domain.Entities.Flight>>(flights));

        
        var result = await _handler.Handle(query, CancellationToken.None);

        
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
       
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ShouldReturnFailure()
    {
        
        var query = new GetFlightsQuery();
        var errorMessage = "Database error";

        

        _flightRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<IEnumerable<global::Domain.Entities.Flight>>(errorMessage));

        
        var result = await _handler.Handle(query, CancellationToken.None);

        
        Assert.True(result.IsFailure);
        Assert.Equal(errorMessage, result.Error);
    }
}
