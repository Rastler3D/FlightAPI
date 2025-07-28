using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Converters;

public class DateTimeOffsetConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
    d => d.ToUniversalTime(),
    d => d.ToUniversalTime());