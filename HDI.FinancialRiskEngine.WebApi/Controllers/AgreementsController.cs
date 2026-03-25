
using HDI.FinancialRiskEngine.Application.DTOs.Agreements;
using HDI.FinancialRiskEngine.Application.Interfaces;
using HDI.FinancialRiskEngine.Application.Interfaces.HDI.FinancialRiskEngine.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementsController : ControllerBase
    {
        private readonly IAgreementService _agreementService;

        public AgreementsController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        /// <summary>
        /// Belirli tenant'a ait anlaşmaları listeler.
        /// </summary>
        [HttpGet("{tenantId:int}")]
        public async Task<IActionResult> GetAll(int tenantId)
        {
            var agreements = await _agreementService.GetAllAsync(tenantId);
            return Ok(agreements);
        }

        /// <summary>
        /// Belirli tenant içindeki tek bir anlaşmanın detayını getirir.
        /// </summary>
        [HttpGet("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> GetById(int tenantId, int id)
        {
            var agreement = await _agreementService.GetByIdAsync(id, tenantId);

            if (agreement is null)
                return NotFound("Anlaşma bulunamadı.");

            return Ok(agreement);
        }

        /// <summary>
        /// Yeni anlaşma oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAgreementDto dto)
        {
            var id = await _agreementService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { tenantId = dto.TenantId, id },
                new { id });
        }

        /// <summary>
        /// Mevcut anlaşmayı günceller.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAgreementDto dto)
        {
            var updated = await _agreementService.UpdateAsync(dto);

            if (!updated)
                return NotFound("Güncellenecek anlaşma bulunamadı.");

            return Ok("Anlaşma başarıyla güncellendi.");
        }

        /// <summary>
        /// Anlaşmayı soft delete ile siler.
        /// </summary>
        [HttpDelete("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> Delete(int tenantId, int id)
        {
            var deleted = await _agreementService.DeleteAsync(id, tenantId);

            if (!deleted)
                return NotFound("Silinecek anlaşma bulunamadı.");

            return Ok("Anlaşma başarıyla silindi.");
        }
    }
}
