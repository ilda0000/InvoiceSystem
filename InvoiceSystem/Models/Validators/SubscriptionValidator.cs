using FluentValidation;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Models.Validators
{
    public class SubscriptionValidator : AbstractValidator<SubscriptionDTO>
    {
        public SubscriptionValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.PlanId)
                .GreaterThan(0);

            RuleFor(x => x.StartDate)
                .NotNull();

            RuleFor(x => x.IsActive)
                .NotNull();
        }
    }
}