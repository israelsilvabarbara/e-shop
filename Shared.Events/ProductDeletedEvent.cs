namespace Shared.Events
{
    public record ProductDeletedEvent
    (
        Guid Id,
        Guid ProductId,
        string ProductName,
        DateTime EventDate
    ):BaseEvent(Id);
}
