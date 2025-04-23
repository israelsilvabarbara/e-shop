namespace Shared.Events
{
    public record LogConsumedEvent
    (
        Guid Id,
        DateTime Timestamp,
        string Service,
        string Details
    ):BaseEvent(Id);
}