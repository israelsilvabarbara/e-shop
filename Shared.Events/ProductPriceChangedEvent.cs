namespace Shared.Events
{
    public record ProductPriceChangedEvent
    (
        Guid productId,
        decimal Price,
        DateTime EventDate
    );
}