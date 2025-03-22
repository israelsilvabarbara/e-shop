using FluentValidation;

namespace Catalog.API.DTOs
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.PictureFileName)
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.CatalogBrandId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.CatalogTypeId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.RestockThreshold)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxStockThreshold)
                .GreaterThanOrEqualTo(0);
        }

    }
}
