using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {

        private readonly AppDbContexts _context;
        public InvoiceRepository(AppDbContexts context) => _context = context;
        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }
        public async Task<bool> InvoiceExistsAsync(int subscriptionId, DateTime billingDate)
        {
            return await _context.Invoices.AnyAsync(i =>
                i.SubscriptionId == subscriptionId &&
                i.BillingDate.Year == billingDate.Year &&
                i.BillingDate.Month == billingDate.Month);
        }

        //Helper method to get invoices by customer ID 
        public async Task<List<Invoice>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Invoices
                .Include(i => i.Subscription)
                .Where(i => i.Subscription.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
