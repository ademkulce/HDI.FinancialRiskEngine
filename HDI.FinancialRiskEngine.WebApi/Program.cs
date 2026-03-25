using HDI.FinancialRiskEngine.Infrastructure;
using HDI.FinancialRiskEngine.WebApi;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

// services.AddScoped<IAgreementService, AgreementService>();
// services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
// services.AddScoped<IBusinessTopicService, BusinessTopicService>();
// services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
