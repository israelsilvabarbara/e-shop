
using FluentValidation;
using Shared.EventBridge.Enums;


namespace Logger.API.DTOs
{
    public class ServiceSelectorRequestValidator : AbstractValidator<ServiceSelectorRequest>
    {
        public ServiceSelectorRequestValidator()
        {
            RuleFor(x => x.Service)
                .NotEmpty()
                .WithMessage("Service name is required.")
                .Must(service => Enum.IsDefined(typeof(Services), service))
                .WithMessage($"Invalid service name. Available options: {string.Join(", ", Enum.GetNames(typeof(Services)))}");
        }
    }
}