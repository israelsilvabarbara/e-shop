namespace Shared.Events
{
    public record OrderCancelledEvent
    (
        Guid Id,
        int OrderId,
        DateTime EventDate
    ):IEvent;
}