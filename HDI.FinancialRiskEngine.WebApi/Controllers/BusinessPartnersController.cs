using HDI.FinancialRiskEngine.Application.DTOs.BusinessPartners;
using HDI.FinancialRiskEngine.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnersController : ControllerBase
    {
        private readonly IBusinessPartnerService _service;

        public BusinessPartnersController(IBusinessPartnerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tenant'a ait partnerları listeler
        /// </summary>
        [HttpGet("{tenantId:int}")]
        public async Task<IActionResult> GetAll(int tenantId)
        {
            var result = await _service.GetAllAsync(tenantId);
            return Ok(result);
        }

        /// <summary>
        /// Tek partner getirir
        /// </summary>
        [HttpGet("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> GetById(int tenantId, int id)
        {
            var result = await _service.GetByIdAsync(id, tenantId);

            if (result is null)
                return NotFound("Partner bulunamadı.");

            return Ok(result);
        }

        /// <summary>
        /// Yeni partner oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessPartnerDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return Ok(new { id });
        }

        /// <summary>
        /// Partner günceller
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBusinessPartnerDto dto)
        {
            var updated = await _service.UpdateAsync(dto);

            if (!updated)
                return NotFound("Partner bulunamadı.");

            return Ok("Partner güncellendi.");
        }

        /// <summary>
        /// Partner siler (soft delete)
        /// </summary>
        [HttpDelete("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> Delete(int tenantId, int id)
        {
            var deleted = await _service.DeleteAsync(id, tenantId);

            if (!deleted)
                return NotFound("Partner bulunamadı.");

            return Ok("Partner silindi.");
        }

        /// <summary>
        /// ApiKey yeniler
        /// </summary>
        [HttpPost("regenerate-api-key/{tenantId:int}/{id:int}")]
        public async Task<IActionResult> RegenerateApiKey(int tenantId, int id)
        {
            var apiKey = await _service.RegenerateApiKeyAsync(id, tenantId);

            return Ok(new { apiKey });
        }
    }
}