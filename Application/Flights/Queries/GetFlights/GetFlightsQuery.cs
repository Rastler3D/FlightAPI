using Application.Common.Abstractions.Messaging;
using Application.Common.Security;
using Domain.Common;

namespace Application.Flights.Queries.GetFlights;

[Authorize]
public sealed record GetFlightsQuery(
    string? Origin = null,
    string? Destination = null) : ICachedQuery<IEnumerable<FlightResponse>>
{
    public string CacheKey => $"flights:{Origin ?? "all"}:{Destination ?? "all"}";
    public TimeSpan? CacheExpiration => TimeSpan.FromMinutes(5);
}
