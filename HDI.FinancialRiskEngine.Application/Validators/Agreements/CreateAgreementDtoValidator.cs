using FluentValidation;
using HDI.FinancialRiskEngine.Application.DTOs.Agreements;

namespace HDI.FinancialRiskEngine.Application.Validators.Agreements
{
    public class CreateAgreementDtoValidator : AbstractValidator<CreateAgreementDto>
    {
        public CreateAgreementDtoValidator()
        {
            RuleFor(x => x.TenantId)
                .GreaterThan(0);

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.BaseRiskRate)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => !x.EndDate.HasValue || x.EndDate.Value >= x.StartDate)
                .WithMessage("EndDate, StartDate değerinden küçük olamaz.");
        }
    }
}