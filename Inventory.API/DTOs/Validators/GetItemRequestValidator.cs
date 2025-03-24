using FluentValidation;

namespace Inventory.API.DTOs
{
    public class GetItemRequestValidator : AbstractValidator<GetItemsRequest>
    {
        public GetItemRequestValidator()
        {
           RuleFor(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("ProductIds cannot be empty.")
            .Must(ids => ids.All(id => id.GetType() == typeof(int)))
            .WithMessage("ProductIds must only contain numbers.")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("ProductIds must only contain positive integers.");
        }
    }
}