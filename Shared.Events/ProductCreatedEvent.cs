namespace Shared.Events
{
    public record ProductCreatedEvent
    (
        Guid Id,
        Guid ProductId,
        string ProductName,
        DateTime EventDate
    ):IEvent;
}

