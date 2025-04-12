public class EventBridge
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly Logger _logger;

    public EventBridge(IPublishEndpoint publishEndpoint, Logger logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
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

        // Create and send a log for the event
        await _logger.LogAsync(new LogMessage
        {
            Id = correlationId,
            Service = originService.ToString(),
            Timestamp = DateTime.UtcNow,
            EventType = eventType,
            Status = status,
            Details = details
        });
    }
}
