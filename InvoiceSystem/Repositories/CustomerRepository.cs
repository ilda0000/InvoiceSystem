// File 8: CustomerRepository.cs
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

        public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

        public async Task<(bool NameTaken, bool EmailTaken)> WhichExistAsync(string name, string email)
        {
            var hits = await _context.Customers
                .Where(c => c.Name == name || c.Email == email)
                .Select(c => new { c.Name, c.Email })
                .ToListAsync();

            var nameTaken = hits.Any(h => h.Name == name);
            var emailTaken = hits.Any(h => h.Email == email);
            return (nameTaken, emailTaken);
        }
    }
}
