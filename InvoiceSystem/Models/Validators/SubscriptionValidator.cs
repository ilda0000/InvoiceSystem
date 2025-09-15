using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class SubscriptionValidator : AbstractValidator<SubscriptionDTO>
    {
        public SubscriptionValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be greater than 0.");

            RuleFor(x => x.PlanId)
                .GreaterThan(0).WithMessage("Plan ID must be greater than 0.");

            RuleFor(x => x.StartDate)
                .NotNull().WithMessage("Start date is required.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive must have a value.");
        }
    }
}
