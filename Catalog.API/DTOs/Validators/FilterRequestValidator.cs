using System.Data;
using FluentValidation;

namespace Catalog.API.DTOs
{
    public class FilterRequestValidator : AbstractValidator<FilterRequest>
    {
        public FilterRequestValidator()
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue) 
                .WithMessage("MinPrice cannot be negative if provided.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("MaxPrice cannot be negative if provided.");

            RuleFor(x => x.Brand)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Brand)) 
                .WithMessage("Brand must not exceed 50 characters.");

            RuleFor(x => x.Type)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Type))
                .WithMessage("Type must not exceed 50 characters."); 
                
        }
    }
}