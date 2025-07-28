using Application.Common.Abstractions.Caching;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.Flights.EventHandlers;

public sealed class FlightCacheInvalidationHandler(
    ICacheService cacheService,
    ILogger<FlightCacheInvalidationHandler> logger)
    :
        INotificationHandler<FlightCreatedEvent>,
        INotificationHandler<FlightStatusUpdatedEvent>,
        INotificationHandler<FlightUpdatedEvent>
{
    public async Task Handle(FlightCreatedEvent notification, CancellationToken cancellationToken)
    {
        await InvalidateFlightCaches(cancellationToken);

        logger.LogInformation("Cache invalidated after flight creation: {FlightId} from {Origin} to {Destination}",
            notification.FlightId, notification.Origin, notification.Destination);
    }

    public async Task Handle(FlightStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await InvalidateFlightCaches(cancellationToken);

        logger.LogInformation(
            "Cache invalidated after flight status update: {FlightId} from {OldStatus} to {NewStatus}",
            notification.FlightId, notification.OldStatus, notification.NewStatus);
    }

    public async Task Handle(FlightUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await InvalidateFlightCaches(cancellationToken);

        logger.LogInformation("Cache invalidated after flight update: {FlightId}",
            notification.FlightId);
    }

    private async Task InvalidateFlightCaches(CancellationToken cancellationToken)
    {
        var pattern = "flights:*";

        await cacheService.RemoveByPatternAsync(pattern, cancellationToken);

        logger.LogDebug("Invalidated cache for flights");
    }
}