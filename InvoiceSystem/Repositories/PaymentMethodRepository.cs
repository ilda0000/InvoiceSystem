using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository 
    {

        private readonly AppDbContexts _context;
        public PaymentMethodRepository(AppDbContexts context) => _context = context;

        public async Task<List<PaymentMethod>> GetAllAsync()
            => await _context.PaymentMethods.Where(p => p.IsActive).ToListAsync();

        public async Task<PaymentMethod?> GetByIdAsync(int id)
            => await _context.PaymentMethods.FindAsync(id);

        public async Task<PaymentMethod?> GetByNameAsync(string name)
            => await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Name == name);
    }
}
