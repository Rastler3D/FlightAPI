using Application.Flights.Commands.CreateFlight;
using Application.Flights.Commands.UpdateFlightStatus;
using Application.Flights.Queries.GetFlights;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mappings;

[Mapper]
public static partial class FlightMappings
{
    public static partial FlightResponse ToResponse(this Flight flight);
    public static partial IEnumerable<FlightResponse> ToResponse(this IEnumerable<Flight> flights);
    
    public static partial Flight ToEntity(this CreateFlightCommand command);
    
    [MapperIgnoreTarget(nameof(Flight.Id))]
    public static partial void UpdateEntity(this UpdateFlightStatusCommand command, Flight flight);
}
