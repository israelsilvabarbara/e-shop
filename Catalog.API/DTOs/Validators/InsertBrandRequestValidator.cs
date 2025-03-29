using FluentValidation;

namespace Catalog.API.DTOs
{
    public class InsertBrandRequestValidator : AbstractValidator<InsertBrandRequest>
    {
        public InsertBrandRequestValidator()
        {
            RuleFor(request => request.Brands)
                .NotNull()
                .WithMessage("The 'Brands' collection cannot be null.")
                .NotEmpty()
                .WithMessage("The 'Brands' collection cannot be empty.");

            RuleForEach(request => request.Brands)
                .NotNull()
                .WithMessage("Each type in the 'Brands' collection must not be null.")
                .NotEmpty()
                .WithMessage("Each type in the 'Brands' collection must not be empty.")
                .Matches(@"^[a-zA-Z0-9\s-]+$")
                .WithMessage("Each type must be properly formed and can only contain letters, numbers, spaces, or hyphens.");
        }
    }
}