
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IPlanRepository
    {

        Task<List<Plan>> GetAllAsync();
        Task<Plan?> GetByIdAsync(int id);

    }
}