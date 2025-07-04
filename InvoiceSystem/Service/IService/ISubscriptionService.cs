using InvoiceSystem.Models.DTO;

namespace InvoiceSystem.Service
{
    public interface ISubscriptionService
    {
        Task<bool> CreateSubscriptionAsync(SubscriptionDTO dto);
        Task<List<SubscriptionDTO>> GetSubscriptionsByCustomerAsync(int customerId);

    }
}