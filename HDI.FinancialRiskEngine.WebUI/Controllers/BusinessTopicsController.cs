
using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;
using HDI.FinancialRiskEngine.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HDI.FinancialRiskEngine.WebUI.Controllers
{
    public class BusinessTopicsController : Controller
    {
        private readonly BusinessTopicApiService _businessTopicApiService;
        private readonly BusinessPartnerApiService _businessPartnerApiService;
        private readonly AgreementApiService _agreementApiService;

        public BusinessTopicsController(BusinessTopicApiService businessTopicApiService, BusinessPartnerApiService businessPartnerApiService, AgreementApiService agreementApiService)
        {
            _businessTopicApiService = businessTopicApiService;
            _businessPartnerApiService = businessPartnerApiService;
            _agreementApiService = agreementApiService;
        }

        /// <summary>
        /// İş konuları liste ekranını getirir
        /// </summary>
        public async Task<IActionResult> Index()
        {
            int tenantId = 1;

            var topics = await _businessTopicApiService.GetAllAsync(tenantId);

            return View(topics);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int tenantId = 1;

            await LoadDropdownsAsync(tenantId);

            var model = new CreateBusinessTopicDto
            {
                TenantId = tenantId,
                TopicDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBusinessTopicDto dto)
        {
            int tenantId = 1;

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(tenantId);
                return View(dto);
            }

            var created = await _businessTopicApiService.CreateAsync(dto);

            if (!created)
            {
                ModelState.AddModelError(string.Empty, "İş konusu oluşturulamadı.");
                await LoadDropdownsAsync(tenantId);
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync(int tenantId)
        {
            var partners = await _businessPartnerApiService.GetAllAsync(tenantId);
            ViewBag.BusinessPartners = partners.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var agreements = await _agreementApiService.GetAllAsync(tenantId);
            ViewBag.Agreements = agreements.Select(x => new SelectListItem
            {
                Text = $"{x.Code} - {x.Name}",
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}