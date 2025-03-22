using FluentValidation;

namespace Catalog.API.DTOs
{
    public class PaginationRequestValidator: AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
            RuleFor(x => x.PageSize)
                .GreaterThan(0);
        }
    }
}