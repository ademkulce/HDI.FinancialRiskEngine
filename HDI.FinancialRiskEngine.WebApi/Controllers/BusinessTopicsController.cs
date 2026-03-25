using HDI.FinancialRiskEngine.Application.DTOs.BusinessTopics;
using HDI.FinancialRiskEngine.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessTopicsController : ControllerBase
    {
        private readonly IBusinessTopicService _service;

        public BusinessTopicsController(IBusinessTopicService service)
        {
            _service = service;
        }

        /// <summary>
        /// Belirli tenant'a ait iş konularını listeler.
        /// </summary>
        [HttpGet("{tenantId:int}")]
        public async Task<IActionResult> GetAll(int tenantId)
        {
            var result = await _service.GetAllAsync(tenantId);
            return Ok(result);
        }

        /// <summary>
        /// Belirli tenant içindeki tek bir iş konusunun detayını getirir.
        /// </summary>
        [HttpGet("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> GetById(int tenantId, int id)
        {
            var result = await _service.GetByIdAsync(id, tenantId);

            if (result is null)
                return NotFound("İş Konusu bulunamadı.");

            return Ok(result);
        }

        /// <summary>
        /// Yeni iş konusu oluşturur.
        /// Yeni kayıt ilk aşamada Pending statüsünde oluşur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessTopicDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { tenantId = dto.TenantId, id },new { id });
        }

        /// <summary>
        /// Mevcut iş konusunu günceller.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBusinessTopicDto dto)
        {
            var updated = await _service.UpdateAsync(dto);

            if (!updated)
                return NotFound("Güncellenecek İş Konusu bulunamadı.");

            return Ok("İş Konusu başarıyla güncellendi.");
        }

        /// <summary>
        /// İş konusunu soft delete ile siler.
        /// </summary>
        [HttpDelete("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> Delete(int tenantId, int id)
        {
            var deleted = await _service.DeleteAsync(id, tenantId);

            if (!deleted)
                return NotFound("Silinecek İş Konusu bulunamadı.");

            return Ok("İş Konusu başarıyla silindi.");
        }
    }
}