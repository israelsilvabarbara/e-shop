using FluentValidation;


namespace Basket.API.DTOs
{
    public class UpdateBasketItemRequestValidator: AbstractValidator<UpdateBasketItemRequest>
    {
        public UpdateBasketItemRequestValidator()
        {
               
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");
               
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}