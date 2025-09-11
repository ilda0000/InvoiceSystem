// File 11: CustomersController.cs
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Validators;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service) => _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CustomerDTO dto)
        {
            // Manual validation
            var validator = new CustomerValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            // Save customer
            var createdCustomer = await _service.AddCustomerAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return Ok(customer); // NotFound handled via NotFoundException
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }
    }
}
