namespace Shared.Events
{
    public record OrderCreatedEvent
    (
        Guid OrderId,
        Guid CustomerId,
        Guid BasketId,
        decimal TotalPrice,
        DateTime EventDate
    );
}