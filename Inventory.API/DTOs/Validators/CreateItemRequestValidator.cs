using FluentValidation;

namespace Inventory.API.DTOs
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
                
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("ProductName must not exceed 50 characters.")
                .Must(name => name.All(char.IsLetterOrDigit))
                .WithMessage("ProductName must only contain letters and digits.");

            RuleFor(x => x.Stock)
                .NotEmpty()
                .InclusiveBetween(0, 100)
                .WithMessage("Stock must be between 0 and 100.");
            
            RuleFor(x => x.StockTreshold)
                .InclusiveBetween(10, 200)
                .WithMessage("StockTreshold must be between 10 and 200.");
            
        }
    }
}