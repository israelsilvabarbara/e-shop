namespace Shared.Events
{
    public record OrderCancelledEvent
    (
        int OrderId,
        DateTime EventDate
    );
}