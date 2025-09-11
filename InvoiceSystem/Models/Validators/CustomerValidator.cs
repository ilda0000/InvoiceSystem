using FluentValidation;
using InvoiceSystem.ErrorMessages;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Models.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDTO>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                  .NotEmpty().WithMessage(AllErrors.CustomerNameRequired);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(AllErrors.CustomerEmailRequired)
                .EmailAddress().WithMessage(AllErrors.CustomerEmailInvalid);
        }
    }
}
