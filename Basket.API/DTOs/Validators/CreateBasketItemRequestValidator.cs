using FluentValidation;

namespace Basket.API.DTOs
{
    public class CreateBasketItemRequestValidator : AbstractValidator<CreateBasketItemRequest>
    {
        public CreateBasketItemRequestValidator()
        {
            RuleFor(x => x.BuyerId)
                .NotEmpty();

            RuleFor(x => x.ProductId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
                
            RuleFor(x => x.UnitPrice)
                .GreaterThan(0);

            RuleFor(x => x.PictureUrl)
                .NotEmpty()
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("PictureUrl must be a valid URL.");
        }
    }

}