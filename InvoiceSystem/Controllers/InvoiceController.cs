using Microsoft.AspNetCore.Mvc;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Service;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // POST /api/invoices/generate
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMinimalInvoice()
        {
            var result = await _invoiceService.GenerateMonthlyInvoicesMinimalAsync();
            if (result == null)
                return NotFound(); // Or BadRequest(), or a custom message

            return Ok(result);
        }

        [HttpGet("/api/customers/{id}/invoices")]
        public async Task<IActionResult> GetByCustomer(int id)
        {
            var invoices = await _invoiceService.GetInvoicesByCustomerAsync(id);
            return Ok(invoices);
        }
    }
}
