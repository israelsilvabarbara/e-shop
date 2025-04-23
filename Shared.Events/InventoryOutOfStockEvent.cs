namespace Shared.Events
{
    public record ProductOutOfStockEvent
    (
        Guid Id,
        Guid ProductId,
        DateTime EventDate
    ): BaseEvent(Id);
}