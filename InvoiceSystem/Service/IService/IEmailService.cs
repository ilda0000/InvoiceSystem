using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Service
{
    public interface IEmailService
    {
        Task SendInvoiceEmailAsync(string toEmail, Invoice invoice);
    }
}