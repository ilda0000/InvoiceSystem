using FluentValidation;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class PlanValidator : AbstractValidator<PlanDTO>
    {
        public PlanValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Plan name is required.")
                 .MaximumLength(100).WithMessage("Plan name cannot exceed 100 characters.");

            RuleFor(x => x.PricePerMonth)
                .GreaterThanOrEqualTo(0).WithMessage("Price per month must be at least 0.");

            RuleFor(x => x.MaxUsers)
                .GreaterThan(0).WithMessage("Max users must be greater than 0.");
        }
    }
}
