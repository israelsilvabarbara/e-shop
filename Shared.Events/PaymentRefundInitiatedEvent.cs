namespace Shared.Events
{
    public record PaymentRefundInitiatedEvent
    (
        Guid OrderId,
        Guid PaymentId,
        DateTime PaymentDate,
        DateTime EventDate
    );
}