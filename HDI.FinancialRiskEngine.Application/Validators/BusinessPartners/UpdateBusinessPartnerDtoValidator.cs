using FluentValidation;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;

namespace HDI.FinancialRiskEngine.Application.Validators.BusinessPartners
{
    public class UpdateBusinessPartnerDtoValidator : AbstractValidator<UpdateBusinessPartnerDto>
    {
        public UpdateBusinessPartnerDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.TenantId)
                .GreaterThan(0);

            RuleFor(x => x.AgreementId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .MaximumLength(200)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(30);
        }
    }
}
