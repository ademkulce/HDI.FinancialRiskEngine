using FluentValidation;
using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;

namespace HDI.FinancialRiskEngine.Application.Validators.BusinessTopics
{
    public class CreateBusinessTopicDtoValidator : AbstractValidator<CreateBusinessTopicDto>
    {
        public CreateBusinessTopicDtoValidator()
        {
            RuleFor(x => x.TenantId)
                .GreaterThan(0);

            RuleFor(x => x.AgreementId)
                .GreaterThan(0);

            RuleFor(x => x.BusinessPartnerId)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(100);

            RuleFor(x => x.TopicDate)
                .NotEmpty();
        }
    }
}
