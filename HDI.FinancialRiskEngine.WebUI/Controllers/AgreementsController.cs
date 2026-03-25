using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using HDI.FinancialRiskEngine.Infrastructure.Services;
using HDI.FinancialRiskEngine.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebUI.Controllers
{
    public class AgreementsController : Controller
    {
        private readonly AgreementApiService _agreementApiService;

        public AgreementsController(AgreementApiService agreementApiService)
        {
            _agreementApiService = agreementApiService;
        }

        /// <summary>
        /// Anlaşma liste ekranını getirir.
        /// İlk aşamada tenantId sabit verilmiştir.
        /// İleride giriş yapan kullanıcıya göre dinamik hale getirilebilir.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            int tenantId = 1;

            var agreements = await _agreementApiService.GetAllAsync(tenantId);

            return View(agreements);
        }

        /// <summary>
        /// Yeni anlaşma formunu getirir.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            // İlk aşamada tenant sabit veriliyor.
            var model = new CreateAgreementDto
            {
                TenantId = 1,
                StartDate = DateTime.Now,
                IsActive = true
            };

            return View(model);
        }

        /// <summary>
        /// Yeni anlaşma kaydını WebApi üzerinden oluşturur.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAgreementDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var created = await _agreementApiService.CreateAsync(dto);

            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Anlaşma oluşturulurken bir hata oluştu.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}