using FluentValidation;

namespace Catalog.API.DTOs
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.PictureFileName)
                .NotEmpty().WithMessage("PictureFileName is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.CatalogBrandId)
                .GreaterThan(0).WithMessage("CatalogBrandId must be a valid ID.");

            RuleFor(x => x.CatalogTypeId)
                .GreaterThan(0).WithMessage("CatalogTypeId must be a valid ID.");

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0).WithMessage("AvailableStock must be 0 or higher.");

            RuleFor(x => x.RestockThreshold)
                .GreaterThanOrEqualTo(0).WithMessage("RestockThreshold must be 0 or higher.");

            RuleFor(x => x.MaxStockThreshold)
                .GreaterThanOrEqualTo(0).WithMessage("MaxStockThreshold must be 0 or higher.");
        }
    }
}
