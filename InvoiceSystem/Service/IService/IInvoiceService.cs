using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> GenerateMonthlyInvoicesMinimalAsync();
        Task<List<InvoiceDTO>> GetInvoicesByCustomerAsync(int customerId);
    }
}