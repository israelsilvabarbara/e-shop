namespace Shared.Events
{
    public record ProductStockDecreasedEvent
    (
        Guid productId,
        int stock,
    );
}