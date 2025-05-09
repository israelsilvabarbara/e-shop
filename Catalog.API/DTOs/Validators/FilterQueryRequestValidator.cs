using FluentValidation;

namespace Catalog.API.DTOs
{
    public class FilterQueryRequestValidator: AbstractValidator<FilterQueryRequest>
    {
        public FilterQueryRequestValidator()
        {
            RuleFor(x => x.MinPrice)
                .Must(x => x == null || x >= 0).WithMessage("MinPrice cannot be negative.");

            RuleFor(x => x.MaxPrice)
                .Must(x => x == null || x >= 0).WithMessage("MaxPrice cannot be negative.");

            RuleFor(x => x.Type)
                .Must(x => string.IsNullOrWhiteSpace(x) || x.Length <= 50).WithMessage("Type cannot exceed 50 characters.");

            RuleFor(x => x.Brand)
                .Must(x => string.IsNullOrWhiteSpace(x) || x.Length <= 50).WithMessage("Brand cannot exceed 50 characters.");
    
        }
    }
}