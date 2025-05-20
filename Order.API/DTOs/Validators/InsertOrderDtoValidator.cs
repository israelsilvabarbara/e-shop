using FluentValidation;

namespace Order.API.DTOs
{

    public class InsertOrderRequestValidator : AbstractValidator<InsertOrderRequest>
    {
        public InsertOrderRequestValidator()
        {
            RuleFor(request => request.Items)
                .NotEmpty().WithMessage("Order must contain at least one item.");

            RuleForEach(request => request.Items)
                .SetValidator(new OrderItemRequestValidator());
        }
    }

}
