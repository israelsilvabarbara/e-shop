namespace Shared.Events
{
    public record OrderCreatedEvent
    (
        Guid Id,
        Guid OrderId,
        Guid CustomerId,
        Guid BasketId,
        decimal TotalPrice,
        DateTime EventDate
    ): BaseEvent(Id);
}