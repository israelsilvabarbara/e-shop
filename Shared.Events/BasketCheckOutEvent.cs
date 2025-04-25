namespace Shared.Events
{
    public record BasketCheckOutEvent
    (
        Guid Id,
        Guid BasketId,
        Guid CustomerId,
        IEnumerable<Guid> ProductIds,
        DateTime EventDate
    ):IEvent;
}