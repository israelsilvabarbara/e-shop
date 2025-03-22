using FluentValidation;

namespace Catalog.API.DTOs
{
    public class ItemListRequestValidator : AbstractValidator<ItemListRequest>
    {
        public ItemListRequestValidator()
        {
            RuleFor(x => x.Pagination)
                .NotNull()
                .SetValidator( new PaginationRequestValidator());

            RuleFor(x => x.Filter)
            .NotNull()
            .SetValidator(new FilterRequestValidator());
        }
    }
}