using FluentValidation;

namespace Basket.API.DTOs
{
    public class BasketItemRequestValidator : AbstractValidator<BasketItemRequest>
    {
        public BasketItemRequestValidator()
        {

            RuleFor(x => x.ItemId)
                .NotNull()
                .WithMessage("ItemId is required.");

        }
    }
}