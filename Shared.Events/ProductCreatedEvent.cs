namespace Shared.Events
{
    public record ProductCreatedEvent
    (
        Guid ProductId,
        string ProductName,
        DateTime EventDate
    );
}

