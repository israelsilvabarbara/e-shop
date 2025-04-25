namespace Shared.Events
{
    public record ProductPriceChangedEvent
    (
        Guid Id,
        Guid productId,
        decimal Price,
        DateTime EventDate
    ):IEvent;
}