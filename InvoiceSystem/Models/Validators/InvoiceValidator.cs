using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class InvoiceValidator : AbstractValidator<InvoiceDTO>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.SubscriptionId)
                .GreaterThan(0);

            RuleFor(x => x.BillingDate)
                .NotNull();

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Paid)
                .NotNull();
        }
    }
}