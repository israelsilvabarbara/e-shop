using MassTransit;

public class EventBridge
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBridge(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendAsync<TEvent>(
        Guid correlationId,
        TEvent @event,
        Services originService,
        LogEventType eventType,
        LogStatus status,
        string details
    )
    {
        // Publish the event to the message bus
        await _publishEndpoint.Publish(@event);


        var logEvent = new LogEvent(
            Id: correlationId,
            Timestamp: DateTime.UtcNow,
            Service: originService.ToString(),
            EventType: eventType.ToString(),
            Status: status.ToString(),
            Details: details
        );

        // Create and send a log for the event
        await _publishEndpoint.Publish(logEvent);
    }
}
