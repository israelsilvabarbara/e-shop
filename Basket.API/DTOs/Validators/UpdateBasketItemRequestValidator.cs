using FluentValidation;


namespace Basket.API.DTOs
{
    public class UpdateBasketItemRequestValidator: AbstractValidator<UpdateBasketItemRequest>
    {
        public UpdateBasketItemRequestValidator()
        {
            RuleFor(x => x.BasketId)
                .NotEmpty()
                .WithMessage("BasketId is required.");
            
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}