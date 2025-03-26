namespace Shared.Events
{
    public record ProductStockChangedEvent
    (
        Guid ProductId,
        int Stock
    );
}