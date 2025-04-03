using FluentValidation;

namespace Inventory.API.DTOs
{
    public class RestockItemRequestValidator: AbstractValidator<RestockItemRequest>
    {
        public RestockItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.")
                .Must(x => Guid.TryParse(x.ToString(), out _))
                .WithMessage("ProductId must be a valid GUID.");
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}