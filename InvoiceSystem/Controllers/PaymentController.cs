using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InvoiceSystem.Service;
using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("auto")]
        public async Task<IActionResult> AutoRegisterPayment()
        {
            _logger.LogInformation("Attempting to automatically register a payment...");

            var result = await _paymentService.AutoRegisterPaymentAsync();
            if (result == null)
            {
                _logger.LogWarning("No unpaid invoices found or no active payment method.");
                return NotFound("No unpaid invoice or active subscription found.");
            }

            _logger.LogInformation("Payment registered successfully for InvoiceId: {InvoiceId}", result.InvoiceId);
            return Ok(result);
        }
    }
}
