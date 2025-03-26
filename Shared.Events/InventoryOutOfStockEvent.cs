namespace Shared.Events
{
    public record ProductOutOfStockEvent
    (
        Guid ProductId,
        DateTime EventDate
    );
}