using FluentValidation;

namespace Basket.API.DTOs
{
    public class BasketItemRequestValidator : AbstractValidator<BasketItemRequest>
    {
        public BasketItemRequestValidator()
        {

            RuleFor(x => x.ProductId)
                .NotNull()
                .WithMessage("ProductId is required.");

        }
    }
}