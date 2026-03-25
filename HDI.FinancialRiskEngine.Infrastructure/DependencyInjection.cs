using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Infrastructure.Persistence;
using HDI.FinancialRiskEngine.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HDI.FinancialRiskEngine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Uygulamanın MSSQL veritabanına bağlanmasını sağlar.
            // Connection string bilgisi appsettings.json içinden okunur.
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Application katmanındaki servis arayüzlerinin
            // Infrastructure içindeki somut implementasyonlarla eşleşmesini sağlar.
            services.AddScoped<IAgreementService, AgreementService>();
            services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
            services.AddScoped<IBusinessTopicService, BusinessTopicService>();
            services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();

            return services;
        }
    }
}

