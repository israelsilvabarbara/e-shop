namespace Shared.Events
{
    public record ProductStockChangedEvent
    (
        Guid Id,
        Guid ProductId,
        int Stock,
        DateTime EventDate
    ):IEvent;
}