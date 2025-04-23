using MassTransit;
using Shared.EventBridge.Enums;
using Shared.Events;

public class EventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendAsync(
        BaseEvent originalEvent,
        Services originService,
        LogEventType eventType,
        LogStatus status,
        string details
    )
    {
        var correlationId = originalEvent.Id;

        // Publish the event to the message bus
        await _publishEndpoint.Publish(originalEvent);

        var newLogEvent = new LogEvent(
            Id: correlationId,
            Timestamp: DateTime.UtcNow,
            Service: originService.ToString(),
            EventType: eventType.ToString(),
            Status: status.ToString(),
            Details: details
        );

        // Create and send a log for the event
        await _publishEndpoint.Publish(newLogEvent);
    }

    public async Task ConsumeAsync(
        Guid Id,
        Services DestinationService,
        string details
    )
    {
        var consumeEvent = new LogConsumedEvent
        (
            Id: Id,
            Timestamp: DateTime.UtcNow,
            Service: DestinationService.ToString(),
            Details: details
        );

        await _publishEndpoint.Publish(consumeEvent);
    }
}
