namespace Shared.Events
{
    public record InventoryStockLowEvent
    (
        Guid Id,
        Guid ProductId,
        int Stock,
        DateTime EventDate
    ):IEvent;
}