namespace Shared.Events
{
    public record DiscountApplied
    (
        int OrderId,
        int Discount,
        DateTime EventDate
    );
}