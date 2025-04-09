namespace Shared.Events
{
    public record IdentityRefreshAuthorizationEvent
    (
        string PublicKey,
        DateTime Expiration,
        DateTime EventDate
    );
}