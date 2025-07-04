using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice);
        Task<bool> InvoiceExistsAsync(int subscriptionId, DateTime billingDate);
        Task<List<Invoice>> GetByCustomerIdAsync(int customerId);
    }
}