using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceSystem.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly AppDbContexts _context;

        public PlanRepository(AppDbContexts context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Plan?> GetByNameAsync(string name)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task AddAsync(Plan entity)
        {
            await _context.Plans.AddAsync(entity);
        }
    }
}
