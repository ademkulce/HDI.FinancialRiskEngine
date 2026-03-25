using HDI.FinancialRiskEngine.Application.DTOs.RiskAnalyses;
using HDI.FinancialRiskEngine.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HDI.FinancialRiskEngine.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskAnalysesController : ControllerBase
    {
        private readonly IRiskAnalysisService _service;

        public RiskAnalysesController(IRiskAnalysisService service)
        {
            _service = service;
        }

        /// <summary>
        /// Belirli bir BusinessTopic kaydına ait risk analiz sonucunu getirir.
        /// </summary>
        [HttpGet("{tenantId:int}/business-topic/{businessTopicId:int}")]
        public async Task<IActionResult> GetByBusinessTopicId(int tenantId, int businessTopicId)
        {
            var result = await _service.GetByBusinessTopicIdAsync(businessTopicId, tenantId);

            if (result is null)
                return NotFound("Risk analizi bulunamadı.");

            return Ok(result);
        }

        /// <summary>
        /// Belirli bir BusinessTopic için risk analizini çalıştırır ve sonucu kaydeder.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRiskAnalysisDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Risk analiz kaydını siler.
        /// </summary>
        [HttpDelete("{tenantId:int}/{id:int}")]
        public async Task<IActionResult> Delete(int tenantId, int id)
        {
            var deleted = await _service.DeleteAsync(id, tenantId);

            if (!deleted)
                return NotFound("Silinecek risk analizi bulunamadı.");

            return Ok("Risk analizi başarıyla silindi.");
        }
    }
}