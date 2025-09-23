using InvoiceSystem.Models.Entity;
using System.Linq;

namespace InvoiceSystem.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<bool> HasActiveSubscriptionAsync(int customerId, int planId); //active subscription
        Task<int> CountActiveSubscriptionsForPlanAsync(int planId);
        Task AddAsync(Subscription subscription);

        // IQueryable for better server-side execution
        IQueryable<Subscription> GetAllActive();         // used for invoice generation
        IQueryable<Subscription> GetAllByCustomers(int customerId); // used for customer payments, discounts, etc.
    }
}
