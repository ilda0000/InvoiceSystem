using FluentValidation;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.ErrorMessages;

namespace InvoiceSystem.Models.Validators
{
    public class PaymentValidator : AbstractValidator<PaymentDTO>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty();

            RuleFor(x => x.AmountPaid)
                .NotEmpty()
                .WithMessage(AllErrors.PaymentAmountRequired)
                .GreaterThan(0)
                .WithMessage(AllErrors.PaymentAmountRequired);

            RuleFor(x => x.PaymentDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(AllErrors.PaymentDateInvalid);

            RuleFor(x => x.PaymentMethodName)
                .NotEmpty()
                .WithMessage(AllErrors.PaymentMethodRequired);
        }
    }
}   