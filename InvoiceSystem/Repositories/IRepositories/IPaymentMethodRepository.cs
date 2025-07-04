using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod?> GetByIdAsync(int id);
        Task<PaymentMethod?> GetByNameAsync(string name);
    }
}