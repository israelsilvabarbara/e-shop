namespace Shared.Events
{
    public record InventoryStockLowEvent
    (
        Guid ProductId,
        int Stock,
        DateTime EventDate
    );
}