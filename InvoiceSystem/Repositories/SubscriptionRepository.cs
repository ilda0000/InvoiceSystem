using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using InvoiceSystem.Data;
using System.Linq;

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
            await _context.Subscriptions
                .AnyAsync(s => s.CustomerId == customerId && s.PlanId == planId && s.IsActive);

        public async Task<int> CountActiveSubscriptionsForPlanAsync(int planId) =>
            await _context.Subscriptions
                .CountAsync(s => s.PlanId == planId && s.IsActive);

        public async Task AddAsync(Subscription subscription) =>
            await _context.Subscriptions.AddAsync(subscription);

        // IQueryable implementations
        public IQueryable<Subscription> GetAllActive() =>
            _context.Subscriptions.Where(s => s.IsActive);

        public IQueryable<Subscription> GetAllByCustomers(int customerId) =>
            _context.Subscriptions.Where(s => s.CustomerId == customerId);
    }
}
