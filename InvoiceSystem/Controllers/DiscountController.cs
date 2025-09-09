using InvoiceSystem.Models.DTO;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountDTO dto)
        {
            // Only Name and Type are accepted from the client
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Type))
                return BadRequest("Name and Type are required.");

            // Value and MinMonthsRequired are set/calculated automatically in the service or repository
            await _discountService.CreateAsync(new DiscountDTO
            {
                Name = dto.Name,
                Type = dto.Type
                // Value and MinMonthsRequired are ignored here
            });

            return Ok("Discount created successfully.");
        }
    }
}