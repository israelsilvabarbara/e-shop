using FluentValidation;

public class BasketItemRequestValidator : AbstractValidator<BasketItemRequest>
{
    public BasketItemRequestValidator()
    {
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
