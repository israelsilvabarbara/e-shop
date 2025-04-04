using FluentValidation;

namespace Basket.API.DTOs
{
    public class BasketItemRequestValidator : AbstractValidator<BasketItemRequest>
    {
        public BasketItemRequestValidator()
        {

            RuleFor(x => x.BuyerId)
                .NotEmpty()
                .WithMessage("BuyerId is required.");

            RuleFor(x => x.ProductId)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

        }
    }
}