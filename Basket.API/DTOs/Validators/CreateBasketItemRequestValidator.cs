using FluentValidation;

namespace Basket.API.DTOs
{
    public class CreateBasketItemRequestValidator : AbstractValidator<CreateBasketItemRequest>
    {
        public CreateBasketItemRequestValidator()
        {
            RuleFor(x => x.BuyerId)
                .NotEmpty()
                .WithMessage("BasketId is required.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("ProductName is required.")
                .MaximumLength(100)
                .WithMessage("ProductName must not exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("UnitPrice must be greater than 0.");

            RuleFor(x => x.PictureUrl)
                .NotEmpty()
                .WithMessage("PictureUrl is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("PictureUrl must be a valid URL.");
        }
    }

}