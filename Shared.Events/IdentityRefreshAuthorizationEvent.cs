namespace Shared.Events
{
    public record IdentityRefreshAuthorizationEvent
    (
        Guid Id,
        string PublicKey,
        DateTime Expiration,
        DateTime EventDate
    ): BaseEvent(Id);
}