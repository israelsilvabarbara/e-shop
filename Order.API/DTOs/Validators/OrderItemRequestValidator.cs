using FluentValidation;

namespace Order.API.DTOs
{

    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(item => item.Id)
                .NotEmpty().WithMessage("Item ID must not be empty.");

            RuleFor(item => item.Name)
                .NotEmpty().WithMessage("Item name must not be empty.")
                .MaximumLength(100).WithMessage("Item name cannot exceed 100 characters.");

            RuleFor(item => item.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}