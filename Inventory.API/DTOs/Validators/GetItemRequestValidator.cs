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
            .Must(ids => ids.All(id => Guid.TryParse(id.ToString(), out _)))
            .WithMessage("ProductIds must be valid GUIDs.");
        }
    }
}