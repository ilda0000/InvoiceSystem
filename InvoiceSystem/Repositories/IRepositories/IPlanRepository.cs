
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IPlanRepository
    {

        Task<List<Plan>> GetAllAsync();
        Task<Plan?> GetByIdAsync(int id);
        //Task<Plan?> FindAsync(Func<Plan, bool> predicate);
        //Task AddAsync(Plan entity); 

    }
}