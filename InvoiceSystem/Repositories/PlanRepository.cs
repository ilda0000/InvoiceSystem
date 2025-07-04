using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Repositories
{
    public class PlanRepository: IPlanRepository    
    {
        private readonly AppDbContexts _context;
        public PlanRepository(AppDbContexts context) => _context = context;

        public async Task<List<Plan>> GetAllAsync()
        {
            return await _context.Plans.ToListAsync();
        }
        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
