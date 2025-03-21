using FluentValidation;


namespace Basket.API.DTOs
{
    public class UpdateBasketItemRequestValidator: AbstractValidator<UpdateBasketItemRequest>
    {
        public UpdateBasketItemRequestValidator()
        {
            RuleFor(x => x.BuyerId)
                .NotEmpty();
               
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .GreaterThan(0);
               
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}