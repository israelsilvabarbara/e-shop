namespace Shared.Events
{
    public record BasketCheckOutEvent
    (
        Guid BasketId,
        Guid CustomerId,
        IEnumerable<Guid> ProductIds,
        DateTime EventDate
    );
}