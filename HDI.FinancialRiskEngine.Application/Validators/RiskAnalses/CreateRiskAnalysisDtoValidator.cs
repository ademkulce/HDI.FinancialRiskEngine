using FluentValidation;
using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;

namespace HDI.FinancialRiskEngine.Application.Validators.RiskAnalyses
{
    public class CreateRiskAnalysisDtoValidator : AbstractValidator<CreateRiskAnalysisDto>
    {
        public CreateRiskAnalysisDtoValidator()
        {
            RuleFor(x => x.TenantId)
                .GreaterThan(0);

            RuleFor(x => x.BusinessTopicId)
                .GreaterThan(0);
        }
    }
}
