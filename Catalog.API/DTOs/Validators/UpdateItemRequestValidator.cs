using FluentValidation;

namespace Catalog.API.DTOs
{
    public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        public UpdateItemRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Name must not exceed 50 characters if provided.");
            
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 500 characters if provided.");

            RuleFor(x => x.PictureFileName)
                .MaximumLength(250)
                .When(x => !string.IsNullOrEmpty(x.PictureFileName))
                .WithMessage("PictureFileName must not exceed 250 characters if provided.");
            
            RuleFor(x => x.PictureUrl)
                .MaximumLength(250)
                .When(x => !string.IsNullOrEmpty(x.PictureUrl))
                .WithMessage("PictureUrl must not exceed 250 characters if provided.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.PictureUrl))
                .WithMessage("PictureUrl must be a valid URL if provided.");
            
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x.Price != null )
                .WithMessage("Price cannot be negative if provided.");

            RuleFor(x => x.Brand)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Brand))
                .WithMessage("Brand must not exceed 50 characters if provided.");
            
            RuleFor(x => x.Type)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Type))            
                .WithMessage("Type must not exceed 50 characters if provided.");
        }
    }
}