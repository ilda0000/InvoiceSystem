using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContexts _context;
        public CustomerRepository(AppDbContexts context) => _context = context;

        public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);
        public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
