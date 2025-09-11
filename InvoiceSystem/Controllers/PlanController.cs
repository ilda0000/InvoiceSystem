using InvoiceSystem.Service;
using InvoiceSystem.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using InvoiceSystem.ErrorMessages;


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
            
            var validationResult = await _planValidator.ValidateAsync(planDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    status = 400,
                    error = "BadRequest",
                    messages = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            // Check for duplicate plan name
            PlanDTO existingPlan = await _service.GetByNameAsync(planDto.Name);
            if (existingPlan != null)
            {
                return Conflict(new
                {
                    status = 409,
                    error = "Conflict",
                   message = PlanError.PlanDuplicateName

                });

            }

            return CreatedAtAction(nameof(GetById), new { id = planDto.Id }, planDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var plans = await _service.GetAllPlansAsync();
            var plan = plans.FirstOrDefault(p => p.Id == id);
            if (plan == null)
            {
                return NotFound(new
                {
                    status = 404,
                    error = "NotFound",
                    message = PlanError.PlanNotFound
                });
            }
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
