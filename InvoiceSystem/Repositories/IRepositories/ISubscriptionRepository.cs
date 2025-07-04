using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<bool> HasActiveSubscriptionAsync(int customerId, int planId);
        Task<List<Subscription>> GetAllByCustomerAsync(int customerId);
        Task<int> CountActiveSubscriptionsForPlanAsync(int planId);
        Task AddAsync(Subscription subscription);

        Task<List<Subscription>> GetAllActiveAsync(); // used for invoice generation    
        Task<List<Subscription>> GetByCustomerIdAsync(int customerId); //used for customer details related to payments

    }
}