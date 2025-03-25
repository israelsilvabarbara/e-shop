namespace Shared.Events
{
    public record ProductCreatedEvent
    (
        Guid productId,
        string productName,
        int stock,
        DateTime created
    );
}

