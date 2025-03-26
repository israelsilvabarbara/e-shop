namespace Shared.Events
{
    public record PaymentProcessedEvent
    (
        Guid OrderId,
        Guid PaymentId,
        DateTime EventDate
    );
}