using HDI.FinancialRiskEngine.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebUI.Controllers
{
    public class DashboardController : Controller
    {

        private readonly DashboardApiService _dashboardApiService;

        public DashboardController(DashboardApiService dashboardApiService)
        {
            _dashboardApiService = dashboardApiService;
        }
        public async Task<IActionResult> Index()
        {
            var tenantId = 1; // şimdilik sabit bir tenantId kullanıyoruz, gerçek uygulamada kullanıcıya göre dinamik olarak alınabilir

            var dashboardData = await _dashboardApiService.GetDashboardDataAsync(tenantId);

            return View(dashboardData);
        }
    }
}
