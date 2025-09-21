
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
            _logger = logger;
        }

        // POST /api/invoices/generate
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMinimalInvoice()
        {
            
                var invoice = await _invoiceService.GenerateMonthlyInvoicesMinimalAsync();
                if (invoice == null)
                {
                    return NotFound("No active subscriptions found for invoice generation.");
                }
                return Ok(invoice);
            
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<InvoiceDTO>>> GetCustomerInvoices(int customerId)
        {// TODO :  is used try - catch block?? remove
            try
            {
                var invoices = await _invoiceService.GetInvoicesByCustomerAsync(customerId);
                if (!invoices.Any())
                {
                    return NotFound($"No invoices found for customer {customerId}");
                }
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoices for customer {CustomerId}", customerId);
                return StatusCode(500, "An error occurred while retrieving invoices.");
            }
        }
    }
}
