using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IDiscountRepository
    {

        Task<Discount> GetBestApplicableDiscountAsync(int months);
        Task<List<Discount>> GetAllAsync();
        Task<Discount?> GetByNameAsync(string name);

    }
}