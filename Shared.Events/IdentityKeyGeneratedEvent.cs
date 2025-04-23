namespace Shared.Events
{
    public record IdentityKeyGeneratedEvent
    (
        Guid Id,
        DateTime EventDate
    ): BaseEvent(Id);
}