using FluentValidation;
using InvoiceSystem.Exceptions;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/plans")]
    public class PlansController : ControllerBase
    {
        private readonly IPlanService _service;
        private readonly IValidator<PlanDTO> _planValidator;

        public PlansController(IPlanService service, IValidator<PlanDTO> planValidator)
        {
            _service = service;
            _planValidator = planValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlanDTO planDto)
        {
            // Validation handled via FluentValidation middleware
            var validationResult = await _planValidator.ValidateAsync(planDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            return CreatedAtAction(nameof(GetById), new { id = planDto.Id }, planDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plans = await _service.GetAllPlansAsync();
            var plan = plans.FirstOrDefault(p => p.Id == id);

            if (plan == null)
                throw new NotFoundExceptions($"Plan with id {id} not found.");

            return Ok(plan);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _service.GetAllPlansAsync();
            return Ok(plans);
        }
    }
}
