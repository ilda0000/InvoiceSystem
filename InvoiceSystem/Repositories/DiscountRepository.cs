using InvoiceSystem.Data;
using InvoiceSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InvoiceSystem.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContexts _context;

        public DiscountRepository(AppDbContexts context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount> GetBestApplicableDiscountAsync(int months)
        {
            // Get the best discount where MinMonthsRequired <= months
            return await _context.Discounts
                .Where(d => d.MinMonthsRequired <= months)
                .OrderByDescending(d => d.Value) 
                .FirstOrDefaultAsync();
        }

        public async Task<Discount?> GetByNameAsync(string name)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(d => d.MinMonthsRequired == 0 && d.Name == name);
        }
        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }
    }
}