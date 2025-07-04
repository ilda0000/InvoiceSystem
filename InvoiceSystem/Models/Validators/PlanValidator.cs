using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class PlanValidator : AbstractValidator<PlanDTO>
    {
        public PlanValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty();

            RuleFor(x => x.PricePerMonth)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxUsers)
                .GreaterThan(0);
        }
    }
}