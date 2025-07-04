using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class PaymentValidator : AbstractValidator<PaymentDTO>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.InvoiceId)
                .GreaterThan(0);

            RuleFor(x => x.PaymentDate)
                .NotNull();

            RuleFor(x => x.AmountPaid)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PaymentMethodName)
                .NotNull().NotEmpty();
        }
    }
}