using FluentValidation;

namespace Catalog.API.DTOs
{
    public class PaginationQueryRequestValidator : AbstractValidator<PaginationQueryRequest>
    {
        public PaginationQueryRequestValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
        }
    }
}