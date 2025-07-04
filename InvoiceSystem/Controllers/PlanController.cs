using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/plans")]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _service;
        public PlansController(IPlanService service) => _service = service;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _service.GetAllPlansAsync();
            return Ok(plans);
        }
    }
}