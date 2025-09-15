using FluentValidation;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.ErrorMessages;

namespace InvoiceSystem.Models.Validators
{
    public class InvoiceValidator : AbstractValidator<InvoiceDTO>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage(AllErrors.CustomerNotFound);

            RuleFor(x => x.SubscriptionId)
                .NotEmpty()
                .WithMessage(AllErrors.SubscriptionNotFound);

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage(AllErrors.InvoiceAmountInvalid);

            RuleFor(x => x.BillingDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Billing date cannot be in the future.");

            RuleFor(x => x.DiscountApplied)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Discount cannot be negative.");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required.")
                .Must(s => new[] { "created", "paid", "overdue" }.Contains(s))
                .WithMessage("Invalid invoice status.");    
        }
    }
}