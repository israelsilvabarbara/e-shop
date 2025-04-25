namespace Shared.Events
{
    public record PaymentRefundInitiatedEvent
    (
        Guid Id,
        Guid OrderId,
        Guid PaymentId,
        DateTime PaymentDate,
        DateTime EventDate
    ):IEvent;
}