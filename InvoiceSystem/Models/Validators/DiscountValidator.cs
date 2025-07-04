using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class DiscountDTOValidator : AbstractValidator<DiscountDTO>
    {
        public DiscountDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty();

            RuleFor(x => x.Type)
                .NotNull().NotEmpty()
                .Must(type => type == "Fixed" || type == "Percentage")
                .WithMessage("Type must be 'Fixed' or 'Percentage'");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MinMonthsRequired)
                .GreaterThanOrEqualTo(0);
        }
    }
}