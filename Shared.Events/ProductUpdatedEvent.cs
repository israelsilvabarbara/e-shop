namespace Shared.Events
{
    public record ProductUpdatedEvent
    (
        Guid productId,
        Guid productName,
        int stock,
        DateTime EventDate
    );
}