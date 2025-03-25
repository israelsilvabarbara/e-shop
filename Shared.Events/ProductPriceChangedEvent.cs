namespace Shared.Events
{
    public record ProductRestockedEvent
    (
        Guid productId,
        int quantity
    );
}