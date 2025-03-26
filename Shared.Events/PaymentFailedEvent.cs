namespace Shared.Events
{
    public record PaymentFailedEvent
    (
        Guid OrderId,
        Guid PaymentId,
        string Reason,
        string Code,
        string Message,
        DateTime EventDate
    );
}