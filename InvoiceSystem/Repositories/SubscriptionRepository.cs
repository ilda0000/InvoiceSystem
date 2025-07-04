using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Data; // Add this if AppDbContexts is in this namespace
    
namespace InvoiceSystem.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContexts _context;

        public SubscriptionRepository(AppDbContexts context)
        {
            _context = context;
        }

        public async Task<bool> HasActiveSubscriptionAsync(int customerId, int planId) =>
            await _context.Subscriptions.AnyAsync(s => s.CustomerId == customerId && s.PlanId == planId && s.IsActive);

        public async Task<List<Subscription>> GetAllByCustomerAsync(int customerId) =>
            await _context.Subscriptions.Where(s => s.CustomerId == customerId).ToListAsync();

        public async Task<int> CountActiveSubscriptionsForPlanAsync(int planId) =>
            await _context.Subscriptions.CountAsync(s => s.PlanId == planId && s.IsActive);

        public async Task AddAsync(Subscription subscription) =>
            await _context.Subscriptions.AddAsync(subscription);

        public async Task<List<Subscription>> GetAllActiveAsync() // This method retrieves all active subscriptions, useful for invoice generation  
        {
            return await _context.Subscriptions
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<List<Subscription>> GetByCustomerIdAsync(int customerId) //  for customer details related to payments   
        {
            // Implementation to fetch subscriptions by customer ID
            return await _context.Subscriptions
                .Where(s => s.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
