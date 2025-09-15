using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContexts _context;
        public PaymentRepository(AppDbContexts context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
    }
}
