using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;
using HDI.FinancialRiskEngine.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HDI.FinancialRiskEngine.WebUI.Controllers
{
    public class BusinessPartnersController : Controller
    {
        private readonly BusinessPartnerApiService _businessPartnerApiService;
        private readonly AgreementApiService agreementApiService;

        public BusinessPartnersController(BusinessPartnerApiService businessPartnerApiService, AgreementApiService agreementApiService)
        {
            _businessPartnerApiService = businessPartnerApiService;
            this.agreementApiService = agreementApiService;
        }

        /// <summary>
        /// İş ortakları liste ekranını getirir.
        /// İlk aşamada tenantId sabit verilmiştir.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            int tenantId = 1;

            var partners = await _businessPartnerApiService.GetAllAsync(tenantId);

            return View(partners);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int tenantId = 1;

            var agreements = await agreementApiService.GetAllAsync(tenantId);

            ViewBag.Agreements = agreements.Select(x=> new SelectListItem
            {
                Text =$"{x.Code} - {x.Name}",
                Value = x.Id.ToString()
            }).ToList();

            var model = new CreateBusinessPartnerDto
            {
                TenantId = 1,
                IsActive = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBusinessPartnerDto dto)
        {
            int tenantId = 1;

            if (!ModelState.IsValid)
            {
                var agreements = await agreementApiService.GetAllAsync(tenantId);

                ViewBag.Agreements = agreements.Select(x => new SelectListItem
                {
                    Text = $"{x.Code} - {x.Name}",
                    Value = x.Id.ToString()
                }).ToList();
            }
                
            var created = await _businessPartnerApiService.CreateAsync(dto);

            if (!created)
            {
                ModelState.AddModelError(string.Empty, "İş ortağı oluşturulamadı.");

                var agreements = await agreementApiService.GetAllAsync(tenantId);

                ViewBag.Agreements = agreements.Select(x => new SelectListItem
                {
                    Text = $"{x.Code} - {x.Name}",
                    Value = x.Id.ToString()
                }).ToList();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}