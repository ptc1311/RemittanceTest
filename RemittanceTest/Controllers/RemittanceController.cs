using Microsoft.AspNetCore.Mvc;
using RemittanceTest.Models;
using RemittanceTest.Services;

namespace RemittanceTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemittanceController : ControllerBase
    {
        private readonly IRemittanceService _service; //TC+++

        // TODO: 1. 請透過建構子注入 (Constructor Injection) 引入 IRemittanceService
        // Constructor Injection, TC+++:start
        public RemittanceController(IRemittanceService service)
        {
            _service = service;
        } //TC+++:end

        [HttpPost("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            // TODO: 2. 呼叫 Service 執行取消邏輯
            // TODO: 3. 根據 Service 回傳的結果，回傳相對應的 HTTP 狀態碼 (Ok / BadRequest / NotFound)
            // tc+++:start
            if (id <= 0)
                return BadRequest("Invalid id");

            var (isSuccess, message) = _service.CancelRemittance(id);

            if (isSuccess)
                return Ok(new { message = "Cancelled successfully" });

            return message switch
            {
                "NOT_FOUND" => NotFound(), //404
                "INVALID_STATUS" => BadRequest("Status not allowed"), //400
                _ => StatusCode(500)
            };
            //tc+++:end
            //return Ok();
        }
    }
}
