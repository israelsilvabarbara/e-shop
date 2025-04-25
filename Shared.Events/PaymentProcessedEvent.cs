namespace Shared.Events
{
    public record PaymentProcessedEvent
    (
        Guid Id,
        Guid OrderId,
        Guid PaymentId,
        DateTime EventDate
    ):IEvent;
}