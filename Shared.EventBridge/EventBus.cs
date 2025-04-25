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

    public async Task SendAsync<TEvent>(
        TEvent originalEvent,
        Services originService,
        LogEventType eventType,
        LogStatus status,
        string details
    ) where TEvent : IEvent
    {
        var senderId = originalEvent.Id;

        var logEvent = new LogEvent(
            Id: senderId,
            Timestamp: DateTime.UtcNow,
            Service: originService.ToString(),
            EventType: eventType.ToString(),
            Status: status.ToString(),
            Details: details
        );

        await _publishEndpoint.Publish(logEvent);
        await _publishEndpoint.Publish(originalEvent);
    }

    public async Task ConsumeAsync(
        Guid consumerId,
        Services DestinationService,
        string details
    )
    {
        var logConsumeEvent = new LogConsumedEvent
        (
            Id: consumerId,
            Timestamp: DateTime.UtcNow,
            Service: DestinationService.ToString(),
            Details: details
        );

        await _publishEndpoint.Publish(logConsumeEvent);
    }
}
