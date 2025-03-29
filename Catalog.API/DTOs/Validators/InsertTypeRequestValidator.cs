
using FluentValidation;

namespace Catalog.API.DTOs
{
    public class InsertTypeRequestValidator : AbstractValidator<InsertTypeRequest>
    {
        public InsertTypeRequestValidator()
        {
            RuleFor(request => request.Types)
                .NotNull()
                .WithMessage("The 'Types' collection cannot be null.")
                .NotEmpty()
                .WithMessage("The 'Types' collection cannot be empty.");

            RuleForEach(request => request.Types)
                .NotNull()
                .WithMessage("Each type in the 'Types' collection must not be null.")
                .NotEmpty()
                .WithMessage("Each type in the 'Types' collection must not be empty.")
                .Matches(@"^[a-zA-Z0-9\s-]+$")
                .WithMessage("Each type must be properly formed and can only contain letters, numbers, spaces, or hyphens.");
        }
    }
}
