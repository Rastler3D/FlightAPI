using System.Text.Json.Serialization;

namespace Domain.Enums;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FlightStatus
{
    InTime = 0,
    Delayed = 1,
    Cancelled = 2
}
